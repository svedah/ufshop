using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202511141002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ShopId",
                table: "ShopImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopImages_ShopId",
                table: "ShopImages",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopImages_Shops_ShopId",
                table: "ShopImages",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopImages_Shops_ShopId",
                table: "ShopImages");

            migrationBuilder.DropIndex(
                name: "IX_ShopImages_ShopId",
                table: "ShopImages");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "ShopImages");
        }
    }
}
