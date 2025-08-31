using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatQueueSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    IsOverflow = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShiftStart = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ShiftEnd = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Seniority = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CurrentChats = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agents_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LastPollTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedAgentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PollMissCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatSessions_Agents_AssignedAgentId",
                        column: x => x.AssignedAgentId,
                        principalTable: "Agents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChatSessions_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agents_TeamId",
                table: "Agents",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_AssignedAgentId",
                table: "ChatSessions",
                column: "AssignedAgentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_TeamId",
                table: "ChatSessions",
                column: "TeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatSessions");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
