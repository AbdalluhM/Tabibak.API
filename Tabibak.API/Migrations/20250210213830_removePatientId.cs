using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabibak.API.Migrations
{
    /// <inheritdoc />
    public partial class removePatientId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bafd3ae1-30e4-43ea-8ff1-cfd02aa0ccbd", "AQAAAAIAAYagAAAAEBAArd273QfkBU9yFyPqM9gC3ZIK65EwBIs7CXTtNpp61nbCIggd4b3PWakN8FGgOA==", "cdfe2f52-654f-4453-8ead-3f65e68575b7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bd4b542e-e155-422c-81d0-802b580249b4", "AQAAAAIAAYagAAAAEFz2JvltR5ParHBNKVLCo6mSi7rw13KB4m5M8PmCImstEElLsrtayMu+T9119ce0zA==", "9397b6fb-0ad5-4b53-9596-5335c8c09e07" });
        }
    }
}
