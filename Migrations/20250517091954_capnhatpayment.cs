using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopp.Migrations
{
    /// <inheritdoc />
    public partial class capnhatpayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypePayment",
                table: "tb_Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypePayment",
                table: "tb_Order");
        }
    }
}
