using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvancedTodoApplication.Migrations
{
    public partial class userboardmanytomanyreladded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Board_BoardId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BoardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserBoard",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBoard", x => new { x.UserId, x.BoardId });
                    table.ForeignKey(
                        name: "FK_UserBoard_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBoard_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBoard_BoardId",
                table: "UserBoard",
                column: "BoardId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBoard");

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BoardId",
                table: "AspNetUsers",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Board_BoardId",
                table: "AspNetUsers",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
