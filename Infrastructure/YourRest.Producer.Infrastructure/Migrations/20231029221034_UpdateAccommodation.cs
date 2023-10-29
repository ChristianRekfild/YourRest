using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourRest.Producer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccommodation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccommodationTypeId",
                table: "Accommodations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Accommodations",
                type: "text",
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Accommodations");
        }
    }
}
