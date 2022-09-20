using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineChat.Migrations
{
    public partial class renameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationRoomUser_Conversations_RoomsId",
                table: "ConversationRoomUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationRoomId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ConversationRoomId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Conversations",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ConversationRoomId",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Conversations",
                newName: "ConversationRooms");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConversationRooms",
                table: "ConversationRooms",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RoomId",
                table: "Messages",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationRoomUser_ConversationRooms_RoomsId",
                table: "ConversationRoomUser",
                column: "RoomsId",
                principalTable: "ConversationRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ConversationRooms_RoomId",
                table: "Messages",
                column: "RoomId",
                principalTable: "ConversationRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationRoomUser_ConversationRooms_RoomsId",
                table: "ConversationRoomUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ConversationRooms_RoomId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_RoomId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConversationRooms",
                table: "ConversationRooms");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "ConversationRooms",
                newName: "Conversations");

            migrationBuilder.AddColumn<int>(
                name: "ConversationRoomId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Conversations",
                table: "Conversations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationRoomId",
                table: "Messages",
                column: "ConversationRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationRoomUser_Conversations_RoomsId",
                table: "ConversationRoomUser",
                column: "RoomsId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationRoomId",
                table: "Messages",
                column: "ConversationRoomId",
                principalTable: "Conversations",
                principalColumn: "Id");
        }
    }
}
