using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doan1_v1.Migrations
{
    /// <inheritdoc />
    public partial class migrate9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatePredictSuccess",
                table: "Orders",
                newName: "DateReceive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateReceive",
                table: "Orders",
                newName: "DatePredictSuccess");
        }
    }
}
