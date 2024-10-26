using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doan1_v1.Migrations
{
    /// <inheritdoc />
    public partial class migrate15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductDetails_Orders_OrderId",
                table: "OrderProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductDetails_Products_ProductId",
                table: "OrderProductDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderProductDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderProductDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductDetails_Orders_OrderId",
                table: "OrderProductDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductDetails_Products_ProductId",
                table: "OrderProductDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductDetails_Orders_OrderId",
                table: "OrderProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductDetails_Products_ProductId",
                table: "OrderProductDetails");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "OrderProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrderProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductDetails_Orders_OrderId",
                table: "OrderProductDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProductDetails_Products_ProductId",
                table: "OrderProductDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
