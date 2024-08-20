using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MessagingService.Migrations
{
    /// <inheritdoc />
    public partial class InitailCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SenderEmail = table.Column<string>(type: "longtext", nullable: true),
                    ReceiverEmail = table.Column<string>(type: "longtext", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsReceived = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message_Id", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
