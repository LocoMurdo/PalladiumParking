using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscriptionPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 20000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 2000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 15000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 30000.00m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 25000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 4,
                column: "Price",
                value: 3000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 5,
                column: "Price",
                value: 20000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 6,
                column: "Price",
                value: 35000.00m);
        }
    }
}
