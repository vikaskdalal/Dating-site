using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class AddedPhotoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Photos",
                schema: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 7, 12, 18, 1, 56, 167, DateTimeKind.Local).AddTicks(1604), new DateTime(2022, 7, 12, 18, 1, 56, 167, DateTimeKind.Local).AddTicks(1667), new DateTime(2022, 7, 12, 18, 1, 56, 167, DateTimeKind.Local).AddTicks(1595), new byte[] { 130, 126, 133, 95, 48, 211, 30, 117, 127, 247, 84, 141, 179, 188, 15, 103, 164, 232, 184, 198, 141, 65, 49, 203, 175, 9, 157, 15, 154, 167, 82, 244, 173, 85, 114, 126, 56, 88, 15, 25, 70, 124, 94, 223, 114, 244, 129, 7, 31, 84, 223, 216, 173, 7, 226, 100, 71, 188, 152, 189, 192, 59, 110, 183 }, new byte[] { 40, 12, 199, 166, 85, 42, 193, 151, 54, 0, 159, 35, 58, 58, 162, 156, 3, 167, 55, 94, 121, 168, 175, 166, 220, 96, 231, 140, 84, 219, 82, 162, 229, 209, 53, 43, 208, 35, 202, 140, 75, 59, 148, 177, 63, 175, 53, 93, 110, 74, 86, 159, 199, 36, 240, 166, 0, 174, 180, 154, 145, 189, 236, 252, 15, 21, 170, 242, 7, 186, 141, 213, 125, 169, 31, 204, 87, 42, 98, 38, 230, 231, 85, 84, 169, 73, 48, 240, 223, 43, 108, 207, 250, 107, 65, 164, 212, 201, 114, 67, 24, 116, 244, 69, 12, 141, 20, 142, 27, 233, 147, 230, 125, 90, 163, 103, 94, 80, 141, 6, 241, 54, 124, 209, 4, 0, 29, 34 } });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                schema: "User",
                table: "Photos",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos",
                schema: "User");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 7, 1, 10, 16, 48, 25, DateTimeKind.Local).AddTicks(5030), new DateTime(2022, 7, 1, 10, 16, 48, 25, DateTimeKind.Local).AddTicks(5100), new DateTime(2022, 7, 1, 10, 16, 48, 25, DateTimeKind.Local).AddTicks(5020), new byte[] { 81, 98, 212, 160, 253, 161, 149, 151, 156, 143, 145, 247, 163, 155, 83, 157, 161, 84, 115, 209, 114, 218, 227, 2, 86, 104, 131, 247, 144, 65, 244, 3, 111, 133, 185, 6, 49, 1, 133, 226, 79, 220, 91, 199, 110, 149, 62, 182, 77, 152, 54, 139, 209, 121, 55, 198, 181, 227, 16, 104, 99, 25, 253, 196 }, new byte[] { 29, 190, 163, 140, 23, 99, 159, 63, 198, 25, 242, 78, 152, 151, 104, 227, 29, 11, 18, 20, 74, 137, 207, 51, 123, 85, 8, 251, 96, 150, 226, 96, 248, 231, 39, 63, 181, 52, 136, 140, 254, 38, 31, 0, 107, 83, 171, 189, 141, 213, 142, 212, 159, 123, 109, 95, 102, 247, 244, 121, 198, 212, 24, 25, 179, 59, 48, 119, 199, 253, 176, 222, 16, 207, 248, 190, 116, 81, 187, 100, 103, 195, 1, 81, 185, 100, 177, 217, 53, 4, 252, 180, 123, 72, 150, 38, 5, 183, 128, 44, 60, 73, 29, 238, 31, 185, 188, 167, 240, 101, 92, 70, 65, 165, 254, 55, 111, 86, 245, 185, 13, 218, 55, 151, 151, 164, 136, 140 } });
        }
    }
}
