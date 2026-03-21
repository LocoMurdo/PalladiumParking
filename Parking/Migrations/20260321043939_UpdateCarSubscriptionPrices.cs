using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarSubscriptionPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 3000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 25000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 40000.00m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 5000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 35000.00m);

            migrationBuilder.UpdateData(
                table: "SubscriptionPrice",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 60000.00m);
        }
    }
}
