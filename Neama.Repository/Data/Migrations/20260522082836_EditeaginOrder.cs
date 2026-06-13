using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Neama.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditeaginOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartnerId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Orders");
        }
    }
}
