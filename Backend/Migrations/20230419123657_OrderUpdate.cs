using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class OrderUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishInOrder_Orders_OrderId",
                table: "DishInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatusLog_Orders_OrderId",
                table: "OrderStatusLog");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatusLog_OrderId",
                table: "OrderStatusLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderId_DishId",
                table: "DishInOrder");

            migrationBuilder.DropIndex(
                name: "IX_DishInOrder_OrderId",
                table: "DishInOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserId_DishId",
                table: "DishesInCart");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderStatusLog");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DishInOrder");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Dishes");

            migrationBuilder.AddColumn<long>(
                name: "OrderNumber",
                table: "OrderStatusLog",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Number",
                table: "Orders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryTime",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "OrderNumber",
                table: "DishInOrder",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "Dishes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Number");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishInOrder",
                table: "DishInOrder",
                columns: new[] { "DishId", "OrderNumber" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishesInCart",
                table: "DishesInCart",
                columns: new[] { "DishId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusLog_OrderNumber",
                table: "OrderStatusLog",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_DishInOrder_OrderNumber",
                table: "DishInOrder",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_RestaurantId",
                table: "Dishes",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Restaurants_RestaurantId",
                table: "Dishes",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishInOrder_Orders_OrderNumber",
                table: "DishInOrder",
                column: "OrderNumber",
                principalTable: "Orders",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatusLog_Orders_OrderNumber",
                table: "OrderStatusLog",
                column: "OrderNumber",
                principalTable: "Orders",
                principalColumn: "Number",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Restaurants_RestaurantId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_DishInOrder_Orders_OrderNumber",
                table: "DishInOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderStatusLog_Orders_OrderNumber",
                table: "OrderStatusLog");

            migrationBuilder.DropIndex(
                name: "IX_OrderStatusLog_OrderNumber",
                table: "OrderStatusLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishInOrder",
                table: "DishInOrder");

            migrationBuilder.DropIndex(
                name: "IX_DishInOrder_OrderNumber",
                table: "DishInOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishesInCart",
                table: "DishesInCart");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_RestaurantId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "OrderStatusLog");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "DishInOrder");

            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "Dishes");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "OrderStatusLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "DishInOrder",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Dishes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderId_DishId",
                table: "DishInOrder",
                columns: new[] { "DishId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserId_DishId",
                table: "DishesInCart",
                columns: new[] { "DishId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatusLog_OrderId",
                table: "OrderStatusLog",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DishInOrder_OrderId",
                table: "DishInOrder",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishInOrder_Orders_OrderId",
                table: "DishInOrder",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStatusLog_Orders_OrderId",
                table: "OrderStatusLog",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
