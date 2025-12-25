using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaWebSeyid.Migrations
{
    /// <inheritdoc />
    public partial class AddedReyting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReytingCount",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReytingCount",
                table: "Products");
        }
    }
}
