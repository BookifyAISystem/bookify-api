using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookify_data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelf_customerId",
                table: "Bookshelf");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_customerId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_customerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Customer_CustomerId1",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_customerId",
                table: "Wishlist");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Wishlist_CustomerId1",
                table: "Wishlist");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Wishlist");

            migrationBuilder.AddColumn<int>(
                name: "WishlistId",
                table: "Account",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_WishlistId",
                table: "Account",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Wishlist_WishlistId",
                table: "Account",
                column: "WishlistId",
                principalTable: "Wishlist",
                principalColumn: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelf_customerId",
                table: "Bookshelf",
                column: "CustomerId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_customerId",
                table: "Feedback",
                column: "CustomerId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_customerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_customerId",
                table: "Wishlist",
                column: "CustomerId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Wishlist_WishlistId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelf_customerId",
                table: "Bookshelf");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_customerId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_customerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_customerId",
                table: "Wishlist");

            migrationBuilder.DropIndex(
                name: "IX_Account_WishlistId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Account");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "Wishlist",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastEdited = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_accountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_CustomerId1",
                table: "Wishlist",
                column: "CustomerId1",
                unique: true,
                filter: "[CustomerId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AccountId",
                table: "Customer",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelf_customerId",
                table: "Bookshelf",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_customerId",
                table: "Feedback",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_customerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Customer_CustomerId1",
                table: "Wishlist",
                column: "CustomerId1",
                principalTable: "Customer",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_customerId",
                table: "Wishlist",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
