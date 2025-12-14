using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsAndConfiguraion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_AuthorId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Chats_ChatId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ChatId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "AvatarPath",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsFile",
                table: "Messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUploaded",
                table: "Messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserChats",
                columns: table => new
                {
                    ChatsId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChats", x => new { x.ChatsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserChats_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChats_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserChats_UsersId",
                table: "UserChats",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_AuthorId",
                table: "Messages",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_AuthorId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "UserChats");

            migrationBuilder.DropColumn(
                name: "AvatarPath",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsFile",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsUploaded",
                table: "Messages");

            migrationBuilder.AddColumn<int>(
                name: "ChatId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ChatId",
                table: "Users",
                column: "ChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_AuthorId",
                table: "Messages",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Chats_ChatId",
                table: "Users",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id");
        }
    }
}
