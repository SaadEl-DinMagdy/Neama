using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neama.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class userProductEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photos",
                table: "UserProduct",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photos",
                table: "UserProduct");
        }
    }
}
