using Microsoft.EntityFrameworkCore.Migrations;

namespace FindeyVouchers.Domain.Migrations
{
    public partial class AddVoucherType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherType",
                table: "MerchantVouchers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoucherType",
                table: "MerchantVouchers");
        }
    }
}
