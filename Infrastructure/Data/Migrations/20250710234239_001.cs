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
                name: "ErrorDbModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ErrorType = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorDbModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileInfo = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Panels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Model = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Panels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    PanelDbModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Panels_PanelDbModelId",
                        column: x => x.PanelDbModelId,
                        principalTable: "Panels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperationDbModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    ErrorId = table.Column<int>(type: "INTEGER", nullable: false),
                    LogfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PanelDbModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationDbModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationDbModel_ErrorDbModel_ErrorId",
                        column: x => x.ErrorId,
                        principalTable: "ErrorDbModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationDbModel_Logfiles_LogfileId",
                        column: x => x.LogfileId,
                        principalTable: "Logfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationDbModel_Panels_PanelDbModelId",
                        column: x => x.PanelDbModelId,
                        principalTable: "Panels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Defects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ErrorFlag = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ErrorCode = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
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
                name: "IX_Boards_PanelDbModelId",
                table: "Boards",
                column: "PanelDbModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_BoardDbModelId",
                table: "Defects",
                column: "BoardDbModelId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationDbModel_ErrorId",
                table: "OperationDbModel",
                column: "ErrorId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationDbModel_LogfileId",
                table: "OperationDbModel",
                column: "LogfileId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationDbModel_PanelDbModelId",
                table: "OperationDbModel",
                column: "PanelDbModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Defects");

            migrationBuilder.DropTable(
                name: "OperationDbModel");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "ErrorDbModel");

            migrationBuilder.DropTable(
                name: "Logfiles");

            migrationBuilder.DropTable(
                name: "Panels");
        }
    }
}
