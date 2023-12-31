using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Data.Migrations
{
    public partial class UpdateUserAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserCityName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDistrictName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserWardName",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserCityName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserDistrictName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserWardName",
                table: "User");
        }
    }
}
