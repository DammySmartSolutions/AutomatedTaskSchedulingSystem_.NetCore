using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomatedTaskSchedulingSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeAvailandMakeEmpIDUniqueTODB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Employee_EmpID",
                table: "Employee",
                column: "EmpID");

            migrationBuilder.CreateTable(
                name: "EmployeeAvailability",
                columns: table => new
                {
                    AvailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpID = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    AvailDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Avail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAvailability", x => x.AvailID);
                    table.ForeignKey(
                        name: "FK_EmployeeAvailability_Employee_EmpID",
                        column: x => x.EmpID,
                        principalTable: "Employee",
                        principalColumn: "EmpID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmpID",
                table: "Employee",
                column: "EmpID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAvailability_EmpID",
                table: "EmployeeAvailability",
                column: "EmpID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeAvailability");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Employee_EmpID",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EmpID",
                table: "Employee");
        }
    }
}
