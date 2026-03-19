using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class parkinkgseassion02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashMovement_Payments_PaymentId",
                table: "CashMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSessions_Rates_RateId",
                table: "ParkingSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ParkingSessions_ParkingSessionId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_User_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Rates_RateId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rates",
                table: "Rates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSessions",
                table: "ParkingSessions");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "Rate");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "ParkingSessions",
                newName: "ParkingSession");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_UserId",
                table: "Payment",
                newName: "IX_Payment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ParkingSessionId",
                table: "Payment",
                newName: "IX_Payment_ParkingSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSessions_RateId",
                table: "ParkingSession",
                newName: "IX_ParkingSession_RateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rate",
                table: "Rate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSession",
                table: "ParkingSession",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashMovement_Payment_PaymentId",
                table: "CashMovement",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSession_Rate_RateId",
                table: "ParkingSession",
                column: "RateId",
                principalTable: "Rate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_ParkingSession_ParkingSessionId",
                table: "Payment",
                column: "ParkingSessionId",
                principalTable: "ParkingSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Rate_RateId",
                table: "Subscriptions",
                column: "RateId",
                principalTable: "Rate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashMovement_Payment_PaymentId",
                table: "CashMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_Rate_RateId",
                table: "ParkingSession");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_ParkingSession_ParkingSessionId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Rate_RateId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rate",
                table: "Rate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParkingSession",
                table: "ParkingSession");

            migrationBuilder.RenameTable(
                name: "Rate",
                newName: "Rates");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "ParkingSession",
                newName: "ParkingSessions");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_UserId",
                table: "Payments",
                newName: "IX_Payments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_ParkingSessionId",
                table: "Payments",
                newName: "IX_Payments_ParkingSessionId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSession_RateId",
                table: "ParkingSessions",
                newName: "IX_ParkingSessions_RateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rates",
                table: "Rates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParkingSessions",
                table: "ParkingSessions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashMovement_Payments_PaymentId",
                table: "CashMovement",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSessions_Rates_RateId",
                table: "ParkingSessions",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ParkingSessions_ParkingSessionId",
                table: "Payments",
                column: "ParkingSessionId",
                principalTable: "ParkingSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_User_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Rates_RateId",
                table: "Subscriptions",
                column: "RateId",
                principalTable: "Rates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
