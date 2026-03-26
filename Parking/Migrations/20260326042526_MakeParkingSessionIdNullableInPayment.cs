using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class MakeParkingSessionIdNullableInPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_ParkingSession_ParkingSessionId",
                table: "Payment");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingSessionId",
                table: "Payment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_ParkingSession_ParkingSessionId",
                table: "Payment",
                column: "ParkingSessionId",
                principalTable: "ParkingSession",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_ParkingSession_ParkingSessionId",
                table: "Payment");

            migrationBuilder.AlterColumn<int>(
                name: "ParkingSessionId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_ParkingSession_ParkingSessionId",
                table: "Payment",
                column: "ParkingSessionId",
                principalTable: "ParkingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
