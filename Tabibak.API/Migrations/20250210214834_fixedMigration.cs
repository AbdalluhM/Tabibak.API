using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabibak.API.Migrations
{
    /// <inheritdoc />
    public partial class fixedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Doctors_DoctorsDoctorId",
                table: "DoctorSpecialty");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Specialties_SpecialtiesSpecialtyId",
                table: "DoctorSpecialty");

            migrationBuilder.RenameColumn(
                name: "SpecialtiesSpecialtyId",
                table: "DoctorSpecialty",
                newName: "SpecialtyId");

            migrationBuilder.RenameColumn(
                name: "DoctorsDoctorId",
                table: "DoctorSpecialty",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpecialty_SpecialtiesSpecialtyId",
                table: "DoctorSpecialty",
                newName: "IX_DoctorSpecialty_SpecialtyId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c6397f42-4db1-40b4-88d0-ea97ef5c7dd1", "AQAAAAIAAYagAAAAEErYcsJvgSWmoL/Ippoj3LlH/wfrNbQuqQfi9K8YaEnO0SYlsMpwGmSHb0ouMErEoA==", "bad5925c-4db9-43d1-a081-b7a200b12082" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Doctors_DoctorId",
                table: "DoctorSpecialty",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Specialties_SpecialtyId",
                table: "DoctorSpecialty",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "SpecialtyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Doctors_DoctorId",
                table: "DoctorSpecialty");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorSpecialty_Specialties_SpecialtyId",
                table: "DoctorSpecialty");

            migrationBuilder.RenameColumn(
                name: "SpecialtyId",
                table: "DoctorSpecialty",
                newName: "SpecialtiesSpecialtyId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "DoctorSpecialty",
                newName: "DoctorsDoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorSpecialty_SpecialtyId",
                table: "DoctorSpecialty",
                newName: "IX_DoctorSpecialty_SpecialtiesSpecialtyId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bafd3ae1-30e4-43ea-8ff1-cfd02aa0ccbd", "AQAAAAIAAYagAAAAEBAArd273QfkBU9yFyPqM9gC3ZIK65EwBIs7CXTtNpp61nbCIggd4b3PWakN8FGgOA==", "cdfe2f52-654f-4453-8ead-3f65e68575b7" });

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Doctors_DoctorsDoctorId",
                table: "DoctorSpecialty",
                column: "DoctorsDoctorId",
                principalTable: "Doctors",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorSpecialty_Specialties_SpecialtiesSpecialtyId",
                table: "DoctorSpecialty",
                column: "SpecialtiesSpecialtyId",
                principalTable: "Specialties",
                principalColumn: "SpecialtyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
