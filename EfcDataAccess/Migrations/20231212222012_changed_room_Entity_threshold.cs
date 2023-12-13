using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfcDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changed_room_Entity_threshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomProfiles_ThresholdsLimits_LimitsId",
                table: "RoomProfiles");

            migrationBuilder.DropIndex(
                name: "IX_RoomProfiles_LimitsId",
                table: "RoomProfiles");

            migrationBuilder.DropColumn(
                name: "LimitsId",
                table: "RoomProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LimitsId",
                table: "RoomProfiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RoomProfiles_LimitsId",
                table: "RoomProfiles",
                column: "LimitsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomProfiles_ThresholdsLimits_LimitsId",
                table: "RoomProfiles",
                column: "LimitsId",
                principalTable: "ThresholdsLimits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
