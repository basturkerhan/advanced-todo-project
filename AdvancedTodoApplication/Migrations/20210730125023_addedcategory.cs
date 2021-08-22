using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvancedTodoApplication.Migrations
{
    public partial class addedcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Board_BoardId",
                table: "ToDo");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "ToDo",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ToDo_BoardId",
                table: "ToDo",
                newName: "IX_ToDo_CategoryId");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Category_Board_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Board",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_BoardId",
                table: "Category",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Category_CategoryId",
                table: "ToDo",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDo_Category_CategoryId",
                table: "ToDo");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ToDo",
                newName: "BoardId");

            migrationBuilder.RenameIndex(
                name: "IX_ToDo_CategoryId",
                table: "ToDo",
                newName: "IX_ToDo_BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDo_Board_BoardId",
                table: "ToDo",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
