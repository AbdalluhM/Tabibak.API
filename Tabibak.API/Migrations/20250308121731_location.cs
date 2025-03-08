using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabibak.API.Migrations
{
    /// <inheritdoc />
    public partial class location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Doctors");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1001",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9cba580-1cb7-48b2-a9bd-4a5365901906", "AQAAAAIAAYagAAAAEIUsN0NcedtvKcv4Ul1jwbtHrbAbn+3utr6BjU9SwGDxEHzvl7QO5qwDrcm5kMvoKw==", "a1941685-aafc-41e5-a02c-a4ebfc3156ca" });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_LocationId",
                table: "Doctors",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Locations_LocationId",
                table: "Doctors",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Locations_LocationId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_LocationId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Doctors");

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
    }
}
