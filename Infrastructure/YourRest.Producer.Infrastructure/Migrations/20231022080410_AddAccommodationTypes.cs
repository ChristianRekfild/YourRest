using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourRest.Producer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccommodationTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationTypes_Accommodations_AccommodationId",
                table: "AccommodationTypes");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationTypes_AccommodationId",
                table: "AccommodationTypes");

            migrationBuilder.DropColumn(
                name: "AccommodationId",
                table: "AccommodationTypes");

            migrationBuilder.DropColumn(
                name: "AccomodationId",
                table: "AccommodationTypes");

            migrationBuilder.AddColumn<int>(
                name: "AccommodationTypeId",
                table: "Accommodations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_AccommodationTypeId",
                table: "Accommodations",
                column: "AccommodationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_AccommodationTypes_AccommodationTypeId",
                table: "Accommodations",
                column: "AccommodationTypeId",
                principalTable: "AccommodationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_AccommodationTypes_AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.DropIndex(
                name: "IX_Accommodations_AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "AccommodationTypeId",
                table: "Accommodations");

            migrationBuilder.AddColumn<int>(
                name: "AccommodationId",
                table: "AccommodationTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccomodationId",
                table: "AccommodationTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationTypes_AccommodationId",
                table: "AccommodationTypes",
                column: "AccommodationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationTypes_Accommodations_AccommodationId",
                table: "AccommodationTypes",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
