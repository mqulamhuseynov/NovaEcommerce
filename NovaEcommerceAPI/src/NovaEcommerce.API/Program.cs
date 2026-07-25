using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NovaEcommerce.API.Middleware;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.DataAccess.Repositories.Implementations;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.FluentValidation;
using NovaEcommerce.ServicesApp.FluentValidations;
using NovaEcommerce.ServicesApp.Services.Implementations;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Text;
using System.Text.Json;

namespace NovaEcommerce.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFluentValidationAutoValidation();

        builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
        
        builder.Services.AddValidatorsFromAssemblyContaining<LoginValidator>();

        Env.Load();

        var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                // Uzaq server kəsilmələri üçün yenidən cəhd mexanizmi:
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null);

                // Şəbəkə gecikmələri üçün gözləmə müddətini 60 saniyəyə qaldırırıq:
                sqlOptions.CommandTimeout(60);
            }));

        builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        var jwtSecret = builder.Configuration["JwtSettings:Secret"];
            

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSecret!))
                    };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        var result = JsonSerializer.Serialize(new
                        {
                            statusCode = StatusCodes.Status401Unauthorized,
                            message = "unauthorized"
                        });

                        await context.Response.WriteAsync(result);
                    },
                    OnForbidden = async context =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;

                        var result = JsonSerializer.Serialize(new
                        {
                            statusCode = StatusCodes.Status403Forbidden,
                            message = "forbidden"
                        });

                        await context.Response.WriteAsync(result);
                    }
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        builder.Services.AddScoped<IHomeRepository, HomeRepository>();

        builder.Services.AddScoped<IFlashSaleNotifyRepository, FlashSaleNotifyRepository>();
        builder.Services.AddScoped<IFlashSaleRepository, FlashSaleRepository>();
        builder.Services.AddScoped<IFlashSaleService, FlashSaleService>();

        builder.Services.AddScoped<ICheckoutShippingRepository, CheckoutShippingRepository>();
        builder.Services.AddScoped<ICheckoutShippingService, CheckoutShippingService>();
        builder.Services.AddScoped<ICheckoutPaymentRepository, CheckoutPaymentRepository>();
        builder.Services.AddScoped<ICheckoutPaymentService, CheckoutPaymentService>();
        builder.Services.AddScoped<ICheckoutSummaryRepository, CheckoutSummaryRepository>();
        builder.Services.AddScoped<ICheckoutSummaryService, CheckoutSummaryService>();
        builder.Services.AddScoped<IPlaceOrderRepository, PlaceOrderRepository>();
        builder.Services.AddScoped<IPlaceOrderService, PlaceOrderService>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IReviewService, ReviewService>();

        builder.Services.AddScoped<IReturnRequestRepository, ReturnRequestRepository>();
        builder.Services.AddScoped<IReturnRequestService, ReturnRequestService>();
        builder.Services.AddScoped<IAddressService, AddressService>();  

        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddScoped<ICartService, CartService>();
        builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFileStorageService, FileStorageService>();
        builder.Services.AddScoped<IWishlistService, WishlistService>();

        builder.Services.AddControllers();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowTester", policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("X-Session-Id"));
        });

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NovaEcommerce.API",
                Version = "v1"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();


        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseCors("AllowTester");

        app.MapControllers();

        app.MapGet("/", () => Results.Redirect("/swagger"));

        app.Run();
    }
}