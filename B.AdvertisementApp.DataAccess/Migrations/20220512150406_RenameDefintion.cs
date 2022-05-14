using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace B.AdvertisementApp.DataAccess.Migrations
{
    public partial class RenameDefintion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Defination",
                table: "AppRoles",
                newName: "Definition");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Definition",
                table: "AppRoles",
                newName: "Defination");
        }
    }
}
