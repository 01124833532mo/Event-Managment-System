using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagment.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntityForSponserAndFeedBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SponserId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: true),
                    AttenddeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_AspNetUsers_AttenddeId",
                        column: x => x.AttenddeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedbacks_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sponsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sponsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_SponserId",
                table: "Events",
                column: "SponserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_AttenddeId",
                table: "Feedbacks",
                column: "AttenddeId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_EventId",
                table: "Feedbacks",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Sponsers_SponserId",
                table: "Events",
                column: "SponserId",
                principalTable: "Sponsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Sponsers_SponserId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Sponsers");

            migrationBuilder.DropIndex(
                name: "IX_Events_SponserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SponserId",
                table: "Events");
        }
    }
}
