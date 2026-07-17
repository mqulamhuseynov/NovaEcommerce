using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaEcommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class WishlistChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShareToken",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotifyRequested",
                table: "WishlistItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceAtAdd",
                table: "WishlistItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShareToken",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "NotifyRequested",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "PriceAtAdd",
                table: "WishlistItems");
        }
    }
}
