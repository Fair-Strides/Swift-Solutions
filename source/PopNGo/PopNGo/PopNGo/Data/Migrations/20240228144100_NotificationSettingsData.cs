using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopNGo.Data.Migrations
{
    /// <inheritdoc />
    public partial class NotificationSettingsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NotifyWeekBefore",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
            
            migrationBuilder.AddColumn<bool>(
                name: "NotifyDayBefore",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
            
            migrationBuilder.AddColumn<bool>(
                name: "NotifyDayOf",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifyWeekBefore",
                table: "AspNetUsers");
            
            migrationBuilder.DropColumn(
                name: "NotifyDayBefore",
                table: "AspNetUsers");
            
            migrationBuilder.DropColumn(
                name: "NotifyDayOf",
                table: "AspNetUsers");
        }
    }
}
