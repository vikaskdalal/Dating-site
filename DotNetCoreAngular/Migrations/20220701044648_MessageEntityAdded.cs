using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotNetCoreAngular.Migrations
{
    public partial class MessageEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Message");

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    SenderUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    RecipientUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRead = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MessageSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RecipientDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_RecipientId",
                        column: x => x.RecipientId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalSchema: "User",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 7, 1, 10, 16, 48, 25, DateTimeKind.Local).AddTicks(5030), new DateTime(2022, 7, 1, 10, 16, 48, 25, DateTimeKind.Local).AddTicks(5100), new DateTime(2022, 7, 1, 10, 16, 48, 25, DateTimeKind.Local).AddTicks(5020), new byte[] { 81, 98, 212, 160, 253, 161, 149, 151, 156, 143, 145, 247, 163, 155, 83, 157, 161, 84, 115, 209, 114, 218, 227, 2, 86, 104, 131, 247, 144, 65, 244, 3, 111, 133, 185, 6, 49, 1, 133, 226, 79, 220, 91, 199, 110, 149, 62, 182, 77, 152, 54, 139, 209, 121, 55, 198, 181, 227, 16, 104, 99, 25, 253, 196 }, new byte[] { 29, 190, 163, 140, 23, 99, 159, 63, 198, 25, 242, 78, 152, 151, 104, 227, 29, 11, 18, 20, 74, 137, 207, 51, 123, 85, 8, 251, 96, 150, 226, 96, 248, 231, 39, 63, 181, 52, 136, 140, 254, 38, 31, 0, 107, 83, 171, 189, 141, 213, 142, 212, 159, 123, 109, 95, 102, 247, 244, 121, 198, 212, 24, 25, 179, 59, 48, 119, 199, 253, 176, 222, 16, 207, 248, 190, 116, 81, 187, 100, 103, 195, 1, 81, 185, 100, 177, 217, 53, 4, 252, 180, 123, 72, 150, 38, 5, 183, 128, 44, 60, 73, 29, 238, 31, 185, 188, 167, 240, 101, 92, 70, 65, 165, 254, 55, 111, 86, 245, 185, 13, 218, 55, 151, 151, 164, 136, 140 } });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                schema: "Message",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                schema: "Message",
                table: "Messages",
                column: "SenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Message");

            migrationBuilder.UpdateData(
                schema: "User",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "DateOfBirth", "LastActive", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2022, 6, 30, 8, 27, 11, 33, DateTimeKind.Local).AddTicks(7547), new DateTime(2022, 6, 30, 8, 27, 11, 33, DateTimeKind.Local).AddTicks(7601), new DateTime(2022, 6, 30, 8, 27, 11, 33, DateTimeKind.Local).AddTicks(7539), new byte[] { 175, 190, 41, 87, 11, 236, 251, 74, 239, 54, 70, 239, 182, 10, 252, 223, 71, 203, 78, 191, 55, 79, 203, 233, 16, 38, 156, 148, 134, 0, 57, 7, 225, 84, 194, 202, 90, 12, 139, 148, 38, 216, 237, 129, 55, 19, 37, 146, 129, 110, 207, 4, 245, 89, 42, 197, 174, 50, 99, 160, 118, 158, 5, 204 }, new byte[] { 162, 7, 113, 170, 132, 79, 224, 245, 101, 102, 207, 91, 39, 157, 232, 251, 64, 198, 59, 183, 20, 100, 206, 137, 132, 98, 31, 19, 134, 97, 159, 149, 187, 72, 203, 16, 198, 239, 194, 192, 150, 177, 80, 20, 164, 26, 30, 112, 82, 116, 102, 5, 0, 48, 12, 251, 151, 225, 255, 126, 110, 6, 5, 103, 144, 152, 198, 51, 165, 100, 113, 234, 162, 36, 162, 17, 155, 230, 102, 98, 129, 107, 243, 105, 1, 45, 129, 245, 37, 249, 99, 20, 138, 108, 102, 159, 175, 102, 229, 160, 177, 121, 225, 22, 45, 138, 54, 78, 92, 251, 214, 108, 8, 142, 75, 68, 44, 36, 132, 186, 138, 148, 9, 104, 251, 8, 119, 27 } });
        }
    }
}
