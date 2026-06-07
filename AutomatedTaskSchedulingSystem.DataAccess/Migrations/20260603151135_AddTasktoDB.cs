using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomatedTaskSchedulingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTasktoDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocId = table.Column<int>(type: "int", nullable: false),
                    MinEmployees = table.Column<int>(type: "int", nullable: false),
                    MaxEmployees = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.TaskID);
                    table.ForeignKey(
                        name: "FK_Tasks_Location_LocId",
                        column: x => x.LocId,
                        principalTable: "Location",
                        principalColumn: "LocId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_LocId",
                table: "Tasks",
                column: "LocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
