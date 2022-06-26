using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class RemoveFileMeta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_FileMetas_FileMetaId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "FileMetas");

            migrationBuilder.DropIndex(
                name: "IX_Files_FileMetaId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "FileMetaId",
                table: "Files");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileMetaId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FileMetas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModificationType = table.Column<int>(type: "int", nullable: false),
                    Modify = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMetas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_FileMetaId",
                table: "Files",
                column: "FileMetaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_FileMetas_FileMetaId",
                table: "Files",
                column: "FileMetaId",
                principalTable: "FileMetas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
