using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YourRest.Producer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccommodationFacility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccommodationFacility",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationFacility", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationFacilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccommodationFacilityId = table.Column<int>(type: "integer", nullable: false),
                    AccommodationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationFacilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccommodationFacilities_AccommodationFacility_Accommodation~",
                        column: x => x.AccommodationFacilityId,
                        principalTable: "AccommodationFacility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodationFacilities_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationFacilities_AccommodationFacilityId",
                table: "AccommodationFacilities",
                column: "AccommodationFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationFacilities_AccommodationId",
                table: "AccommodationFacilities",
                column: "AccommodationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationFacilities");

            migrationBuilder.DropTable(
                name: "AccommodationFacility");
        }
    }
}
