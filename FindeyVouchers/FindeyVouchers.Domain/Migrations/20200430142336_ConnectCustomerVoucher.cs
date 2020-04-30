using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindeyVouchers.Domain.Migrations
{
    public partial class ConnectCustomerVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VoucherMerchantId",
                table: "CustomerVouchers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerVouchers_VoucherMerchantId",
                table: "CustomerVouchers",
                column: "VoucherMerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerVouchers_MerchantVouchers_VoucherMerchantId",
                table: "CustomerVouchers",
                column: "VoucherMerchantId",
                principalTable: "MerchantVouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerVouchers_MerchantVouchers_VoucherMerchantId",
                table: "CustomerVouchers");

            migrationBuilder.DropIndex(
                name: "IX_CustomerVouchers_VoucherMerchantId",
                table: "CustomerVouchers");

            migrationBuilder.DropColumn(
                name: "VoucherMerchantId",
                table: "CustomerVouchers");
        }
    }
}
