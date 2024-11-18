using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shoppetApi.Migrations
{
    /// <inheritdoc />
    public partial class role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId1",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_RoleId1",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoleId1",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId1",
                table: "User",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId1",
                table: "User",
                column: "RoleId1",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
