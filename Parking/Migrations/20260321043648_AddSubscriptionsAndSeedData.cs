using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Parking.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionsAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Person_PersonId",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Subscriptions",
                newName: "VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_PersonId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_VehicleId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Plan",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Subscriptions",
                type: "decimal(16,2)",
                precision: 16,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubscriptionPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    Plan = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(16,2)", precision: 16, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPrice", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Rate",
                columns: new[] { "Id", "IsActive", "PricePerHour", "VehicleType" },
                values: new object[,]
                {
                    { 1, true, 1000.00m, 1 },
                    { 2, true, 800.00m, 2 }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionPrice",
                columns: new[] { "Id", "Plan", "Price", "VehicleType" },
                values: new object[,]
                {
                    { 1, 1, 5000.00m, 1 },
                    { 2, 2, 35000.00m, 1 },
                    { 3, 3, 60000.00m, 1 },
                    { 4, 1, 3000.00m, 2 },
                    { 5, 2, 20000.00m, 2 },
                    { 6, 3, 35000.00m, 2 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Vehicle_VehicleId",
                table: "Subscriptions",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Vehicle_VehicleId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubscriptionPrice");

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rate",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Subscriptions",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_VehicleId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_PersonId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Person_PersonId",
                table: "Subscriptions",
                column: "PersonId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
