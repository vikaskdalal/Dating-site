using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class AddedFlagPropInConnectionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Flag",
                schema: "SignalR",
                table: "Connections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 8, 8, 11, 23, 50, 365, DateTimeKind.Local).AddTicks(488), new DateTime(2022, 8, 8, 11, 23, 50, 365, DateTimeKind.Local).AddTicks(542), new DateTime(2022, 8, 8, 11, 23, 50, 365, DateTimeKind.Local).AddTicks(481), new byte[] { 63, 198, 172, 68, 52, 28, 110, 166, 242, 74, 27, 90, 197, 25, 38, 228, 189, 96, 91, 133, 181, 78, 205, 109, 191, 154, 132, 3, 105, 12, 50, 120, 58, 126, 200, 208, 155, 91, 86, 55, 193, 126, 207, 55, 230, 65, 38, 2, 99, 47, 226, 175, 90, 168, 190, 231, 101, 17, 59, 186, 84, 169, 5, 221 }, new byte[] { 181, 156, 91, 95, 109, 88, 107, 124, 23, 27, 133, 73, 33, 54, 226, 62, 9, 200, 30, 200, 143, 128, 145, 157, 198, 72, 248, 20, 146, 59, 156, 242, 112, 168, 228, 118, 126, 77, 63, 182, 70, 136, 221, 253, 11, 147, 60, 236, 239, 215, 189, 99, 52, 186, 194, 2, 144, 169, 15, 151, 116, 5, 82, 57, 111, 185, 241, 75, 50, 159, 114, 118, 166, 170, 227, 26, 6, 146, 173, 178, 67, 173, 129, 200, 91, 47, 4, 20, 9, 52, 38, 177, 184, 172, 144, 101, 85, 16, 5, 193, 177, 100, 99, 182, 102, 180, 239, 40, 140, 83, 86, 26, 190, 83, 20, 94, 99, 66, 161, 143, 52, 254, 174, 128, 8, 239, 246, 248 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flag",
                schema: "SignalR",
                table: "Connections");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 7, 29, 19, 49, 33, 232, DateTimeKind.Local).AddTicks(7643), new DateTime(2022, 7, 29, 19, 49, 33, 232, DateTimeKind.Local).AddTicks(7692), new DateTime(2022, 7, 29, 19, 49, 33, 232, DateTimeKind.Local).AddTicks(7636), new byte[] { 135, 32, 213, 245, 91, 176, 240, 61, 193, 218, 229, 84, 43, 94, 107, 149, 20, 59, 165, 3, 195, 197, 173, 21, 42, 192, 113, 103, 229, 171, 250, 15, 206, 163, 40, 174, 98, 64, 205, 175, 195, 231, 234, 105, 61, 115, 87, 74, 228, 73, 26, 136, 90, 159, 200, 32, 98, 46, 216, 155, 5, 247, 207, 133 }, new byte[] { 47, 212, 253, 241, 175, 228, 2, 210, 216, 12, 251, 224, 51, 225, 4, 205, 153, 65, 163, 148, 156, 125, 231, 78, 173, 182, 247, 154, 31, 8, 26, 92, 103, 90, 148, 99, 45, 30, 158, 218, 19, 37, 247, 246, 200, 179, 156, 147, 69, 19, 118, 10, 57, 199, 60, 126, 181, 39, 18, 66, 114, 69, 82, 179, 134, 175, 164, 156, 215, 244, 101, 4, 206, 85, 139, 135, 152, 28, 149, 0, 252, 237, 118, 8, 89, 99, 44, 38, 100, 63, 200, 38, 19, 200, 170, 129, 176, 72, 229, 13, 253, 180, 112, 228, 243, 103, 125, 253, 105, 242, 28, 29, 201, 29, 205, 198, 242, 203, 23, 70, 200, 249, 120, 76, 63, 89, 119, 197 } });
        }
    }
}
