using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desktop.Data.Migrations
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
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    FileInfo = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false)
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
                        .Annotation("Sqlite:Autoincrement", true)
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
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 124, nullable: false),
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
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PanelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Panels_PanelId",
                        column: x => x.PanelId,
                        principalTable: "Panels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Result = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false),
                    LogfileId = table.Column<int>(type: "INTEGER", nullable: true),
                    OperationDbModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationTasks_Logfiles_LogfileId",
                        column: x => x.LogfileId,
                        principalTable: "Logfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationTasks_Operations_OperationDbModelId",
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
                    OperationTaskDbModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defects_OperationTasks_OperationTaskDbModelId",
                        column: x => x.OperationTaskDbModelId,
                        principalTable: "OperationTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_PanelDbModelId",
                table: "Boards",
                column: "PanelDbModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_OperationTaskDbModelId",
                table: "Defects",
                column: "OperationTaskDbModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_PanelId",
                table: "Operations",
                column: "PanelId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTasks_LogfileId",
                table: "OperationTasks",
                column: "LogfileId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationTasks_OperationDbModelId",
                table: "OperationTasks",
                column: "OperationDbModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Defects");

            migrationBuilder.DropTable(
                name: "OperationTasks");

            migrationBuilder.DropTable(
                name: "Logfiles");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Panels");
        }
    }
}
