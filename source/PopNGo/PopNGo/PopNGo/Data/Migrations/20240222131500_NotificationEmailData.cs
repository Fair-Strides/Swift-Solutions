using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopNGo.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotificationEmailData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationEmail",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationEmail",
                table: "AspNetUsers");
        }
    }
}
