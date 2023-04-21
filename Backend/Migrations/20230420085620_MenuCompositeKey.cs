using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class MenuCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishMenu_Menus_IntoMenusId",
                table: "DishMenu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menus",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishMenu",
                table: "DishMenu");

            migrationBuilder.DropIndex(
                name: "IX_DishMenu_IntoMenusId",
                table: "DishMenu");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Menus");

            migrationBuilder.RenameColumn(
                name: "IntoMenusId",
                table: "DishMenu",
                newName: "IntoMenusRestaurantId");

            migrationBuilder.AddColumn<string>(
                name: "IntoMenusName",
                table: "DishMenu",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menus",
                table: "Menus",
                columns: new[] { "Name", "RestaurantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishMenu",
                table: "DishMenu",
                columns: new[] { "DishesId", "IntoMenusName", "IntoMenusRestaurantId" });

            migrationBuilder.CreateIndex(
                name: "IX_DishMenu_IntoMenusName_IntoMenusRestaurantId",
                table: "DishMenu",
                columns: new[] { "IntoMenusName", "IntoMenusRestaurantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DishMenu_Menus_IntoMenusName_IntoMenusRestaurantId",
                table: "DishMenu",
                columns: new[] { "IntoMenusName", "IntoMenusRestaurantId" },
                principalTable: "Menus",
                principalColumns: new[] { "Name", "RestaurantId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishMenu_Menus_IntoMenusName_IntoMenusRestaurantId",
                table: "DishMenu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Menus",
                table: "Menus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishMenu",
                table: "DishMenu");

            migrationBuilder.DropIndex(
                name: "IX_DishMenu_IntoMenusName_IntoMenusRestaurantId",
                table: "DishMenu");

            migrationBuilder.DropColumn(
                name: "IntoMenusName",
                table: "DishMenu");

            migrationBuilder.RenameColumn(
                name: "IntoMenusRestaurantId",
                table: "DishMenu",
                newName: "IntoMenusId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Menus",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Menus",
                table: "Menus",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishMenu",
                table: "DishMenu",
                columns: new[] { "DishesId", "IntoMenusId" });

            migrationBuilder.CreateIndex(
                name: "IX_DishMenu_IntoMenusId",
                table: "DishMenu",
                column: "IntoMenusId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishMenu_Menus_IntoMenusId",
                table: "DishMenu",
                column: "IntoMenusId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
