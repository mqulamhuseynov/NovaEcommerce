# NovaEcommerce API

NovaEcommerce — geyim/moda yönümlü onlayn mağaza üçün hazırlanmış **ASP.NET Core 8** əsaslı backend API-dir. Layihə **Clean Architecture** prinsiplərinə uyğun olaraq qatlara bölünüb və autentifikasiya, məhsul kataloqu, brendlər, səbət (qonaq daxil olmaqla), wishlist, flash sale-lər, checkout, sifariş idarəetməsi, rəylər (review), geri qaytarma (return), ünvan və ödəniş üsulları kimi tam bir e-ticarət funksionallığını əhatə edir.

## İçindəkilər

- [Texnologiyalar](#texnologiyalar)
- [Layihə strukturu](#layihə-strukturu)
- [Domain modeli](#domain-modeli)
- [Əsas funksionallıqlar](#əsas-funksionallıqlar)
- [Quraşdırma](#quraşdırma)
- [Konfiqurasiya](#konfiqurasiya)
- [Miqrasiyalar (Migrations)](#miqrasiyalar-migrations)
- [Autentifikasiya](#autentifikasiya)
- [Qonaq (Guest) Səbət Mexanizmi](#qonaq-guest-səbət-mexanizmi)
- [Checkout Axını](#checkout-axını)
- [API Endpoint-ləri](#api-endpointləri)
- [Xəta İdarəetməsi](#xəta-idarəetməsi)
- [Swagger](#swagger)


## Texnologiyalar

- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core 8.0.28** (SQL Server provider, retry-on-failure aktiv)
- **ASP.NET Core Identity** — `AppUser : IdentityUser<int>` üzərində qurulmuş fərdiləşdirilmiş istifadəçi modeli (`int` tipli Id, `users` cədvəli)
- **JWT Bearer Authentication** + Refresh Token mexanizmi
- **FluentValidation** (`FluentValidation.AspNetCore`, avtomatik validasiya aktivləşdirilib)
- **Swagger / Swashbuckle** (Bearer token dəstəyi ilə)


## Layihə strukturu

Solution 4 əsas layihədən ibarətdir:

```
NovaEcommerceAPI/
└── src/
    ├── NovaEcommerce.API/            # Controller-lər, Program.cs, middleware, appsettings
    ├── NovaEcommerce.ServicesApp/    # Servislər, DTO-lar, interfeyslər, FluentValidation qaydaları
    ├── NovaEcommerce.DataAccess/     # DbContext, EF Core Migrations, Repository implementasiyaları, Entity konfiqurasiyaları
    └── NovaEcommerce.Domain/         # Entity-lər və Enum-lar (heç bir asılılığı olmayan "core" qat)
```

Qatlar arasında asılılıq istiqaməti: **API → ServicesApp → DataAccess → Domain**. Bu, biznes qaydalarının (`ServicesApp`) konkret verilənlər bazası texnologiyasından asılı olmadan, yalnız interfeyslər (`Services.Interfaces.Repository`, `Services.Interfaces.Service`) vasitəsilə işləməsinə imkan verir. Hər servis üçün ayrıca interfeys və implementasiya mövcuddur (məs. `ICartService` / `CartService`, `IWishlistRepository` / `WishlistRepository`).

## Domain modeli

Əsas entity-lər (`NovaEcommerce.Domain.Entities`):

| Entity | Qeyd |
|---|---|
| `AppUser` | Identity istifadəçisi; `MemberTier` (Bronze/Silver/Gold), `IsGuest`, avatar |
| `Product`, `ProductVariant`, `ProductImage`, `ProductTag` | Məhsul → rəng/ölçü üzrə variant (hər biri öz SKU, qiymət, stok sayı ilə) |
| `Brand`, `Category` | Kateqoriyalar iyerarxik (`ParentCategoryId`) |
| `Cart`, `CartItem` | Həm `UserId`, həm də `SessionId` ilə əlaqələndirilə bilər (qonaq səbəti) |
| `Wishlist`, `WishlistItem` | Paylaşım tokeni (`ShareToken`), qiymət düşüşü izləmə (`PriceAtAdd`) |
| `FlashSale`, `FlashSaleItem`, `NotifyRequest` | Endirim kampaniyaları və stok bitdikdə bildiriş sorğuları |
| `CheckoutSession` | Çatdırılma + ödəniş məlumatlarının müvəqqəti saxlanması (`ExpiresAt` ilə) |
| `Order`, `OrderItem`, `OrderStatusHistory` | Sifariş və snapshot-based tarixçə (qiymət/ünvan/kart məlumatları sifariş anında "dondurulur") |
| `Review` | Yalnız `OrderItemId` üzərindən unikal — "verified purchase" modeli |
| `ReturnRequest` | Geri qaytarma/dəyişdirmə, `Photos` JSON kimi saxlanılır |
| `Address`, `PaymentMethod` | İstifadəçiyə aid, defolt seçim dəstəyi |
| `Coupon` | Faiz və ya sabit endirim, minimum sifariş məbləği şərti |
| `Notification`, `HeroBanner`, `Faq`, `SupportArticle` | Yardımçı/marketinq məzmunu |

> Qeyd: `Photos` (Review, ReturnRequest) verilənlər bazasında `List<string>` → JSON sətri kimi saxlanılır (`JsonListConverter`, xüsusi `ValueComparer` ilə).

## Əsas funksionallıqlar

| Modul | Təsvir |
|---|---|
| **Auth** | Qeydiyyat, giriş, JWT access/refresh token, logout, cari istifadəçi (`/me`) |
| **Users** | Profil yeniləmə, şifrə dəyişdirmə, avatar yükləmə |
| **Products / Home** | PLP (filtr + sıralama + səhifələmə), məhsul detalları, uyğun məhsullar, kateqoriyalar, hero banner-lər, seçilmiş məhsullar |
| **Brands** | Brend səhifəsi, rəng/ölçü/qiymət filtri, bestseller-lər |
| **Cart** | Qonaq (session-based) və qeydiyyatlı istifadəçi səbəti, məhsul əlavə/yenilə/sil, "sonra üçün saxla", kupon tətbiqi, giriş zamanı qonaq səbətinin birləşdirilməsi |
| **Wishlist** | Çoxlu wishlist, məhsul əlavə/sil, stokda olmayan məhsul üçün bildiriş, qiymət düşüşü izləmə, paylaşıla bilən link |
| **Flash Sales** | Aktiv və gələcək endirim kampaniyaları, bildiriş abunəliyi |
| **Checkout** | Çatdırılma məlumatları → ödəniş → xülasə → sifarişin təsdiqlənməsi (tranzaksiya daxilində, çoxaddımlı `CheckoutSession` əsasında) |
| **Orders** | Sifariş tarixçəsi, detallar, izləmə (tracking), təkrar sifariş (reorder), invoys, status irəliləmə |
| **Reviews** | Yalnız təsdiqlənmiş alış (verified purchase) əsasında rəy yazma, məhsul üzrə rəylərin siyahısı |
| **Return Requests** | Geri qaytarma/dəyişdirmə tələbi, şəkil yükləmə |
| **Addresses / Payment Methods** | Ünvan və ödəniş üsullarının idarə edilməsi, defolt seçim |

## Quraşdırma

### Tələblər

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (yerli və ya uzaq instans)
- (İstəyə bağlı) Visual Studio 2022 / Rider / VS Code

### Addımlar

```bash
git clone <repo-url>
cd NovaEcommerceAPI

# Paketlərin bərpası
dotnet restore

# Miqrasiyaların tətbiqi (bax: aşağıdakı "Miqrasiyalar" bölməsi)
dotnet ef database update --project src/NovaEcommerce.DataAccess --startup-project src/NovaEcommerce.API

# Layihənin işə salınması
dotnet run --project src/NovaEcommerce.API
```

İşə düşdükdən sonra API defolt olaraq aşağıdakı ünvanlarda əlçatan olacaq (bax `launchSettings.json`):

- `http://localhost:5166`
- `https://localhost:7216`

Kök ünvan (`/`) avtomatik olaraq `/swagger`-ə yönləndirilir.

## Konfiqurasiya

Layihə həssas məlumatlar üçün `.env` faylını (`DotNetEnv.Env.Load()`) və `appsettings.json`-u birlikdə istifadə edir. Kök qovluqda `.env` faylı yaradın və minimum aşağıdakı dəyişənləri təyin edin:

```
ConnectionStrings__DatabaseConnection=Server=...;Database=NovaEcommerce;...
JwtSettings__Secret=<minimum 32 simvollu güclü açar>
JwtSettings__AccessExpires=15m
```

`appsettings.json`-da olan digər konfiqurasiyalar:

```json
{
  "Cart": {
    "FreeShippingThreshold": 100,
    "ShippingCost": 10
  },
  "App": {
    "BaseUrl": "auyescommerce.az"
  }
}
```

- `Cart:FreeShippingThreshold` / `Cart:ShippingCost` — səbət xülasəsində pulsuz çatdırılma həddi və standart çatdırılma haqqı.
- `App:BaseUrl` — wishlist paylaşım linklərinin (`/wishlist/shared/{token}`) qurulması üçün istifadə olunur.

> **Qeyd:** `JwtSettings:Secret` boş olarsa tətbiq işə düşən zaman istisna atacaq (bax `JwtService.GenerateAccessToken`).

## Miqrasiyalar (Migrations)

Verilənlər bazası sxemi EF Core Code-First miqrasiyaları ilə idarə olunur (`NovaEcommerce.DataAccess/Migrations`).

Yeni miqrasiya əlavə etmək üçün:

```bash
dotnet ef migrations add <MiqrasiyaAdi> \
  --project src/NovaEcommerce.DataAccess \
  --startup-project src/NovaEcommerce.API
```

Miqrasiyaları bazaya tətbiq etmək üçün:

```bash
dotnet ef database update \
  --project src/NovaEcommerce.DataAccess \
  --startup-project src/NovaEcommerce.API
```

Identity cədvəlləri fərdiləşdirilib: istifadəçi cədvəli `AspNetUsers` əvəzinə **`users`** adlanır, sifariş status tarixçəsi isə **`order_status_history`** adlı cədvəldə saxlanılır.

## Autentifikasiya

- Giriş/qeydiyyatdan sonra **access token** (JWT, defolt 15 dəqiqə) və **refresh token** (7 gün, verilənlər bazasında saxlanılır) qaytarılır.
- Access token bitdikdə `/api/auth/refresh-token` endpoint-i ilə yeni cüt token almaq mümkündür.
- Qorunan endpoint-lərə sorğu göndərərkən `Authorization: Bearer <access_token>` header-i tələb olunur.
- `ClaimTypes.NameIdentifier` claim-i istifadəçinin `Id`-sini ehtiva edir və controller-lərdə `int userId` kimi oxunur.
- Parolun tələbləri (`Program.cs`-də konfiqurasiya olunub): minimum 6 simvol, rəqəm/böyük hərf/kiçik hərf/xüsusi simvol məcburi deyil.

## Qonaq (Guest) Səbət Mexanizmi

Səbət funksionallığı həm qeydiyyatlı, həm də qonaq istifadəçilər üçün işləyir:

- Qonaq istifadəçi üçün `X-Session-Id` başlığı (header) istifadə olunur. Əgər header göndərilməyibsə, server yeni bir GUID yaradıb cavabda geri qaytarır.
- İstifadəçi giriş etdikdə, `POST /api/cart/merge` endpoint-i qonaq səbətini istifadəçinin hesabına köçürür/birləşdirir (uyğun gələn məhsullar üçün say cəmlənir, stok həddi ilə məhdudlaşdırılır).

## Checkout Axını

Checkout prosesi çoxaddımlı `CheckoutSession` modelinə əsaslanır:

1. **`POST /api/checkout/shipping`** — çatdırılma məlumatları saxlanılır, `CheckoutSessionId` qaytarılır (session 1 saat etibarlıdır — `ExpiresAt`).
2. **`POST /api/checkout/payment`** 🔒 — ödəniş növü (`Card` / `PayPal` / `Klarna`) və (kart seçilibsə) kart məlumatları saxlanılır.
3. **`GET /api/checkout/summary/{checkoutSessionId}`** — çatdırılma, ödəniş və qiymət xülasəsi qaytarılır.
4. **`POST /api/checkout/place-order`** 🔒 — sifariş tranzaksiya daxilində yaradılır: stok azaldılır, `OrderItem`/`OrderStatusHistory` yaradılır, səbət və `CheckoutSession` silinir.

## API Endpoint-ləri

Aşağıda əsas controller-lər üzrə qısa xülasə verilib (tam siyahı üçün Swagger-ə baxın).

### Auth — `api/auth`
| Metod | Endpoint | Təsvir |
|---|---|---|
| POST | `/register` | Yeni istifadəçi qeydiyyatı |
| POST | `/login` | Giriş |
| POST | `/refresh-token` | Access token-in yenilənməsi |
| POST | `/logout` | Çıxış (refresh token-in ləğvi) |
| GET | `/me` 🔒 | Cari istifadəçi məlumatı |

### Users — `api/users` 🔒
| Metod | Endpoint | Təsvir |
|---|---|---|
| PATCH | `/profile` | Profilin yenilənməsi |
| PATCH | `/password` | Şifrənin dəyişdirilməsi |
| POST | `/avatar` | Avatar şəklinin yüklənməsi |

### Products — `api/Products`
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/` | Filtrlənmiş və səhifələnmiş məhsul siyahısı (PLP) — kateqoriya, rəng, ölçü, qiymət aralığı, sıralama |
| GET | `/{id}` | Məhsulun detalları (variantlar, rəylər, uyğun məhsullar) |

### Brands — `api/brands`
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/{slug}` | Brend səhifəsi (rəng/ölçü/qiymət filtri, bestseller-lər) |

### Home — `api/home`
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/hero-banners` | Ana səhifə banner-ləri |
| GET | `/curated-picks` | Seçilmiş məhsullar |
| GET | `/categories` | Ana kateqoriyalar |

### Cart — `api/cart`
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/` | Səbətin alınması |
| POST | `/items` | Məhsulun səbətə əlavə edilməsi |
| PATCH | `/items/{id}` | Say (quantity) yeniləmə |
| DELETE | `/items/{id}` | Məhsulun silinməsi |
| PATCH | `/items/{id}/save-for-later` | "Sonra üçün saxla" statusu |
| POST | `/apply-coupon` | Kupon tətbiqi |
| GET | `/summary` | Qiymət xülasəsi (subtotal, çatdırılma, endirim, cəm) |
| POST | `/merge` 🔒 | Qonaq səbətinin hesaba birləşdirilməsi |

### Wishlist — `api/wishlists` 🔒
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/` | Wishlist-lərin siyahısı |
| POST | `/` | Yeni wishlist yaradılması |
| GET | `/{id}/items` | Wishlist elementləri |
| POST | `/{id}/items` | Element əlavə edilməsi |
| DELETE | `/{id}/items/{itemId}` | Elementin silinməsi |
| POST | `/{id}/items/{itemId}/notify` | Stok bildirişi tələbi |
| GET | `/{id}/share` | Paylaşım linkinin alınması |

### Flash Sales — `api/flash-sales`
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/active` | Aktiv kampaniya |
| GET | `/upcoming` | Gələcək kampaniyalar |
| POST | `/{id}/notify` 🔒 | Bildiriş abunəliyi |

### Checkout — `api/checkout`
| Metod | Endpoint | Təsvir |
|---|---|---|
| POST | `/shipping` | Çatdırılma məlumatlarının saxlanması |
| POST | `/payment` 🔒 | Ödəniş məlumatlarının saxlanması |
| GET | `/summary/{checkoutSessionId}` | Sifariş xülasəsi |
| POST | `/place-order` 🔒 | Sifarişin təsdiqlənməsi |

### Orders — `api/orders` 🔒
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/` | Sifarişlərin siyahısı (status/axtarış/səhifələmə) |
| GET | `/{orderNumber}` | Sifariş detalları |
| GET | `/{orderNumber}/tracking` | İzləmə tarixçəsi |
| POST | `/{orderId}/reorder` | Təkrar sifariş |
| GET | `/{orderNumber}/invoice` | İnvoys |
| POST | `/{orderNumber}/advance-status` | Statusun irəlilədilməsi |

### Reviews — `api/reviews`, `api/products/{id}/reviews`
| Metod | Endpoint | Təsvir |
|---|---|---|
| POST | `/api/reviews` 🔒 | Rəyin yaradılması (yalnız satın alınmış məhsul üçün) |
| GET | `/api/products/{id}/reviews` | Məhsul üzrə rəylər |

### Return Requests — `api/return-requests` 🔒
| Metod | Endpoint | Təsvir |
|---|---|---|
| POST | `/` | Geri qaytarma/dəyişdirmə tələbi |
| GET | `/` | Tələblərin siyahısı |
| GET | `/{id}` | Tələbin detalları |
| POST | `/upload` | Şəkil yükləmə |

### Addresses — `api/addresses` 🔒
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/` | Ünvanların siyahısı |
| POST | `/` | Yeni ünvan |
| PATCH | `/{addressId}/set-default` | Defolt ünvan seçimi |
| DELETE | `/{addressId}` | Ünvanın silinməsi |

### Payment Methods — `api/payment-methods` 🔒
| Metod | Endpoint | Təsvir |
|---|---|---|
| GET | `/` | Ödəniş üsullarının siyahısı |
| POST | `/` | Yeni kart əlavə edilməsi |
| PATCH | `/{id}/set-default` | Defolt kart seçimi |
| DELETE | `/{id}` | Kartın silinməsi |

🔒 — JWT ilə autentifikasiya tələb edir.

## Xəta İdarəetməsi

Bütün endpoint-lər `ExceptionMiddleware` vasitəsilə vahid formatda cavab qaytarır:

```json
{
  "statusCode": 400,
  "message": "..."
}
```

| İstisna tipi | HTTP Status |
|---|---|
| `UnauthorizedAccessException` | 401 |
| `InvalidOperationException` | 409 |
| `KeyNotFoundException` | 404 |
| Digər bütün istisnalar | 400 |

> ⚠️ Hazırkı implementasiyada `Exception.Message` birbaşa cavaba yazılır — bu, EF Core-un xam SQL xətalarının müştəriyə sızmasına səbəb ola bilər (bax aşağıdakı bilinən problemlər bölməsi).

## Swagger

Tətbiq işə düşdükdən sonra API sənədləşdirməsinə aşağıdakı ünvandan baxmaq olar:

```
/swagger
```

JWT ilə qorunan endpoint-ləri test etmək üçün Swagger-də "Authorize" düyməsi vasitəsilə `Bearer <token>` formatında access token daxil edin.

## Bilinən Problemlər və Təkmilləşdirmə Planı

Layihə üzərində aparılan kod review-u zamanı aşkarlanmış, hələ həll edilməmiş məsələlər. Yeni töhfə verərkən bu siyahını nəzərə alın.

### 🔴 Kritik təhlükəsizlik

- **Checkout summary IDOR** — `GET /api/checkout/summary/{checkoutSessionId}` autentifikasiya tələb etmir və `checkoutSessionId`-nin sorğunu göndərən istifadəçiyə aid olduğunu yoxlamır.
- **Checkout payment** — endpoint sessiya sahibliyini yoxlamadan `CheckoutPaymentRequestDto.CheckoutSessionId`-ni qəbul edir.
- **Xam SQL xətalarının sızması** — `ExceptionMiddleware` ümumi `Exception`-ları 400 kodu ilə `Message` daxilində qaytarır ki, bu da EF Core səviyyəsindəki xətaları (cədvəl/sütun adları və s.) müştəriyə göstərə bilər.
- **Fayl yükləmə validasiyası** — həm avatar (`FileStorageService`), həm də return-request şəkilləri (`ReturnRequestService.UploadPhotoAsync`) yalnız fayl uzantısını yoxlayır, magic-byte/əsl MIME tipi yoxlaması yoxdur.

### 🟠 Yarımçıq axın

- **Qonaq checkout** — səbət və çatdırılma qatlarında qonaq dəstəklənsə də, `CheckoutController.Payment` `[Authorize]` ilə işarələnib və `PlaceOrderService.PlaceOrderAsync(int userId)` yalnız `int userId` qəbul edir — yəni qonaq istifadəçi ödəniş və sifariş mərhələsində bloklanır.

### 🟡 Biznes məntiqi

- **Checkout summary-də endirim həmişə 0** (`CheckoutSummaryService.GetSummaryAsync` — `discount = 0`), amma sifariş yaradılarkən (`PlaceOrderService`) kupon tətbiq olunur → müştəriyə göstərilən qiymətlə faktiki ödəniş fərqli ola bilər.
- **Sabit məbləğli kupon mənfi cəmə səbəb ola bilər** — `CartService.BuildSummary`-də `Math.Min(coupon.DiscountValue, subtotal)` tətbiq olunsa da, `PlaceOrderService`-də bu məhdudiyyət yoxdur.
- **`CardValidator` bütün ödəniş növlərinə tətbiq olunur** — kart/CVV sahələrini şərtsiz məcburi edir, nəticədə PayPal və Klarna ödənişləri bloklanır.
- **Stok azalması üçün concurrency token yoxdur** (`ProductVariant.StockQuantity`) — paralel sifarişlərdə overselling riski.
- **Flash sale `SoldCount`** heç vaxt artırılmır.
- **`OrdersController.AdvanceStatus`** hər hansı autentifikasiya olunmuş istifadəçi tərəfindən çağırıla bilər — sifariş sahibliyi yoxlanılmır (yalnız `orderNumber` ilə axtarılır, amma servis daxilində `userId` ilə uzlaşma yoxlanılsa da, endpoint-in adı və məqsədi admin/sistem əməliyyatına bənzəyir — icazə modelinin aydınlaşdırılması lazımdır).

### 🟢 Validasiya boşluqları

Aşağıdakı DTO-lar üçün FluentValidation qaydaları yoxdur (nəticədə xam EF constraint istisnaları generik 400 xətası kimi görünür):

- `CreateAddressDto`
- `CreatePaymentMethodDto`
- `CreateWishlistRequestDto`
- `CreateReviewDto`
- `CreateRequestReturnDto`

### 🔵 Performans

- **`BrandRepository.GetBrandAsync`** brendin bütün məhsul kataloqunu yaddaşa yükləyir, filtrasiya (`BrandService`) yalnız bundan sonra yaddaşda aparılır.
- Bir neçə yerdə (`CheckoutSummaryRepository`, `PlaceOrderRepository` və s.) çoxlu kolleksiya `.Include()` zənciri `.AsSplitQuery()` istifadə etmədən aparılır — cartesian explosion riski.

### ⚪ Lokalizasiya

- **`UserService.UpdateProfileAsync`**-da e-poçtun normallaşdırılması üçün `dto.Email.ToUpper()` istifadə olunur — Azərbaycan/Türk mədəni mühitində "dotted-I" davranışı səbəbindən (`i` → `İ` yox, `I`) səhv nəticələr verə bilər. `ToUpperInvariant()` istifadə edilməlidir.

---

*Bu README layihənin hazırkı vəziyyətini əks etdirir və kod dəyişdikcə yenilənməlidir.*
