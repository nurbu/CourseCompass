using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseCompass.Migrations
{
    /// <inheritdoc />
    public partial class FixGradeColumnSize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollment_Student_Course_Semester",
                table: "Enrollments");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "Enrollments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Enrollments_StudentId",
                table: "Enrollments");

            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "Enrollments",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollment_Student_Course_Semester",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId", "Semester", "Year" },
                unique: true);
        }
    }
}
