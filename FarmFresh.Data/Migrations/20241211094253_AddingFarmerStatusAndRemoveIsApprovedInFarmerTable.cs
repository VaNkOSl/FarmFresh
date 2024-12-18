using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmFresh.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingFarmerStatusAndRemoveIsApprovedInFarmerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Farmers");

            migrationBuilder.AddColumn<int>(
                name: "FarmerStatus",
                table: "Farmers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmerStatus",
                table: "Farmers");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Farmers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
