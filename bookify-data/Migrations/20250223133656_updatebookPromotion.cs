using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookify_data.Migrations
{
    /// <inheritdoc />
    public partial class updatebookPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_promotionId",
                table: "Book");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "Book",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Book",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_promotionId",
                table: "Book",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "PromotionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_promotionId",
                table: "Book");

            migrationBuilder.AlterColumn<int>(
                name: "PromotionId",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_promotionId",
                table: "Book",
                column: "PromotionId",
                principalTable: "Promotion",
                principalColumn: "PromotionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
