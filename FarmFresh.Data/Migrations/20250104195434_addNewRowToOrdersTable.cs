using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmFresh.Data.Migrations
{
    /// <inheritdoc />
    public partial class addNewRowToOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipmentNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ShipmentNumber",
                table: "Orders");
        }
    }
}
