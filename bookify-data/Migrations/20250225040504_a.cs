using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookify_data.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_voucherId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_voucherId",
                table: "Order",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "VoucherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_voucherId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_voucherId",
                table: "Order",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "VoucherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
