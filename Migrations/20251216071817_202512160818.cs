using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202512160818 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "ShopOrders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ShopOrderPayment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ConfirmedAmount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopOrderPayment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopOrders_PaymentId",
                table: "ShopOrders",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopOrders_ShopOrderPayment_PaymentId",
                table: "ShopOrders",
                column: "PaymentId",
                principalTable: "ShopOrderPayment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopOrders_ShopOrderPayment_PaymentId",
                table: "ShopOrders");

            migrationBuilder.DropTable(
                name: "ShopOrderPayment");

            migrationBuilder.DropIndex(
                name: "IX_ShopOrders_PaymentId",
                table: "ShopOrders");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "ShopOrders");
        }
    }
}
