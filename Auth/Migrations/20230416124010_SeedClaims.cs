using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Auth.Migrations
{
    /// <inheritdoc />
    public partial class SeedClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 8, "Grant", "RestaurantAdmin", new Guid("3a582199-77b1-4352-a61a-fce564ebb8d4") },
                    { 9, "Grant", "Cook", new Guid("3a582199-77b1-4352-a61a-fce564ebb8d4") },
                    { 10, "Grant", "Manager", new Guid("3a582199-77b1-4352-a61a-fce564ebb8d4") },
                    { 11, "Grant", "Courier", new Guid("3a582199-77b1-4352-a61a-fce564ebb8d4") },
                    { 12, "Grant", "RestaurantOwner", new Guid("3a582199-77b1-4352-a61a-fce564ebb8d4") },
                    { 13, "PersonalData", "Read", new Guid("3a582199-77b1-4352-a61a-fce564ebb8d4") },
                    { 14, "PersonalData", "Change", new Guid("3a582199-77b1-4352-a61a-fce564ebb8d4") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 3, "Grant", "Courier", new Guid("761a9b67-f1e1-49b0-9a84-38e40be52d19") },
                    { 7, "Grant", "Courier", new Guid("33d4a50c-3a9d-4c24-a4a7-4f2dbb64ad82") }
                });
        }
    }
}
