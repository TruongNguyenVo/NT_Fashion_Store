using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace doan1_v1.Migrations
{
    /// <inheritdoc />
    public partial class migrate8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Customer",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReports_SupplierId",
                table: "PurchaseReports",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReports_UserId",
                table: "PurchaseReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReportProductDetails_ProductId",
                table: "PurchaseReportProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseReportProductDetails_PurchaseReportId",
                table: "PurchaseReportProductDetails",
                column: "PurchaseReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductDetails_OrderId",
                table: "OrderProductDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProductDetails_ProductId",
                table: "OrderProductDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_CartId",
                table: "CartDetails",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_ProductId",
                table: "CartDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Carts_CartId",
                table: "CartDetails",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Products_ProductId",
                table: "CartDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReportProductDetails_Products_ProductId",
                table: "PurchaseReportProductDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReportProductDetails_PurchaseReports_PurchaseReportId",
                table: "PurchaseReportProductDetails",
                column: "PurchaseReportId",
                principalTable: "PurchaseReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReports_Suppliers_SupplierId",
                table: "PurchaseReports",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseReports_Users_UserId",
                table: "PurchaseReports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Carts_CartId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Products_ProductId",
                table: "CartDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Users_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductDetails_Orders_OrderId",
                table: "OrderProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProductDetails_Products_ProductId",
                table: "OrderProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Products_ProductId",
                table: "ProductImages");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReportProductDetails_Products_ProductId",
                table: "PurchaseReportProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReportProductDetails_PurchaseReports_PurchaseReportId",
                table: "PurchaseReportProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReports_Suppliers_SupplierId",
                table: "PurchaseReports");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseReports_Users_UserId",
                table: "PurchaseReports");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReports_SupplierId",
                table: "PurchaseReports");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReports_UserId",
                table: "PurchaseReports");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReportProductDetails_ProductId",
                table: "PurchaseReportProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseReportProductDetails_PurchaseReportId",
                table: "PurchaseReportProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderProductDetails_OrderId",
                table: "OrderProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderProductDetails_ProductId",
                table: "OrderProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_CartId",
                table: "CartDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_ProductId",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Customer",
                table: "Orders");
        }
    }
}
