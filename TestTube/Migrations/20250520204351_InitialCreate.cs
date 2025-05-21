using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestTube.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scientists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scientists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ScientistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipment_Scientists_ScientistId",
                        column: x => x.ScientistId,
                        principalTable: "Scientists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Scientists",
                columns: new[] { "Id", "Department", "Email", "HireDate", "Name" },
                values: new object[,]
                {
                    { 1, "Physics", "marie.curie@testtube.com", new DateTime(2020, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Marie Curie" },
                    { 2, "Physics", "albert.einstein@testtube.com", new DateTime(2021, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Albert Einstein" },
                    { 3, "Chemistry", "rosalind.franklin@testtube.com", new DateTime(2022, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Rosalind Franklin" }
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "Id", "Manufacturer", "Name", "Price", "PurchaseDate", "ScientistId", "SerialNumber" },
                values: new object[,]
                {
                    { 1, "Zeiss", "Microscope", 15000.00m, new DateTime(2021, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, "MS-12345" },
                    { 2, "Thermo Fisher", "Centrifuge", 8500.00m, new DateTime(2022, 4, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, "CF-67890" },
                    { 3, "Agilent", "Spectrometer", 22000.00m, new DateTime(2023, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, "SP-24680" },
                    { 4, "Bio-Rad", "PCR Machine", 12000.00m, new DateTime(2023, 6, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "PCR-13579" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ScientistId",
                table: "Equipment",
                column: "ScientistId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Scientists");
        }
    }
}
