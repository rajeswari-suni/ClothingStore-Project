using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStore_Project.Migrations
{
    public partial class AgentMembershipUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPercentage",
                table: "Agents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Agents");
        }
    }
}
