using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class RemovedFlagPropInConnectionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new DateTime(2022, 8, 8, 18, 11, 6, 833, DateTimeKind.Local).AddTicks(1627), new DateTime(2022, 8, 8, 18, 11, 6, 833, DateTimeKind.Local).AddTicks(1674), new DateTime(2022, 8, 8, 18, 11, 6, 833, DateTimeKind.Local).AddTicks(1620), new byte[] { 193, 53, 92, 94, 201, 151, 157, 111, 132, 115, 37, 107, 59, 51, 95, 54, 146, 152, 201, 103, 184, 222, 93, 82, 67, 75, 5, 101, 9, 26, 192, 4, 168, 241, 90, 101, 91, 115, 236, 234, 37, 110, 153, 153, 192, 163, 89, 72, 1, 135, 241, 37, 197, 101, 173, 168, 147, 199, 157, 148, 31, 139, 31, 174 }, new byte[] { 19, 5, 150, 112, 126, 214, 195, 215, 207, 45, 207, 182, 158, 17, 113, 139, 179, 10, 74, 134, 146, 227, 84, 207, 187, 212, 254, 138, 166, 135, 169, 199, 101, 74, 236, 53, 49, 247, 106, 220, 34, 45, 7, 48, 180, 147, 116, 250, 243, 62, 70, 167, 240, 155, 18, 210, 17, 122, 161, 135, 59, 5, 221, 32, 77, 131, 178, 112, 128, 170, 128, 25, 176, 140, 141, 128, 153, 2, 143, 210, 51, 93, 237, 172, 99, 41, 171, 43, 80, 183, 73, 152, 185, 137, 89, 73, 192, 160, 134, 11, 83, 133, 52, 43, 186, 147, 51, 130, 15, 252, 14, 189, 15, 210, 247, 6, 36, 173, 127, 8, 66, 157, 239, 8, 223, 45, 29, 90 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { new DateTime(2022, 8, 8, 18, 9, 22, 632, DateTimeKind.Local).AddTicks(503), new DateTime(2022, 8, 8, 18, 9, 22, 632, DateTimeKind.Local).AddTicks(660), new DateTime(2022, 8, 8, 18, 9, 22, 632, DateTimeKind.Local).AddTicks(485), new byte[] { 152, 202, 241, 214, 207, 192, 190, 10, 186, 198, 246, 201, 105, 207, 42, 136, 132, 205, 147, 10, 237, 157, 49, 222, 247, 169, 77, 187, 100, 139, 209, 34, 232, 59, 64, 102, 64, 20, 68, 101, 131, 191, 190, 37, 133, 2, 81, 31, 169, 189, 104, 189, 10, 208, 161, 169, 31, 14, 114, 61, 184, 202, 102, 202 }, new byte[] { 244, 140, 148, 97, 199, 172, 189, 186, 184, 38, 121, 53, 139, 94, 70, 169, 234, 225, 185, 129, 220, 149, 75, 171, 6, 146, 149, 175, 22, 36, 0, 133, 88, 167, 246, 120, 88, 133, 2, 202, 21, 56, 68, 30, 165, 50, 135, 39, 53, 255, 248, 89, 109, 45, 134, 251, 187, 74, 30, 87, 165, 20, 124, 45, 133, 152, 127, 120, 162, 211, 16, 124, 176, 85, 234, 119, 223, 119, 255, 166, 235, 190, 74, 250, 237, 53, 43, 204, 160, 233, 68, 103, 241, 175, 62, 44, 172, 8, 134, 50, 169, 242, 95, 13, 131, 214, 253, 79, 187, 2, 50, 160, 6, 76, 137, 92, 167, 151, 20, 169, 49, 37, 179, 237, 233, 151, 231, 206 } });
        }
    }
}
