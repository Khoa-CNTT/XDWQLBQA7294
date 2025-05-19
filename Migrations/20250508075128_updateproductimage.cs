using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopp.Migrations
{
    /// <inheritdoc />
    public partial class updateproductimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductImageid",
                table: "tb_ProductImage",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tb_Product",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_tb_ProductImage_ProductImageid",
                table: "tb_ProductImage",
                column: "ProductImageid");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_ProductImage_tb_ProductImage_ProductImageid",
                table: "tb_ProductImage",
                column: "ProductImageid",
                principalTable: "tb_ProductImage",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_ProductImage_tb_ProductImage_ProductImageid",
                table: "tb_ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_tb_ProductImage_ProductImageid",
                table: "tb_ProductImage");

            migrationBuilder.DropColumn(
                name: "ProductImageid",
                table: "tb_ProductImage");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "tb_Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
