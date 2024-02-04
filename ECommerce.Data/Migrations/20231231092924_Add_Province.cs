using Microsoft.EntityFrameworkCore.Migrations;

namespace ECommerce.Data.Migrations
{
    public partial class Add_Province : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    FullNameEn = table.Column<string>(nullable: true),
                    CodeName = table.Column<string>(nullable: true),
                    ProvinceCode = table.Column<string>(nullable: true),
                    AdministrativeUnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    FullNameEn = table.Column<string>(nullable: true),
                    CodeName = table.Column<string>(nullable: true),
                    AdministrativeUnitId = table.Column<int>(nullable: true),
                    AdministrativeRegionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NameEn = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    FullNameEn = table.Column<string>(nullable: true),
                    CodeName = table.Column<string>(nullable: true),
                    DistrictCode = table.Column<string>(nullable: true),
                    AdministrativeUnitId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Code);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "Wards");
        }
    }
}
