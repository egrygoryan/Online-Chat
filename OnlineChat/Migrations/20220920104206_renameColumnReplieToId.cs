using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineChat.Migrations
{
    public partial class renameColumnReplieToId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RepliedId",
                table: "Messages",
                newName: "ReplyToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReplyToId",
                table: "Messages",
                newName: "RepliedId");
        }
    }
}
