using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmFresh.Data.Migrations
{
    /// <inheritdoc />
    public partial class StreetsAndQuartersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasLocation",
                table: "Cities");

            migrationBuilder.CreateTable(
                name: "Quarters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quarters_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Streets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Streets_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quarters_CityID",
                table: "Quarters",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Streets_CityID",
                table: "Streets",
                column: "CityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quarters");

            migrationBuilder.DropTable(
                name: "Streets");

            migrationBuilder.AddColumn<bool>(
                name: "HasLocation",
                table: "Cities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
