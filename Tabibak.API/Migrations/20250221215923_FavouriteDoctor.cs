using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabibak.API.Migrations
{
    /// <inheritdoc />
    public partial class FavouriteDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Qualification",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactInfo",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fees",
                table: "Doctors",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FavoriteDoctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteDoctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteDoctors_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteDoctors_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7958cf3a-c35a-4118-bc54-820bde49f0c3", "AQAAAAIAAYagAAAAEIF8RUOZpHscvUsyGy3Ywv1Y5EHqetgwdppL4CS7oP9ogREBiV4UcJosLocWxPAXhA==", "3be36578-8083-47f9-b467-61c780d77c49" });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteDoctors_DoctorId",
                table: "FavoriteDoctors",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteDoctors_PatientId",
                table: "FavoriteDoctors",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteDoctors");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Fees",
                table: "Doctors");

            migrationBuilder.AlterColumn<string>(
                name: "Qualification",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactInfo",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26395f0f-ac1c-4e8f-83d5-41ac792823a9", "AQAAAAIAAYagAAAAECLkNb+Od7hl50ZlLVm4bdbJckBQPRmFoIWveiHACJwdO6L1lu80Ecymv4MFSs29Lw==", "5baf10b1-0fc3-443f-a8ae-4cb3b5e32e90" });
        }
    }
}
