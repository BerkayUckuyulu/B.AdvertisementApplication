using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B.AdvertisementApp.DataAccess.Migrations
{
    public partial class DataAddedToRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "Defination" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "Defination" },
                values: new object[] { 2, "Member" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
