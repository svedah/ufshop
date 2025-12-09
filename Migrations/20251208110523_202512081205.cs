using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ufshop.Migrations
{
    /// <inheritdoc />
    public partial class _202512081205 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "CartItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "CartItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "CartItems");
        }
    }
}
