using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class DishRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DishRates_DishId",
                table: "DishRates");

            migrationBuilder.AddColumn<decimal>(
                name: "CachedRateCount",
                table: "Dishes",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "CachedRateScore",
                table: "Dishes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishRates",
                table: "DishRates",
                columns: new[] { "DishId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DishRates",
                table: "DishRates");

            migrationBuilder.DropColumn(
                name: "CachedRateCount",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CachedRateScore",
                table: "Dishes");

            migrationBuilder.CreateIndex(
                name: "IX_DishRates_DishId",
                table: "DishRates",
                column: "DishId");
        }
    }
}
