using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FindeyVouchers.Domain.Migrations
{
    public partial class AddMerchantVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MerchantVouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MerchantId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ValidUntil = table.Column<DateTime>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantVouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MerchantVouchers_AspNetUsers_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantVouchers_MerchantId",
                table: "MerchantVouchers",
                column: "MerchantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantVouchers");
        }
    }
}
