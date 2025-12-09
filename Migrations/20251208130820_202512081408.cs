using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202512081408 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopItemPropertyOption");

            migrationBuilder.DropTable(
                name: "ShopItemProperty");

            migrationBuilder.AddColumn<bool>(
                name: "Uploadable",
                table: "ShopItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CartFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Filename = table.Column<string>(type: "TEXT", nullable: false),
                    CartItemId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartFile_CartItems_CartItemId",
                        column: x => x.CartItemId,
                        principalTable: "CartItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartFile_CartItemId",
                table: "CartFile",
                column: "CartItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartFile");

            migrationBuilder.DropColumn(
                name: "Uploadable",
                table: "ShopItems");

            migrationBuilder.CreateTable(
                name: "ShopItemProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ImageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CartId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShopItemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Uploadable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItemProperty_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ShopItemProperty_ShopImages_ImageId",
                        column: x => x.ImageId,
                        principalTable: "ShopImages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItemProperty_ShopItems_ShopItemId",
                        column: x => x.ShopItemId,
                        principalTable: "ShopItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShopItemPropertyOption",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ShopItemPropertyId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemPropertyOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopItemPropertyOption_ShopItemProperty_ShopItemPropertyId",
                        column: x => x.ShopItemPropertyId,
                        principalTable: "ShopItemProperty",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProperty_CartId",
                table: "ShopItemProperty",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProperty_ImageId",
                table: "ShopItemProperty",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemProperty_ShopItemId",
                table: "ShopItemProperty",
                column: "ShopItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemPropertyOption_ShopItemPropertyId",
                table: "ShopItemPropertyOption",
                column: "ShopItemPropertyId");
        }
    }
}
