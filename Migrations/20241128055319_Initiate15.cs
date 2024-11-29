using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_mvc.Migrations
{
    /// <inheritdoc />
    public partial class Initiate15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1d7ff57c-9e72-44c3-924e-c558e552242a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83bc2d65-bb91-4baa-8804-cd293e6bc720");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b99edcaa-d854-45e8-94f1-caf38f2e0a33");

            migrationBuilder.RenameColumn(
                name: "UId",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3c7cc8aa-86a1-48d4-98d4-afe7b3335db1", "2", "User", "USER" },
                    { "96f0a652-23c9-4818-9dea-1a1963fd7585", "1", "Admin", "ADMIN" },
                    { "ec4e9715-81c8-42fa-b4b1-393e36f00b5e", "3", "HR", "HR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3c7cc8aa-86a1-48d4-98d4-afe7b3335db1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "96f0a652-23c9-4818-9dea-1a1963fd7585");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec4e9715-81c8-42fa-b4b1-393e36f00b5e");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "UId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1d7ff57c-9e72-44c3-924e-c558e552242a", "3", "HR", "HR" },
                    { "83bc2d65-bb91-4baa-8804-cd293e6bc720", "1", "Admin", "ADMIN" },
                    { "b99edcaa-d854-45e8-94f1-caf38f2e0a33", "2", "User", "USER" }
                });
        }
    }
}
