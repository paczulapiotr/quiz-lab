using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Master.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentGamRoundToGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentRound",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRound",
                table: "Games");
        }
    }
}
