using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202512081354 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemProperties_Carts_CartId",
                table: "ShopItemProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemProperties_ShopImages_ImageId",
                table: "ShopItemProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemProperties_ShopItems_ShopItemId",
                table: "ShopItemProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemPropertyOptions_ShopItemProperties_ShopItemPropertyId",
                table: "ShopItemPropertyOptions");

            migrationBuilder.DropTable(
                name: "CartItemPropertyOption");

            migrationBuilder.DropTable(
                name: "CartItemProperty");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopItemPropertyOptions",
                table: "ShopItemPropertyOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopItemProperties",
                table: "ShopItemProperties");

            migrationBuilder.RenameTable(
                name: "ShopItemPropertyOptions",
                newName: "ShopItemPropertyOption");

            migrationBuilder.RenameTable(
                name: "ShopItemProperties",
                newName: "ShopItemProperty");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemPropertyOptions_ShopItemPropertyId",
                table: "ShopItemPropertyOption",
                newName: "IX_ShopItemPropertyOption_ShopItemPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemProperties_ShopItemId",
                table: "ShopItemProperty",
                newName: "IX_ShopItemProperty_ShopItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemProperties_ImageId",
                table: "ShopItemProperty",
                newName: "IX_ShopItemProperty_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemProperties_CartId",
                table: "ShopItemProperty",
                newName: "IX_ShopItemProperty_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopItemPropertyOption",
                table: "ShopItemPropertyOption",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopItemProperty",
                table: "ShopItemProperty",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemProperty_Carts_CartId",
                table: "ShopItemProperty",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemProperty_ShopImages_ImageId",
                table: "ShopItemProperty",
                column: "ImageId",
                principalTable: "ShopImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemProperty_ShopItems_ShopItemId",
                table: "ShopItemProperty",
                column: "ShopItemId",
                principalTable: "ShopItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemPropertyOption_ShopItemProperty_ShopItemPropertyId",
                table: "ShopItemPropertyOption",
                column: "ShopItemPropertyId",
                principalTable: "ShopItemProperty",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemProperty_Carts_CartId",
                table: "ShopItemProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemProperty_ShopImages_ImageId",
                table: "ShopItemProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemProperty_ShopItems_ShopItemId",
                table: "ShopItemProperty");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopItemPropertyOption_ShopItemProperty_ShopItemPropertyId",
                table: "ShopItemPropertyOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopItemPropertyOption",
                table: "ShopItemPropertyOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopItemProperty",
                table: "ShopItemProperty");

            migrationBuilder.RenameTable(
                name: "ShopItemPropertyOption",
                newName: "ShopItemPropertyOptions");

            migrationBuilder.RenameTable(
                name: "ShopItemProperty",
                newName: "ShopItemProperties");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemPropertyOption_ShopItemPropertyId",
                table: "ShopItemPropertyOptions",
                newName: "IX_ShopItemPropertyOptions_ShopItemPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemProperty_ShopItemId",
                table: "ShopItemProperties",
                newName: "IX_ShopItemProperties_ShopItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemProperty_ImageId",
                table: "ShopItemProperties",
                newName: "IX_ShopItemProperties_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItemProperty_CartId",
                table: "ShopItemProperties",
                newName: "IX_ShopItemProperties_CartId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopItemPropertyOptions",
                table: "ShopItemPropertyOptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopItemProperties",
                table: "ShopItemProperties",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CartItemProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CartItemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Uploadable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItemProperty_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CartItemPropertyOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CartItemPropertyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItemPropertyOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItemPropertyOption_CartItemProperty_CartItemPropertyId",
                        column: x => x.CartItemPropertyId,
                        principalTable: "CartItemProperty",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItemProperty_CartItemId",
                table: "CartItemProperty",
                column: "CartItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItemPropertyOption_CartItemPropertyId",
                table: "CartItemPropertyOption",
                column: "CartItemPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemProperties_Carts_CartId",
                table: "ShopItemProperties",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemProperties_ShopImages_ImageId",
                table: "ShopItemProperties",
                column: "ImageId",
                principalTable: "ShopImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemProperties_ShopItems_ShopItemId",
                table: "ShopItemProperties",
                column: "ShopItemId",
                principalTable: "ShopItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItemPropertyOptions_ShopItemProperties_ShopItemPropertyId",
                table: "ShopItemPropertyOptions",
                column: "ShopItemPropertyId",
                principalTable: "ShopItemProperties",
                principalColumn: "Id");
        }
    }
}
