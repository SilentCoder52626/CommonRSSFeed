using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonRSSFeed.Migrations
{
    /// <inheritdoc />
    public partial class addUserExtraColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppUsers");
        }
    }
}
