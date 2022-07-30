using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class RemovedUsernameFromMessageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientUsername",
                schema: "Message",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderUsername",
                schema: "Message",
                table: "Messages");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { new DateTime(2022, 7, 29, 19, 49, 33, 232, DateTimeKind.Local).AddTicks(7643), new DateTime(2022, 7, 29, 19, 49, 33, 232, DateTimeKind.Local).AddTicks(7692), new DateTime(2022, 7, 29, 19, 49, 33, 232, DateTimeKind.Local).AddTicks(7636), new byte[] { 135, 32, 213, 245, 91, 176, 240, 61, 193, 218, 229, 84, 43, 94, 107, 149, 20, 59, 165, 3, 195, 197, 173, 21, 42, 192, 113, 103, 229, 171, 250, 15, 206, 163, 40, 174, 98, 64, 205, 175, 195, 231, 234, 105, 61, 115, 87, 74, 228, 73, 26, 136, 90, 159, 200, 32, 98, 46, 216, 155, 5, 247, 207, 133 }, new byte[] { 47, 212, 253, 241, 175, 228, 2, 210, 216, 12, 251, 224, 51, 225, 4, 205, 153, 65, 163, 148, 156, 125, 231, 78, 173, 182, 247, 154, 31, 8, 26, 92, 103, 90, 148, 99, 45, 30, 158, 218, 19, 37, 247, 246, 200, 179, 156, 147, 69, 19, 118, 10, 57, 199, 60, 126, 181, 39, 18, 66, 114, 69, 82, 179, 134, 175, 164, 156, 215, 244, 101, 4, 206, 85, 139, 135, 152, 28, 149, 0, 252, 237, 118, 8, 89, 99, 44, 38, 100, 63, 200, 38, 19, 200, 170, 129, 176, 72, 229, 13, 253, 180, 112, 228, 243, 103, 125, 253, 105, 242, 28, 29, 201, 29, 205, 198, 242, 203, 23, 70, 200, 249, 120, 76, 63, 89, 119, 197 }, "mdhmpjucooq5clmqiqsfg" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientUsername",
                schema: "Message",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderUsername",
                schema: "Message",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { new DateTime(2022, 7, 28, 20, 36, 3, 171, DateTimeKind.Local).AddTicks(1651), new DateTime(2022, 7, 28, 20, 36, 3, 171, DateTimeKind.Local).AddTicks(1698), new DateTime(2022, 7, 28, 20, 36, 3, 171, DateTimeKind.Local).AddTicks(1644), new byte[] { 121, 170, 98, 42, 9, 228, 35, 53, 148, 75, 114, 147, 177, 10, 106, 36, 210, 184, 184, 145, 115, 164, 196, 181, 224, 238, 178, 147, 153, 236, 28, 31, 193, 30, 247, 138, 124, 213, 43, 182, 121, 187, 2, 244, 144, 252, 167, 76, 104, 214, 58, 164, 226, 95, 216, 186, 171, 178, 253, 122, 52, 39, 219, 53 }, new byte[] { 232, 133, 41, 131, 99, 122, 138, 228, 155, 233, 224, 45, 163, 11, 43, 153, 192, 62, 6, 59, 213, 222, 84, 240, 83, 245, 160, 109, 238, 161, 229, 134, 182, 35, 165, 145, 216, 189, 133, 209, 77, 208, 106, 249, 69, 161, 116, 193, 124, 232, 125, 147, 21, 199, 229, 114, 145, 204, 21, 56, 254, 221, 197, 196, 181, 223, 28, 202, 98, 4, 111, 192, 188, 8, 247, 146, 191, 172, 124, 66, 126, 65, 98, 114, 180, 151, 170, 80, 68, 9, 116, 166, 219, 45, 88, 101, 212, 135, 255, 3, 0, 31, 247, 25, 159, 72, 39, 188, 1, 216, 120, 82, 58, 141, 18, 118, 250, 160, 245, 194, 174, 39, 54, 150, 93, 188, 32, 216 }, "m+dhmpjucooq5clmqiqsfg==" });
        }
    }
}
