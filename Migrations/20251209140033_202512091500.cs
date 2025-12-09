using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202512091500 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrders_Customer_CustomerId",
                table: "ShopOrders");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "ShopOrderStatus",
                table: "ShopOrders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ShopOrders",
                newName: "CustomerInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopOrders_CustomerId",
                table: "ShopOrders",
                newName: "IX_ShopOrders_CustomerInfoId");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId",
                table: "ShopOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ShopOrders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Uploadable",
                table: "CartItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrders_CartId",
                table: "ShopOrders",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrders_Carts_CartId",
                table: "ShopOrders",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrders_CustomerInfos_CustomerInfoId",
                table: "ShopOrders",
                column: "CustomerInfoId",
                principalTable: "CustomerInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrders_Carts_CartId",
                table: "ShopOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrders_CustomerInfos_CustomerInfoId",
                table: "ShopOrders");

            migrationBuilder.DropIndex(
                name: "IX_ShopOrders_CartId",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "Uploadable",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ShopOrders",
                newName: "ShopOrderStatus");

            migrationBuilder.RenameColumn(
                name: "CustomerInfoId",
                table: "ShopOrders",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopOrders_CustomerInfoId",
                table: "ShopOrders",
                newName: "IX_ShopOrders_CustomerId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Carts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CartId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerInfoId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerInfos_CustomerInfoId",
                        column: x => x.CustomerInfoId,
                        principalTable: "CustomerInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CartId",
                table: "Customer",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerInfoId",
                table: "Customer",
                column: "CustomerInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrders_Customer_CustomerId",
                table: "ShopOrders",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
