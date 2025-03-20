using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookify_data.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlistNameToWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WishlistName",
                table: "Wishlist",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WishlistName",
                table: "Wishlist");
        }
    }
}
