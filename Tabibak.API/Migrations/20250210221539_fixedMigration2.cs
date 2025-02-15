using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabibak.API.Migrations
{
    /// <inheritdoc />
    public partial class fixedMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Doctors");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2411eeb1-3f8e-479e-93ca-31ff07775d3c", "AQAAAAIAAYagAAAAEAMhb4AtTm5EoS92Q5uDMy5O26boJc+/KCuQMN1XjjNt9mJX5Up28Ry5Xwu6WGT0hA==", "40de3679-dacb-4f44-b45d-14f2929145e8" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c6397f42-4db1-40b4-88d0-ea97ef5c7dd1", "AQAAAAIAAYagAAAAEErYcsJvgSWmoL/Ippoj3LlH/wfrNbQuqQfi9K8YaEnO0SYlsMpwGmSHb0ouMErEoA==", "bad5925c-4db9-43d1-a081-b7a200b12082" });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_PatientId",
                table: "Doctors",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Patients_PatientId",
                table: "Doctors",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId");
        }
    }
}
