using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class DefaultRecordInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "User",
                table: "Users",
                columns: new[] { "Id", "City", "Country", "Created", "DateOfBirth", "Email", "Gender", "Interests", "Introduction", "LastActive", "LookingFor", "Name", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { 1, null, null, new DateTime(2022, 6, 30, 8, 27, 11, 33, DateTimeKind.Local).AddTicks(7547), new DateTime(2022, 6, 30, 8, 27, 11, 33, DateTimeKind.Local).AddTicks(7601), "vikaskdalal@gmail.com", null, null, null, new DateTime(2022, 6, 30, 8, 27, 11, 33, DateTimeKind.Local).AddTicks(7539), null, "Vikas", new byte[] { 175, 190, 41, 87, 11, 236, 251, 74, 239, 54, 70, 239, 182, 10, 252, 223, 71, 203, 78, 191, 55, 79, 203, 233, 16, 38, 156, 148, 134, 0, 57, 7, 225, 84, 194, 202, 90, 12, 139, 148, 38, 216, 237, 129, 55, 19, 37, 146, 129, 110, 207, 4, 245, 89, 42, 197, 174, 50, 99, 160, 118, 158, 5, 204 }, new byte[] { 162, 7, 113, 170, 132, 79, 224, 245, 101, 102, 207, 91, 39, 157, 232, 251, 64, 198, 59, 183, 20, 100, 206, 137, 132, 98, 31, 19, 134, 97, 159, 149, 187, 72, 203, 16, 198, 239, 194, 192, 150, 177, 80, 20, 164, 26, 30, 112, 82, 116, 102, 5, 0, 48, 12, 251, 151, 225, 255, 126, 110, 6, 5, 103, 144, 152, 198, 51, 165, 100, 113, 234, 162, 36, 162, 17, 155, 230, 102, 98, 129, 107, 243, 105, 1, 45, 129, 245, 37, 249, 99, 20, 138, 108, 102, 159, 175, 102, 229, 160, 177, 121, 225, 22, 45, 138, 54, 78, 92, 251, 214, 108, 8, 142, 75, 68, 44, 36, 132, 186, 138, 148, 9, 104, 251, 8, 119, 27 }, "m+dhmpjucooq5clmqiqsfg==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
