using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagment.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_AttenddeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AttenddeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AttenddeId",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttenddeId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AttenddeId",
                table: "Events",
                column: "AttenddeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_AttenddeId",
                table: "Events",
                column: "AttenddeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
