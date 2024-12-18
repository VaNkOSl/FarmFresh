using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmFresh.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedPropertyToProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Products");
        }
    }
}
