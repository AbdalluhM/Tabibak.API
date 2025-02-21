using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabibak.API.Migrations
{
    /// <inheritdoc />
    public partial class updateApoointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Appointments",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "Appointments",
                type: "time",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78bb6e83-6837-4593-bcf1-b01011a96b70", "AQAAAAIAAYagAAAAEOfNNwMe26wwIj886y5yWY/kd8KUO0d/mUcwTt5XXy6LwB++Eb9YCn4fpTr1fjEmxw==", "a1203e4d-f159-499a-bb58-7620c4130668" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7958cf3a-c35a-4118-bc54-820bde49f0c3", "AQAAAAIAAYagAAAAEIF8RUOZpHscvUsyGy3Ywv1Y5EHqetgwdppL4CS7oP9ogREBiV4UcJosLocWxPAXhA==", "3be36578-8083-47f9-b467-61c780d77c49" });
        }
    }
}
