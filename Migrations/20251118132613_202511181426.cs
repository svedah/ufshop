using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202511181426 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "ShopItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "ShopItems");
        }
    }
}
