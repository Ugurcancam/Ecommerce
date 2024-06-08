using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AltCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AltCategoryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_AltCategoryId",
                table: "Products",
                column: "AltCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AltCategories_AltCategoryId",
                table: "Products",
                column: "AltCategoryId",
                principalTable: "AltCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_AltCategories_AltCategoryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_AltCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AltCategoryId",
                table: "Products");
        }
    }
}
