using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindeyVouchers.Domain.Migrations
{
    public partial class AddCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MerchantVouchers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MerchantVouchers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "MerchantVouchers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VoucherCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    MerchantId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherCategories_AspNetUsers_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantVouchers_CategoryId",
                table: "MerchantVouchers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCategories_MerchantId",
                table: "VoucherCategories",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_MerchantVouchers_VoucherCategories_CategoryId",
                table: "MerchantVouchers",
                column: "CategoryId",
                principalTable: "VoucherCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MerchantVouchers_VoucherCategories_CategoryId",
                table: "MerchantVouchers");

            migrationBuilder.DropTable(
                name: "VoucherCategories");

            migrationBuilder.DropIndex(
                name: "IX_MerchantVouchers_CategoryId",
                table: "MerchantVouchers");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "MerchantVouchers");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MerchantVouchers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MerchantVouchers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
