using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovaEcommerce.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class paymentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "CheckoutSessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "CheckoutSessions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutSessions_PaymentMethodId",
                table: "CheckoutSessions",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutSessions_PaymentMethods_PaymentMethodId",
                table: "CheckoutSessions",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutSessions_PaymentMethods_PaymentMethodId",
                table: "CheckoutSessions");

            migrationBuilder.DropIndex(
                name: "IX_CheckoutSessions_PaymentMethodId",
                table: "CheckoutSessions");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "CheckoutSessions");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "CheckoutSessions");
        }
    }
}
