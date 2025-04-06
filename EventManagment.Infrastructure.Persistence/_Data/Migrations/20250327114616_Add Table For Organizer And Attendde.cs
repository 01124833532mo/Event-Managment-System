using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventManagment.Infrastructure.Persistence._Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableForOrganizerAndAttendde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizerId",
                table: "Registrations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttenddeId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_OrganizerId",
                table: "Registrations",
                column: "OrganizerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_AspNetUsers_OrganizerId",
                table: "Registrations",
                column: "OrganizerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_AttenddeId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_AspNetUsers_OrganizerId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_OrganizerId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Events_AttenddeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "AttenddeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
