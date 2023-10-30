using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourRest.Producer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Relationship_many_to_many_for_Room_and_RoomFacitity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomFacilities_Rooms_RoomId",
                table: "RoomFacilities");

            migrationBuilder.DropIndex(
                name: "IX_RoomFacilities_RoomId",
                table: "RoomFacilities");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "RoomFacilities");

            migrationBuilder.CreateTable(
                name: "RoomRoomFacility",
                columns: table => new
                {
                    RoomFacilitiesId = table.Column<int>(type: "integer", nullable: false),
                    RoomsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRoomFacility", x => new { x.RoomFacilitiesId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_RoomRoomFacility_RoomFacilities_RoomFacilitiesId",
                        column: x => x.RoomFacilitiesId,
                        principalTable: "RoomFacilities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRoomFacility_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRoomFacility_RoomsId",
                table: "RoomRoomFacility",
                column: "RoomsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRoomFacility");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "RoomFacilities",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RoomFacilities_RoomId",
                table: "RoomFacilities",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomFacilities_Rooms_RoomId",
                table: "RoomFacilities",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
