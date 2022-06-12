using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ChangeUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDiskSpaces_Users_UserId",
                table: "UserDiskSpaces");

            migrationBuilder.DropIndex(
                name: "IX_UserDiskSpaces_UserId",
                table: "UserDiskSpaces");

            migrationBuilder.DropColumn(
                name: "UsedDiskSpace",
                table: "UserDiskSpaces");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserDiskSpaces");

            migrationBuilder.RenameColumn(
                name: "UserDiskSpaceId",
                table: "Users",
                newName: "DiskSpaceId");

            migrationBuilder.AddColumn<string>(
                name: "UsedDiskSpade",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DiskSpaceId",
                table: "Users",
                column: "DiskSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserDiskSpaces_DiskSpaceId",
                table: "Users",
                column: "DiskSpaceId",
                principalTable: "UserDiskSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserDiskSpaces_DiskSpaceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_DiskSpaceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UsedDiskSpade",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "DiskSpaceId",
                table: "Users",
                newName: "UserDiskSpaceId");

            migrationBuilder.AddColumn<string>(
                name: "UsedDiskSpace",
                table: "UserDiskSpaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserDiskSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserDiskSpaces_UserId",
                table: "UserDiskSpaces",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDiskSpaces_Users_UserId",
                table: "UserDiskSpaces",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
