using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaEcommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCouponCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppliedCouponCode",
                table: "Carts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppliedCouponCode",
                table: "Carts");
        }
    }
}
