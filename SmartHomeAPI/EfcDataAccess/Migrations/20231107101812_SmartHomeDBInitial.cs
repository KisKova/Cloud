using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EfcDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SmartHomeDBInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThresholdsLimits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HumidityMin = table.Column<float>(type: "REAL", nullable: false),
                    HumidityMax = table.Column<float>(type: "REAL", nullable: false),
                    TemperatureMin = table.Column<float>(type: "REAL", nullable: false),
                    TemperatureMax = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThresholdsLimits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Homes",
                columns: table => new
                {
                    HomeId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    DeviceEui = table.Column<string>(type: "TEXT", nullable: true)
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
                    RoomProfileId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomName = table.Column<string>(type: "TEXT", nullable: false),
                    IdealTemperature = table.Column<float>(type: "REAL", nullable: false),
                    IdealHumidity = table.Column<float>(type: "REAL", nullable: false),
                    LimitsId = table.Column<long>(type: "INTEGER", nullable: false),
                    HomeId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "SensorData",
                columns: table => new
                {
                    SensorDataId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TemperatureData = table.Column<float>(type: "REAL", nullable: false),
                    HumidityData = table.Column<float>(type: "REAL", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HomeId = table.Column<long>(type: "INTEGER", nullable: true),
                    RoomProfileId = table.Column<long>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.SensorDataId);
                    table.ForeignKey(
                        name: "FK_SensorData_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "HomeId");
                    table.ForeignKey(
                        name: "FK_SensorData_RoomProfiles_RoomProfileId",
                        column: x => x.RoomProfileId,
                        principalTable: "RoomProfiles",
                        principalColumn: "RoomProfileId");
                });

            migrationBuilder.CreateTable(
                name: "LastMeasurements",
                columns: table => new
                {
                    LastMeasurementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    LastDataMeasurementSensorDataId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeviceEui = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastMeasurements", x => x.LastMeasurementId);
                    table.ForeignKey(
                        name: "FK_LastMeasurements_SensorData_LastDataMeasurementSensorDataId",
                        column: x => x.LastDataMeasurementSensorDataId,
                        principalTable: "SensorData",
                        principalColumn: "SensorDataId");
                });

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
                name: "IX_LastMeasurements_LastDataMeasurementSensorDataId",
                table: "LastMeasurements",
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
                name: "IX_SensorData_HomeId",
                table: "SensorData",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_RoomProfileId",
                table: "SensorData",
                column: "RoomProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastMeasurements");

            migrationBuilder.DropTable(
                name: "SensorData");

            migrationBuilder.DropTable(
                name: "RoomProfiles");

            migrationBuilder.DropTable(
                name: "Homes");

            migrationBuilder.DropTable(
                name: "ThresholdsLimits");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
