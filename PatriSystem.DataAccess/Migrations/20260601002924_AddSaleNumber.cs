using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PatriSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SaleNumber",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleNumber",
                table: "Sales");
        }
    }
}
