using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProniaWebSeyid.Migrations
{
    /// <inheritdoc />
    public partial class Config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Shippings",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tag_Name",
                table: "Tags",
                sql: "LEN([Name])>2");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Shipping_Name",
                table: "Shippings",
                sql: "LEN([Name])>3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Name",
                table: "Products",
                sql: "LEN([Name])>3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_Price",
                table: "Products",
                sql: "Price>=0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Product_ReytingCount",
                table: "Products",
                sql: "ReytingCount >= 0 AND ReytingCount <= 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Tag_Name",
                table: "Tags");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Shipping_Name",
                table: "Shippings");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Name",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_Price",
                table: "Products");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Product_ReytingCount",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Shippings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
