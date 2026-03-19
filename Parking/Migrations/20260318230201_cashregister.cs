using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class cashregister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashRegister_User_UserId",
                table: "CashRegister");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_UserId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_CashRegister_UserId",
                table: "CashRegister");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CashRegister");

            migrationBuilder.AddColumn<int>(
                name: "CashRegisterId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Method",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "CashRegister",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CashRegisterId",
                table: "Payment",
                column: "CashRegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_CashRegister_CashRegisterId",
                table: "Payment",
                column: "CashRegisterId",
                principalTable: "CashRegister",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_CashRegister_CashRegisterId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_CashRegisterId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CashRegisterId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "CashRegister",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CashRegister",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashRegister_UserId",
                table: "CashRegister",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashRegister_User_UserId",
                table: "CashRegister",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
