using Microsoft.EntityFrameworkCore.Migrations;

namespace FindeyVouchers.Domain.Migrations
{
    public partial class AddStripeAccountId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeAccountId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeAccountId",
                table: "AspNetUsers");
        }
    }
}
