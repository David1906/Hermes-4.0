using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class _001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileInfo = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Result = table.Column<int>(type: "INTEGER", nullable: false),
                    LogfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    UploadResult = table.Column<int>(type: "INTEGER", nullable: false),
                    UploadLogfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SkipForSampling = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAutoSend = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsManuallyRemove = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Logfiles_LogfileId",
                        column: x => x.LogfileId,
                        principalTable: "Logfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Operations_Logfiles_UploadLogfileId",
                        column: x => x.UploadLogfileId,
                        principalTable: "Logfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 124, nullable: false),
                    OperationDbModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Operations_OperationDbModelId",
                        column: x => x.OperationDbModelId,
                        principalTable: "Operations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Defects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsRealDefect = table.Column<bool>(type: "INTEGER", nullable: false),
                    ErrorFlag = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 124, nullable: false),
                    ErrorCode = table.Column<string>(type: "TEXT", maxLength: 124, nullable: false),
                    BoardDbModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defects_Boards_BoardDbModelId",
                        column: x => x.BoardDbModelId,
                        principalTable: "Boards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_OperationDbModelId",
                table: "Boards",
                column: "OperationDbModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_BoardDbModelId",
                table: "Defects",
                column: "BoardDbModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_LogfileId",
                table: "Operations",
                column: "LogfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_UploadLogfileId",
                table: "Operations",
                column: "UploadLogfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Defects");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Logfiles");
        }
    }
}
