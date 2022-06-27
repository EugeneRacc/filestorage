using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildId",
                table: "Files");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "ChildId",
                table: "Files",
                type: "int",
                nullable: true);
        }
    }
}
