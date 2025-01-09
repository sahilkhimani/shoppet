using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shoppetApi.Migrations
{
    /// <inheritdoc />
    public partial class pettablechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "petDesc",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "petDesc",
                table: "Pets");
        }
    }
}
