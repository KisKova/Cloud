using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EfcDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CloudDB_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThresholdsLimits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HumidityMin = table.Column<float>(type: "real", nullable: false),
                    HumidityMax = table.Column<float>(type: "real", nullable: false),
                    TemperatureMin = table.Column<float>(type: "real", nullable: false),
                    TemperatureMax = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThresholdsLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Token = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DataMeasures",
                columns: table => new
                {
                    SensorDataId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HomeId = table.Column<long>(type: "bigint", nullable: false),
                    TemperatureData = table.Column<float>(type: "real", nullable: false),
                    HumidityData = table.Column<float>(type: "real", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RoomProfileId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataMeasures", x => x.SensorDataId);
                });

            migrationBuilder.CreateTable(
                name: "LastMeasurement",
                columns: table => new
                {
                    LastMeasurementId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    LastDataMeasurementSensorDataId = table.Column<int>(type: "integer", nullable: true),
                    DeviceEui = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastMeasurement", x => x.LastMeasurementId);
                    table.ForeignKey(
                        name: "FK_LastMeasurement_DataMeasures_LastDataMeasurementSensorDataId",
                        column: x => x.LastDataMeasurementSensorDataId,
                        principalTable: "DataMeasures",
                        principalColumn: "SensorDataId");
                });

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    HomeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CurrentRoomProfileRoomProfileId = table.Column<long>(type: "bigint", nullable: true),
                    DeviceEui = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homes", x => x.HomeId);
                    table.ForeignKey(
                        name: "FK_Homes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomProfiles",
                columns: table => new
                {
                    RoomProfileId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomName = table.Column<string>(type: "text", nullable: false),
                    IdealTemperature = table.Column<float>(type: "real", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    IdealHumidity = table.Column<float>(type: "real", nullable: false),
                    LimitsId = table.Column<long>(type: "bigint", nullable: false),
                    HomeId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomProfiles", x => x.RoomProfileId);
                    table.ForeignKey(
                        name: "FK_RoomProfiles_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "HomeId");
                    table.ForeignKey(
                        name: "FK_RoomProfiles_ThresholdsLimits_LimitsId",
                        column: x => x.LimitsId,
                        principalTable: "ThresholdsLimits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataMeasures_HomeId",
                table: "DataMeasures",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_DataMeasures_RoomProfileId",
                table: "DataMeasures",
                column: "RoomProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_CurrentRoomProfileRoomProfileId",
                table: "Homes",
                column: "CurrentRoomProfileRoomProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_DeviceEui",
                table: "Homes",
                column: "DeviceEui",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homes_UserId",
                table: "Homes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LastMeasurement_LastDataMeasurementSensorDataId",
                table: "LastMeasurement",
                column: "LastDataMeasurementSensorDataId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomProfiles_HomeId",
                table: "RoomProfiles",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomProfiles_LimitsId",
                table: "RoomProfiles",
                column: "LimitsId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomProfiles_UserId",
                table: "RoomProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DataMeasures_Homes_HomeId",
                table: "DataMeasures",
                column: "HomeId",
                principalTable: "Homes",
                principalColumn: "HomeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DataMeasures_RoomProfiles_RoomProfileId",
                table: "DataMeasures",
                column: "RoomProfileId",
                principalTable: "RoomProfiles",
                principalColumn: "RoomProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_RoomProfiles_CurrentRoomProfileRoomProfileId",
                table: "Homes",
                column: "CurrentRoomProfileRoomProfileId",
                principalTable: "RoomProfiles",
                principalColumn: "RoomProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomProfiles_Homes_HomeId",
                table: "RoomProfiles");

            migrationBuilder.DropTable(
                name: "LastMeasurement");

            migrationBuilder.DropTable(
                name: "DataMeasures");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DropTable(
                name: "RoomProfiles");

            migrationBuilder.DropTable(
                name: "ThresholdsLimits");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
