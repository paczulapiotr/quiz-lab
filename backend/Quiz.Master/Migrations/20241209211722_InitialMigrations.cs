using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quiz.Master.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MiniGameDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Archived = table.Column<bool>(type: "INTEGER", nullable: false),
                    DefinitionJsonData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniGameDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentMiniGameId = table.Column<Guid>(type: "TEXT", nullable: true),
                    GameSize = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    StartedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MiniGameInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MiniGameDefinitionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoundsJsonData = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniGameInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiniGameInstances_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiniGameInstances_MiniGameDefinitions_MiniGameDefinitionId",
                        column: x => x.MiniGameDefinitionId,
                        principalTable: "MiniGameDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DeviceId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MiniGameInstanceScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MiniGameInstanceId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiniGameInstanceScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiniGameInstanceScores_MiniGameInstances_MiniGameInstanceId",
                        column: x => x.MiniGameInstanceId,
                        principalTable: "MiniGameInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MiniGameInstanceScores_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_CurrentMiniGameId",
                table: "Games",
                column: "CurrentMiniGameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MiniGameInstanceScores_MiniGameInstanceId",
                table: "MiniGameInstanceScores",
                column: "MiniGameInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGameInstanceScores_PlayerId",
                table: "MiniGameInstanceScores",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGameInstances_GameId",
                table: "MiniGameInstances",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_MiniGameInstances_MiniGameDefinitionId",
                table: "MiniGameInstances",
                column: "MiniGameDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                table: "Players",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_MiniGameInstances_CurrentMiniGameId",
                table: "Games",
                column: "CurrentMiniGameId",
                principalTable: "MiniGameInstances",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_MiniGameInstances_CurrentMiniGameId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "MiniGameInstanceScores");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "MiniGameInstances");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "MiniGameDefinitions");
        }
    }
}
