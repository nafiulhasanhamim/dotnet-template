using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_mvc.Migrations
{
    /// <inheritdoc />
    public partial class InitiateCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a8713c3-9838-4f0c-8ceb-acaf93aa6ccd", "1", "Admin", "ADMIN" },
                    { "97cb1581-95f0-442c-94dc-2170a5bdd459", "3", "HR", "HR" },
                    { "dd109897-3242-4835-8409-1b570aab37a8", "2", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a8713c3-9838-4f0c-8ceb-acaf93aa6ccd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97cb1581-95f0-442c-94dc-2170a5bdd459");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd109897-3242-4835-8409-1b570aab37a8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7b9b2761-94cb-40b7-be71-9eb5867198d2", "2", "User", "USER" },
                    { "8d61e156-86da-4c86-ae36-1b26c603c494", "3", "HR", "HR" },
                    { "d93b8ea4-af2a-4c63-b4ff-482a46c8777d", "1", "Admin", "ADMIN" }
                });
        }
    }
}
