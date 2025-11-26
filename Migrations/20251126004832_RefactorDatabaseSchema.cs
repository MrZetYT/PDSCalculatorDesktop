using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PDSCalculatorDesktop.Migrations
{
    /// <inheritdoc />
    public partial class RefactorDatabaseSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_Enterprises_EnterpriseId",
                table: "Discharges");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalParameters_Discharges_DischargeId",
                table: "TechnicalParameters");

            migrationBuilder.DropTable(
                name: "Measurements");

            migrationBuilder.DropCheckConstraint(
                name: "CK_TechnicalParameters_DischargeAngle",
                table: "TechnicalParameters");

            migrationBuilder.DropColumn(
                name: "GroupLFV",
                table: "Substances");

            migrationBuilder.DropColumn(
                name: "HazardClass",
                table: "Substances");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "ControlPoints");

            migrationBuilder.RenameColumn(
                name: "RegistrationAt",
                table: "Discharges",
                newName: "RegistrationDate");

            migrationBuilder.AddColumn<double>(
                name: "KNK",
                table: "Substances",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "WaterUseTypeId",
                table: "ControlPoints",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BackgroundConcentrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ControlPointId = table.Column<int>(type: "integer", nullable: false),
                    SubstanceId = table.Column<int>(type: "integer", nullable: false),
                    Concentration = table.Column<double>(type: "double precision", nullable: false),
                    MeasurementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundConcentrations", x => x.Id);
                    table.CheckConstraint("CK_BackgroundConcentrations_Concentration", "\"Concentration\" >= 0");
                    table.ForeignKey(
                        name: "FK_BackgroundConcentrations_ControlPoints_ControlPointId",
                        column: x => x.ControlPointId,
                        principalTable: "ControlPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BackgroundConcentrations_Substances_SubstanceId",
                        column: x => x.SubstanceId,
                        principalTable: "Substances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DischargeConcentrations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DischargeId = table.Column<int>(type: "integer", nullable: false),
                    SubstanceId = table.Column<int>(type: "integer", nullable: false),
                    Concentration = table.Column<double>(type: "double precision", nullable: false),
                    MeasurementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DischargeConcentrations", x => x.Id);
                    table.CheckConstraint("CK_DischargeConcentrations_Concentration", "\"Concentration\" >= 0");
                    table.ForeignKey(
                        name: "FK_DischargeConcentrations_Discharges_DischargeId",
                        column: x => x.DischargeId,
                        principalTable: "Discharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DischargeConcentrations_Substances_SubstanceId",
                        column: x => x.SubstanceId,
                        principalTable: "Substances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WaterUseTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterUseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubstanceWaterUseCharacteristics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubstanceId = table.Column<int>(type: "integer", nullable: false),
                    WaterUseTypeId = table.Column<int>(type: "integer", nullable: false),
                    GroupLFV = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    HazardClass = table.Column<int>(type: "integer", nullable: false),
                    PDK = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstanceWaterUseCharacteristics", x => x.Id);
                    table.CheckConstraint("CK_SubstanceWaterUseCharacteristics_HazardClass", "\"HazardClass\" BETWEEN 1 AND 4");
                    table.CheckConstraint("CK_SubstanceWaterUseCharacteristics_PDK", "\"PDK\" > 0");
                    table.ForeignKey(
                        name: "FK_SubstanceWaterUseCharacteristics_Substances_SubstanceId",
                        column: x => x.SubstanceId,
                        principalTable: "Substances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubstanceWaterUseCharacteristics_WaterUseTypes_WaterUseType~",
                        column: x => x.WaterUseTypeId,
                        principalTable: "WaterUseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalParameters_ValidFrom",
                table: "TechnicalParameters",
                column: "ValidFrom");

            migrationBuilder.AddCheckConstraint(
                name: "CK_TechnicalParameters_DischargeAngle",
                table: "TechnicalParameters",
                sql: "\"DischargeAngle\" >= 0 AND \"DischargeAngle\" <= 360");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Substances_KNK",
                table: "Substances",
                sql: "\"KNK\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Discharges_Code",
                table: "Discharges",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlPoints_WaterUseTypeId",
                table: "ControlPoints",
                column: "WaterUseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundConcentrations_ControlPointId_SubstanceId_Measure~",
                table: "BackgroundConcentrations",
                columns: new[] { "ControlPointId", "SubstanceId", "MeasurementDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundConcentrations_MeasurementDate",
                table: "BackgroundConcentrations",
                column: "MeasurementDate");

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundConcentrations_SubstanceId",
                table: "BackgroundConcentrations",
                column: "SubstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_DischargeConcentrations_DischargeId_SubstanceId_Measurement~",
                table: "DischargeConcentrations",
                columns: new[] { "DischargeId", "SubstanceId", "MeasurementDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DischargeConcentrations_MeasurementDate",
                table: "DischargeConcentrations",
                column: "MeasurementDate");

            migrationBuilder.CreateIndex(
                name: "IX_DischargeConcentrations_SubstanceId",
                table: "DischargeConcentrations",
                column: "SubstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SubstanceWaterUseCharacteristics_SubstanceId_WaterUseTypeId",
                table: "SubstanceWaterUseCharacteristics",
                columns: new[] { "SubstanceId", "WaterUseTypeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstanceWaterUseCharacteristics_WaterUseTypeId",
                table: "SubstanceWaterUseCharacteristics",
                column: "WaterUseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterUseTypes_Code",
                table: "WaterUseTypes",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ControlPoints_WaterUseTypes_WaterUseTypeId",
                table: "ControlPoints",
                column: "WaterUseTypeId",
                principalTable: "WaterUseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_Enterprises_EnterpriseId",
                table: "Discharges",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalParameters_Discharges_DischargeId",
                table: "TechnicalParameters",
                column: "DischargeId",
                principalTable: "Discharges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ControlPoints_WaterUseTypes_WaterUseTypeId",
                table: "ControlPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Discharges_Enterprises_EnterpriseId",
                table: "Discharges");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalParameters_Discharges_DischargeId",
                table: "TechnicalParameters");

            migrationBuilder.DropTable(
                name: "BackgroundConcentrations");

            migrationBuilder.DropTable(
                name: "DischargeConcentrations");

            migrationBuilder.DropTable(
                name: "SubstanceWaterUseCharacteristics");

            migrationBuilder.DropTable(
                name: "WaterUseTypes");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalParameters_ValidFrom",
                table: "TechnicalParameters");

            migrationBuilder.DropCheckConstraint(
                name: "CK_TechnicalParameters_DischargeAngle",
                table: "TechnicalParameters");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Substances_KNK",
                table: "Substances");

            migrationBuilder.DropIndex(
                name: "IX_Discharges_Code",
                table: "Discharges");

            migrationBuilder.DropIndex(
                name: "IX_ControlPoints_WaterUseTypeId",
                table: "ControlPoints");

            migrationBuilder.DropColumn(
                name: "KNK",
                table: "Substances");

            migrationBuilder.DropColumn(
                name: "WaterUseTypeId",
                table: "ControlPoints");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "Discharges",
                newName: "RegistrationAt");

            migrationBuilder.AddColumn<string>(
                name: "GroupLFV",
                table: "Substances",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HazardClass",
                table: "Substances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "ControlPoints",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Measurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ControlPointId = table.Column<int>(type: "integer", nullable: true),
                    DischargeId = table.Column<int>(type: "integer", nullable: true),
                    SubstanceId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MeasurementType = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false)
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

            migrationBuilder.AddCheckConstraint(
                name: "CK_TechnicalParameters_DischargeAngle",
                table: "TechnicalParameters",
                sql: "\"DischargeAngle\" > 0");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Discharges_Enterprises_EnterpriseId",
                table: "Discharges",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalParameters_Discharges_DischargeId",
                table: "TechnicalParameters",
                column: "DischargeId",
                principalTable: "Discharges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
