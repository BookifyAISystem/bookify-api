using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookify_data.Migrations
{
    /// <inheritdoc />
    public partial class updateCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Wishlist",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_CustomerId",
                table: "Wishlist",
                newName: "IX_Wishlist_AccountId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Order",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                newName: "IX_Order_AccountId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Feedback",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_CustomerId",
                table: "Feedback",
                newName: "IX_Feedback_AccountId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Bookshelf",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookshelf_CustomerId",
                table: "Bookshelf",
                newName: "IX_Bookshelf_AccountId");

            migrationBuilder.AddColumn<int>(
                name: "AccountId1",
                table: "Wishlist",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_AccountId1",
                table: "Wishlist",
                column: "AccountId1",
                unique: true,
                filter: "[AccountId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookshelf_accountId",
                table: "Bookshelf",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_accountId",
                table: "Feedback",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_accountId",
                table: "Order",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Account_AccountId1",
                table: "Wishlist",
                column: "AccountId1",
                principalTable: "Account",
                principalColumn: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_accountId",
                table: "Wishlist",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookshelf_accountId",
                table: "Bookshelf");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_accountId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_accountId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Account_AccountId1",
                table: "Wishlist");

            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_accountId",
                table: "Wishlist");

            migrationBuilder.DropIndex(
                name: "IX_Wishlist_AccountId1",
                table: "Wishlist");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Wishlist");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Wishlist",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_AccountId",
                table: "Wishlist",
                newName: "IX_Wishlist_CustomerId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Order",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_AccountId",
                table: "Order",
                newName: "IX_Order_CustomerId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Feedback",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_AccountId",
                table: "Feedback",
                newName: "IX_Feedback_CustomerId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Bookshelf",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookshelf_AccountId",
                table: "Bookshelf",
                newName: "IX_Bookshelf_CustomerId");

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
    }
}
