using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_mvc.Migrations
{
    /// <inheritdoc />
    public partial class InitiateCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProduct_Orders_OrderId",
                table: "OrderedProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProduct_Products_ProductId",
                table: "OrderedProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedProduct",
                table: "OrderedProduct");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5001cbbf-acc8-4cc2-baf3-77a18cf43385");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90fbaca6-e580-4fa5-b3c6-81d6e6c4f619");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5554d70-d96b-42de-9f12-2f10375e10e1");

            migrationBuilder.RenameTable(
                name: "OrderedProduct",
                newName: "OrderedProducts");

            migrationBuilder.RenameIndex(
                name: "IX_OrderedProduct_ProductId",
                table: "OrderedProducts",
                newName: "IX_OrderedProducts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderedProduct_OrderId",
                table: "OrderedProducts",
                newName: "IX_OrderedProducts_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedProducts",
                table: "OrderedProducts",
                column: "OrderedProductId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7b9b2761-94cb-40b7-be71-9eb5867198d2", "2", "User", "USER" },
                    { "8d61e156-86da-4c86-ae36-1b26c603c494", "3", "HR", "HR" },
                    { "d93b8ea4-af2a-4c63-b4ff-482a46c8777d", "1", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProducts_Orders_OrderId",
                table: "OrderedProducts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProducts_Products_ProductId",
                table: "OrderedProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProducts_Orders_OrderId",
                table: "OrderedProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProducts_Products_ProductId",
                table: "OrderedProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderedProducts",
                table: "OrderedProducts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7b9b2761-94cb-40b7-be71-9eb5867198d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8d61e156-86da-4c86-ae36-1b26c603c494");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d93b8ea4-af2a-4c63-b4ff-482a46c8777d");

            migrationBuilder.RenameTable(
                name: "OrderedProducts",
                newName: "OrderedProduct");

            migrationBuilder.RenameIndex(
                name: "IX_OrderedProducts_ProductId",
                table: "OrderedProduct",
                newName: "IX_OrderedProduct_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderedProducts_OrderId",
                table: "OrderedProduct",
                newName: "IX_OrderedProduct_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderedProduct",
                table: "OrderedProduct",
                column: "OrderedProductId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5001cbbf-acc8-4cc2-baf3-77a18cf43385", "2", "User", "USER" },
                    { "90fbaca6-e580-4fa5-b3c6-81d6e6c4f619", "3", "HR", "HR" },
                    { "d5554d70-d96b-42de-9f12-2f10375e10e1", "1", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProduct_Orders_OrderId",
                table: "OrderedProduct",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProduct_Products_ProductId",
                table: "OrderedProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
