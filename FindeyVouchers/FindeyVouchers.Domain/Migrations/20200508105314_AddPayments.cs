using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindeyVouchers.Domain.Migrations
{
    public partial class AddPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Customers");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "CustomerVouchers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Amount = table.Column<float>(nullable: false),
                    StripeId = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVouchers_PaymentId",
                table: "CustomerVouchers",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerVouchers_Payments_PaymentId",
                table: "CustomerVouchers",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerVouchers_Payments_PaymentId",
                table: "CustomerVouchers");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_CustomerVouchers_PaymentId",
                table: "CustomerVouchers");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "CustomerVouchers");

            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
