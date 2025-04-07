using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagment.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class makerelationbewteensponserandeventsetnulwhendelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Sponsers_SponserId",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Sponsers_SponserId",
                table: "Events",
                column: "SponserId",
                principalTable: "Sponsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Sponsers_SponserId",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Sponsers_SponserId",
                table: "Events",
                column: "SponserId",
                principalTable: "Sponsers",
                principalColumn: "Id");
        }
    }
}
