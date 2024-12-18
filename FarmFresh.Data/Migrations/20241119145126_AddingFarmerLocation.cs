using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmFresh.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingFarmerLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FarmerLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FarmerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedLocationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmerLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmerLocations_Farmers_FarmerId",
                        column: x => x.FarmerId,
                        principalTable: "Farmers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmerLocations_FarmerId",
                table: "FarmerLocations",
                column: "FarmerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmerLocations");
        }
    }
}
