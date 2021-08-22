using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvancedTodoApplication.Migrations
{
    public partial class usertodomanytomanyreladded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ToDo_ToDoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ToDoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ToDoId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserToDo",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToDoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToDo", x => new { x.UserId, x.ToDoId });
                    table.ForeignKey(
                        name: "FK_UserToDo_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToDo_ToDo_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserToDo_ToDoId",
                table: "UserToDo",
                column: "ToDoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserToDo");

            migrationBuilder.AddColumn<int>(
                name: "ToDoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ToDoId",
                table: "AspNetUsers",
                column: "ToDoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ToDo_ToDoId",
                table: "AspNetUsers",
                column: "ToDoId",
                principalTable: "ToDo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
