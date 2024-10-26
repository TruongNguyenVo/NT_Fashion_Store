using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doan1_v1.Migrations
{
    /// <inheritdoc />
    public partial class migrate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "PurchaseReports");

            migrationBuilder.AddColumn<double>(
                name: "OtherCost",
                table: "PurchaseReports",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherCost",
                table: "PurchaseReports");

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "PurchaseReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
