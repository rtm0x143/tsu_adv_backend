using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Auth.Migrations
{
    /// <inheritdoc />
    public partial class ManageClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 16, "Manage", "Menu", new Guid("761a9b67-f1e1-49b0-9a84-38e40be52d19") },
                    { 17, "Manage", "Kitchen", new Guid("761a9b67-f1e1-49b0-9a84-38e40be52d19") },
                    { 18, "Manage", "Delivery", new Guid("761a9b67-f1e1-49b0-9a84-38e40be52d19") },
                    { 19, "Manage", "Menu", new Guid("33d4a50c-3a9d-4c24-a4a7-4f2dbb64ad82") },
                    { 20, "Manage", "Kitchen", new Guid("33d4a50c-3a9d-4c24-a4a7-4f2dbb64ad82") },
                    { 21, "Manage", "Delivery", new Guid("33d4a50c-3a9d-4c24-a4a7-4f2dbb64ad82") },
                    { 22, "Manage", "Delivery", new Guid("89efd21c-aa39-449a-97b2-474646701433") },
                    { 23, "Manage", "Kitchen", new Guid("59eebf24-ad0f-4bcb-b514-4c72376253ec") },
                    { 24, "Manage", "Menu", new Guid("7e63d4dc-c365-45f8-9bd5-3e83d9f571bc") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);
        }
    }
}
