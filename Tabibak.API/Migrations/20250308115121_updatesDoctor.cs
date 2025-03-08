using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabibak.API.Migrations
{
    /// <inheritdoc />
    public partial class updatesDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptPromoCode",
                table: "Doctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "47da9cf2-4721-4eec-affa-8b694d959e3f", "AQAAAAIAAYagAAAAEL2+2TA1/DV41w/qglzxWkhVHiTV2LGvFOQROLh/0VAu45t90q8ErcvX71Gnoawt1A==", "3398d7fb-ab3e-449f-9967-34856e1212bc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptPromoCode",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Doctors");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78bb6e83-6837-4593-bcf1-b01011a96b70", "AQAAAAIAAYagAAAAEOfNNwMe26wwIj886y5yWY/kd8KUO0d/mUcwTt5XXy6LwB++Eb9YCn4fpTr1fjEmxw==", "a1203e4d-f159-499a-bb58-7620c4130668" });
        }
    }
}
