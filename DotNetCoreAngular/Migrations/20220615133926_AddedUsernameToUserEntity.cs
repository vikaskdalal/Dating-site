using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class AddedUsernameToUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                schema: "User",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                schema: "User",
                table: "Users");
        }
    }
}
