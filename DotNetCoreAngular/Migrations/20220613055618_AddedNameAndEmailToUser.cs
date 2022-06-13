using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class AddedNameAndEmailToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                schema: "User",
                table: "Users",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "User",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "User",
                table: "Users",
                newName: "UserName");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "User",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
