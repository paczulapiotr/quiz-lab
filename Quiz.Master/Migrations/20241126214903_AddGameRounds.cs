using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Master.Migrations
{
    /// <inheritdoc />
    public partial class AddGameRounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rounds",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rounds",
                table: "Games");
        }
    }
}
