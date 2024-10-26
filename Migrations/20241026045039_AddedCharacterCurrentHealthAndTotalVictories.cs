using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiRPG.Migrations
{
    /// <inheritdoc />
    public partial class AddedCharacterCurrentHealthAndTotalVictories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CurrentHealth",
                table: "Characters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "InGame",
                table: "Characters",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TotalVictories",
                table: "Characters",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentHealth",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "InGame",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "TotalVictories",
                table: "Characters");
        }
    }
}
