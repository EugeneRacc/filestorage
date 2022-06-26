using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ChangeFileForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Files_ParentId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_ParentId",
                table: "Files");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Files_ParentId",
                table: "Files",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Files_ParentId",
                table: "Files",
                column: "ParentId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
