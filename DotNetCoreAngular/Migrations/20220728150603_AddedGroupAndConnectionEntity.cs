using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class AddedGroupAndConnectionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SignalR");

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "SignalR",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                schema: "SignalR",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_Connections_Groups_GroupName",
                        column: x => x.GroupName,
                        principalSchema: "SignalR",
                        principalTable: "Groups",
                        principalColumn: "Name");
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 7, 28, 20, 36, 3, 171, DateTimeKind.Local).AddTicks(1651), new DateTime(2022, 7, 28, 20, 36, 3, 171, DateTimeKind.Local).AddTicks(1698), new DateTime(2022, 7, 28, 20, 36, 3, 171, DateTimeKind.Local).AddTicks(1644), new byte[] { 121, 170, 98, 42, 9, 228, 35, 53, 148, 75, 114, 147, 177, 10, 106, 36, 210, 184, 184, 145, 115, 164, 196, 181, 224, 238, 178, 147, 153, 236, 28, 31, 193, 30, 247, 138, 124, 213, 43, 182, 121, 187, 2, 244, 144, 252, 167, 76, 104, 214, 58, 164, 226, 95, 216, 186, 171, 178, 253, 122, 52, 39, 219, 53 }, new byte[] { 232, 133, 41, 131, 99, 122, 138, 228, 155, 233, 224, 45, 163, 11, 43, 153, 192, 62, 6, 59, 213, 222, 84, 240, 83, 245, 160, 109, 238, 161, 229, 134, 182, 35, 165, 145, 216, 189, 133, 209, 77, 208, 106, 249, 69, 161, 116, 193, 124, 232, 125, 147, 21, 199, 229, 114, 145, 204, 21, 56, 254, 221, 197, 196, 181, 223, 28, 202, 98, 4, 111, 192, 188, 8, 247, 146, 191, 172, 124, 66, 126, 65, 98, 114, 180, 151, 170, 80, 68, 9, 116, 166, 219, 45, 88, 101, 212, 135, 255, 3, 0, 31, 247, 25, 159, 72, 39, 188, 1, 216, 120, 82, 58, 141, 18, 118, 250, 160, 245, 194, 174, 39, 54, 150, 93, 188, 32, 216 } });

            migrationBuilder.CreateIndex(
                name: "IX_Connections_GroupName",
                schema: "SignalR",
                table: "Connections",
                column: "GroupName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections",
                schema: "SignalR");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "SignalR");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 7, 12, 18, 1, 56, 167, DateTimeKind.Local).AddTicks(1604), new DateTime(2022, 7, 12, 18, 1, 56, 167, DateTimeKind.Local).AddTicks(1667), new DateTime(2022, 7, 12, 18, 1, 56, 167, DateTimeKind.Local).AddTicks(1595), new byte[] { 130, 126, 133, 95, 48, 211, 30, 117, 127, 247, 84, 141, 179, 188, 15, 103, 164, 232, 184, 198, 141, 65, 49, 203, 175, 9, 157, 15, 154, 167, 82, 244, 173, 85, 114, 126, 56, 88, 15, 25, 70, 124, 94, 223, 114, 244, 129, 7, 31, 84, 223, 216, 173, 7, 226, 100, 71, 188, 152, 189, 192, 59, 110, 183 }, new byte[] { 40, 12, 199, 166, 85, 42, 193, 151, 54, 0, 159, 35, 58, 58, 162, 156, 3, 167, 55, 94, 121, 168, 175, 166, 220, 96, 231, 140, 84, 219, 82, 162, 229, 209, 53, 43, 208, 35, 202, 140, 75, 59, 148, 177, 63, 175, 53, 93, 110, 74, 86, 159, 199, 36, 240, 166, 0, 174, 180, 154, 145, 189, 236, 252, 15, 21, 170, 242, 7, 186, 141, 213, 125, 169, 31, 204, 87, 42, 98, 38, 230, 231, 85, 84, 169, 73, 48, 240, 223, 43, 108, 207, 250, 107, 65, 164, 212, 201, 114, 67, 24, 116, 244, 69, 12, 141, 20, 142, 27, 233, 147, 230, 125, 90, 163, 103, 94, 80, 141, 6, 241, 54, 124, 209, 4, 0, 29, 34 } });
        }
    }
}
