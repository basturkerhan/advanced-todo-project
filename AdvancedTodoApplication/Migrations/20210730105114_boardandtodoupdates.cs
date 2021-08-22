using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvancedTodoApplication.Migrations
{
    public partial class boardandtodoupdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "ToDo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Board",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToDoId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BoardId",
                table: "AspNetUsers",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ToDoId",
                table: "AspNetUsers",
                column: "ToDoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Board_BoardId",
                table: "AspNetUsers",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ToDo_ToDoId",
                table: "AspNetUsers",
                column: "ToDoId",
                principalTable: "ToDo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Board_BoardId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ToDo_ToDoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BoardId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ToDoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "ToDo");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ToDoId",
                table: "AspNetUsers");
        }
    }
}
