using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagingService.Migrations
{
    /// <inheritdoc />
    public partial class SecondCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverEmail",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "Messages");

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverId",
                table: "Messages",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SenderId",
                table: "Messages",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverEmail",
                table: "Messages",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "Messages",
                type: "longtext",
                nullable: true);
        }
    }
}
