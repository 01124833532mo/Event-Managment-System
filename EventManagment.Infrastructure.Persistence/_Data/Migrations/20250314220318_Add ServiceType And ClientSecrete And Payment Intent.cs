using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagment.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTypeAndClientSecreteAndPaymentIntent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ServicePrice",
                table: "Registrations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "ServicePrice",
                table: "Registrations");
        }
    }
}
