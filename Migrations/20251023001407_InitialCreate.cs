using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDSCalculatorDesktop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControlPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Distance = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enterprises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterprises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Substances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GroupLFV = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HazardClass = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discharges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    RegistrationAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EnterpriseId = table.Column<int>(type: "integer", nullable: false),
                    ControlPointId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Discharges_ControlPoints_ControlPointId",
                        column: x => x.ControlPointId,
                        principalTable: "ControlPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Discharges_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubstanceId = table.Column<int>(type: "integer", nullable: false),
                    DischargeId = table.Column<int>(type: "integer", nullable: true),
                    ControlPointId = table.Column<int>(type: "integer", nullable: true),
                    MeasurementType = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurements", x => x.Id);
                    table.CheckConstraint("CK_Measurement_XOR", "(\"DischargeId\" IS NOT NULL AND \"ControlPointId\" IS NULL) OR (\"DischargeId\" IS NULL AND \"ControlPointId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Measurements_ControlPoints_ControlPointId",
                        column: x => x.ControlPointId,
                        principalTable: "ControlPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Measurements_Discharges_DischargeId",
                        column: x => x.DischargeId,
                        principalTable: "Discharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Measurements_Substances_SubstanceId",
                        column: x => x.SubstanceId,
                        principalTable: "Substances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalParameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Diameter = table.Column<double>(type: "double precision", nullable: false),
                    FlowRate = table.Column<double>(type: "double precision", nullable: false),
                    WaterFlowVelocity = table.Column<double>(type: "double precision", nullable: false),
                    DischargeAngle = table.Column<double>(type: "double precision", nullable: false),
                    DistanceToWaterSurface = table.Column<double>(type: "double precision", nullable: false),
                    DistanceToShore = table.Column<double>(type: "double precision", nullable: false),
                    DistanceToControlPoint = table.Column<double>(type: "double precision", nullable: false),
                    DischargeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalParameters", x => x.Id);
                    table.CheckConstraint("CK_TechnicalParameters_Diameter", "\"Diameter\" > 0");
                    table.CheckConstraint("CK_TechnicalParameters_DischargeAngle", "\"DischargeAngle\" > 0");
                    table.CheckConstraint("CK_TechnicalParameters_DistanceToControlPoint", "\"DistanceToControlPoint\" > 0");
                    table.CheckConstraint("CK_TechnicalParameters_DistanceToShore", "\"DistanceToShore\" > 0");
                    table.CheckConstraint("CK_TechnicalParameters_DistanceToWaterSurface", "\"DistanceToWaterSurface\" > 0");
                    table.CheckConstraint("CK_TechnicalParameters_FlowRate", "\"FlowRate\" > 0");
                    table.CheckConstraint("CK_TechnicalParameters_WaterFlowVelocity", "\"WaterFlowVelocity\" > 0");
                    table.ForeignKey(
                        name: "FK_TechnicalParameters_Discharges_DischargeId",
                        column: x => x.DischargeId,
                        principalTable: "Discharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ControlPoints_Number",
                table: "ControlPoints",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_ControlPointId",
                table: "Discharges",
                column: "ControlPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_EnterpriseId",
                table: "Discharges",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enterprises_Code",
                table: "Enterprises",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_ControlPointId",
                table: "Measurements",
                column: "ControlPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_DischargeId",
                table: "Measurements",
                column: "DischargeId");

            migrationBuilder.CreateIndex(
                name: "IX_Measurements_SubstanceId",
                table: "Measurements",
                column: "SubstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Substances_Code",
                table: "Substances",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalParameters_DischargeId",
                table: "TechnicalParameters",
                column: "DischargeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropTable(
                name: "TechnicalParameters");

            migrationBuilder.DropTable(
                name: "Substances");

            migrationBuilder.DropTable(
                name: "Discharges");

            migrationBuilder.DropTable(
                name: "ControlPoints");

            migrationBuilder.DropTable(
                name: "Enterprises");
        }
    }
}