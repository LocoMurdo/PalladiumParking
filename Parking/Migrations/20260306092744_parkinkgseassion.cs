using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class parkinkgseassion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashMovement_Payment_PaymentId",
                table: "CashMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_Rate_RateId",
                table: "ParkingSession");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSession_Vehicle_VehicleId",
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

            migrationBuilder.DropIndex(
                name: "IX_ParkingSession_VehicleId",
                table: "ParkingSession");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Rate");

            migrationBuilder.DropColumn(
                name: "VehicleId",
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

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Rates",
                newName: "PricePerHour");

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

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Rates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VehicleType",
                table: "Rates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "VisitorPlate",
                table: "ParkingSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ParkingSessions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Rates");

            migrationBuilder.RenameTable(
                name: "Rates",
                newName: "Rate");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "ParkingSessions",
                newName: "ParkingSession");

            migrationBuilder.RenameColumn(
                name: "PricePerHour",
                table: "Rate",
                newName: "Price");

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

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Rate",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VisitorPlate",
                table: "ParkingSession",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ParkingSession",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "ParkingSession",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSession_VehicleId",
                table: "ParkingSession",
                column: "VehicleId");

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
                name: "FK_ParkingSession_Vehicle_VehicleId",
                table: "ParkingSession",
                column: "VehicleId",
                principalTable: "Vehicle",
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
    }
}
