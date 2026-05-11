using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStore_Project.Migrations
{
    public partial class SellerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_sellers",
                table: "sellers");

            migrationBuilder.RenameTable(
                name: "sellers",
                newName: "Sellers");

            migrationBuilder.RenameColumn(
                name: "MobileNo",
                table: "Sellers",
                newName: "ShopName");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Sellers",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Sellers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sellers",
                table: "Sellers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sellers",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Sellers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Sellers");

            migrationBuilder.RenameTable(
                name: "Sellers",
                newName: "sellers");

            migrationBuilder.RenameColumn(
                name: "ShopName",
                table: "sellers",
                newName: "MobileNo");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "sellers",
                newName: "CompanyName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sellers",
                table: "sellers",
                column: "Id");
        }
    }
}
