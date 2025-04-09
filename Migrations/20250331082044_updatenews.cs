using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshopp.Migrations
{
    /// <inheritdoc />
    public partial class updatenews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_News_tb_Category_CategoryId",
                table: "tb_News");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "tb_News",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tb_News_tb_Category_CategoryId",
                table: "tb_News",
                column: "CategoryId",
                principalTable: "tb_Category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tb_News_tb_Category_CategoryId",
                table: "tb_News");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "tb_News",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tb_News_tb_Category_CategoryId",
                table: "tb_News",
                column: "CategoryId",
                principalTable: "tb_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
