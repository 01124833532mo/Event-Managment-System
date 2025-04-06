using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagment.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCountAndEventForRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventCountRating",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EventRate",
                table: "Events",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventCountRating",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventRate",
                table: "Events");
        }
    }
}
