using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdvancedTodoApplication.Migrations
{
    public partial class tododateupdatesecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "ToDo",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "ToDo");
        }
    }
}
