using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202512061541 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartItemProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Uploadable = table.Column<bool>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    CartItemId = table.Column<Guid>(type: "TEXT", nullable: true)
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
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    CartItemPropertyId = table.Column<Guid>(type: "TEXT", nullable: true)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItemPropertyOption");

            migrationBuilder.DropTable(
                name: "CartItemProperty");
        }
    }
}
