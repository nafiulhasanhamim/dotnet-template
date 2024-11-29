using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_mvc.Migrations
{
    /// <inheritdoc />
    public partial class InitiateCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5001cbbf-acc8-4cc2-baf3-77a18cf43385", "2", "User", "USER" },
                    { "90fbaca6-e580-4fa5-b3c6-81d6e6c4f619", "3", "HR", "HR" },
                    { "d5554d70-d96b-42de-9f12-2f10375e10e1", "1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3c7cc8aa-86a1-48d4-98d4-afe7b3335db1", "2", "User", "USER" },
                    { "96f0a652-23c9-4818-9dea-1a1963fd7585", "1", "Admin", "ADMIN" },
                    { "ec4e9715-81c8-42fa-b4b1-393e36f00b5e", "3", "HR", "HR" }
                });
        }
    }
}
