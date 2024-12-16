using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Master.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelWithState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoundsJsonData",
                table: "MiniGameInstances",
                newName: "StateJsonData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateJsonData",
                table: "MiniGameInstances",
                newName: "RoundsJsonData");
        }
    }
}
