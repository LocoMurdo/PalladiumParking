using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class parkinkgseassion03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Payment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
