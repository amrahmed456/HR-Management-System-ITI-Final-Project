using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    /// <inheritdoc />
    public partial class reports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendanceReportId",
                table: "EmployeeAttendance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AttendanceReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceReport_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction,
                        onUpdate: ReferentialAction.NoAction
                        );
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAttendance_AttendanceReportId",
                table: "EmployeeAttendance",
                column: "AttendanceReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceReport_EmployeeId",
                table: "AttendanceReport",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeAttendance_AttendanceReport_AttendanceReportId",
                table: "EmployeeAttendance",
                column: "AttendanceReportId",
                principalTable: "AttendanceReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeAttendance_AttendanceReport_AttendanceReportId",
                table: "EmployeeAttendance");

            migrationBuilder.DropTable(
                name: "AttendanceReport");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeAttendance_AttendanceReportId",
                table: "EmployeeAttendance");

            migrationBuilder.DropColumn(
                name: "AttendanceReportId",
                table: "EmployeeAttendance");
        }
    }
}
