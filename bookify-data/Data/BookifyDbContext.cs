using bookify_data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Data
{
	public class BookifyDbContext : DbContext
	{
		public BookifyDbContext() { }

		public BookifyDbContext(DbContextOptions<BookifyDbContext> dbContextOptions) : base(dbContextOptions)
		{

		}
		public DbSet<Promotion> Promotions { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Collection> Collections { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Voucher> Vouchers { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Wishlist> Wishlists { get; set; }
		public DbSet<WishlistDetail> WishlistDetails { get; set; }
		public DbSet<BookShelf> BookShelves { get; set; }
		public DbSet<BookShelfDetail> BookShelfDetails { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// ------------------------------
			//  BẢNG Promotions
			// ------------------------------
			modelBuilder.Entity<Promotion>(entity =>
			{
				entity.ToTable("Promotions");
				entity.HasKey(e => e.PromotionId);

				// Nếu muốn mapping chi tiết cột:
				// entity.Property(e => e.PromotionId).HasColumnName("promotion_id");
				// entity.Property(e => e.Content).HasColumnName("content").HasColumnType("nvarchar(10001000)");
				// ...
			});

			// ------------------------------
			//  BẢNG Author
			// ------------------------------
			modelBuilder.Entity<Author>(entity =>
			{
				entity.ToTable("Author");
				entity.HasKey(e => e.AuthorId);
			});

			// ------------------------------
			//  BẢNG Collections
			// ------------------------------
			modelBuilder.Entity<Collection>(entity =>
			{
				entity.ToTable("Collections");
				entity.HasKey(e => e.CollectionId);
			});

			// ------------------------------
			//  BẢNG Categories
			// ------------------------------
			modelBuilder.Entity<Category>(entity =>
			{
				entity.ToTable("Categories");
				entity.HasKey(e => e.CategoryId);
			});

			// ------------------------------
			//  BẢNG Book
			// ------------------------------
			modelBuilder.Entity<Book>(entity =>
			{
				entity.ToTable("Book");
				entity.HasKey(e => e.BookId);

				// Khai báo FK: Book -> Promotions (promotion_id)
				entity.HasOne(e => e.Promotion)
					  .WithMany(p => p.Books)
					  .HasForeignKey(e => e.PromotionId)
					  .HasConstraintName("FK_Book.promotion_id");

				// Khai báo FK: Book -> Author (author_id)
				entity.HasOne(e => e.Author)
					  .WithMany(a => a.Books)
					  .HasForeignKey(e => e.AuthorId)
					  .HasConstraintName("FK_Book.author_id");

				// Khai báo FK: Book -> Collections (collection_id)
				entity.HasOne(e => e.Collection)
					  .WithMany(c => c.Books)
					  .HasForeignKey(e => e.CollectionId)
					  .HasConstraintName("FK_Book.collection_id");

				// Khai báo FK: Book -> Categories (category_id)
				entity.HasOne(e => e.Category)
					  .WithMany(cat => cat.Books)
					  .HasForeignKey(e => e.CategoryId)
					  .HasConstraintName("FK_Book.category_id");
			});

			// ------------------------------
			//  BẢNG Voucher
			// ------------------------------
			modelBuilder.Entity<Voucher>(entity =>
			{
				entity.ToTable("Voucher");
				entity.HasKey(e => e.VoucherId);
			});

			// ------------------------------
			//  BẢNG Role
			// ------------------------------
			modelBuilder.Entity<Role>(entity =>
			{
				entity.ToTable("Role");
				entity.HasKey(e => e.RoleId);
			});

			// ------------------------------
			//  BẢNG Account
			// ------------------------------
			modelBuilder.Entity<Account>(entity =>
			{
				entity.ToTable("Account");
				entity.HasKey(e => e.AccountId);

				// Khai báo FK: Account -> Role (role_id)
				entity.HasOne(a => a.Role)
					  .WithMany(r => r.Accounts)
					  .HasForeignKey(a => a.RoleId)
					  .HasConstraintName("FK_Account.role_id");
			});

			// ------------------------------
			//  BẢNG Customer
			// ------------------------------
			modelBuilder.Entity<Customer>(entity =>
			{
				entity.ToTable("Customer");
				entity.HasKey(e => e.CustomerId);

				// FK: Customer -> Account (acount_id)
				entity.HasOne(c => c.Account)
					  .WithMany(a => a.Customers)
					  .HasForeignKey(c => c.AccountId)
					  .HasConstraintName("FK_Customer.acount_id");
			});

			// ------------------------------
			//  BẢNG Order
			// ------------------------------
			modelBuilder.Entity<Order>(entity =>
			{
				entity.ToTable("Order");
				entity.HasKey(e => e.OrderId);

				// FK: Order -> Voucher (voucher_id)
				entity.HasOne(o => o.Voucher)
					  .WithMany(v => v.Orders)
					  .HasForeignKey(o => o.VoucherId)
					  .HasConstraintName("FK_Order.voucher_id");

				// FK: Order -> Customer (customer_id)
				entity.HasOne(o => o.Customer)
					  .WithMany(c => c.Orders)
					  .HasForeignKey(o => o.CustomerId)
					  .HasConstraintName("FK_Order.customer_id");
			});

			// ------------------------------
			//  BẢNG Payments
			// ------------------------------
			modelBuilder.Entity<Payment>(entity =>
			{
				entity.ToTable("Payments");
				entity.HasKey(e => e.PaymentId);

				// FK: Payment -> Order (order_id)
				entity.HasOne(p => p.Order)
					  .WithMany(o => o.Payments)
					  .HasForeignKey(p => p.OrderId)
					  .HasConstraintName("FK_Payments.order_id");
			});

			// ------------------------------
			//  BẢNG OrderDetails
			// ------------------------------
			modelBuilder.Entity<OrderDetail>(entity =>
			{
				entity.ToTable("OrderDetails");
				entity.HasKey(e => e.OrderdetailId);

				// FK: OrderDetail -> Order (order_id)
				entity.HasOne(od => od.Order)
					  .WithMany(o => o.OrderDetails)
					  .HasForeignKey(od => od.OrderId)
					  .HasConstraintName("FK_OrderDetails.order_id");

				// FK: OrderDetail -> Book (book_id)
				entity.HasOne(od => od.Book)
					  .WithMany(b => b.OrderDetails)
					  .HasForeignKey(od => od.BookId)
					  .HasConstraintName("FK_OrderDetails.book_id");
			});

			// ------------------------------
			//  BẢNG Wishlists
			// ------------------------------
			modelBuilder.Entity<Wishlist>(entity =>
			{
				entity.ToTable("Wishlists");
				entity.HasKey(e => e.WishlistId);

				// FK: Wishlist -> Customer (customer_id)
				entity.HasOne(w => w.Customer)
					  .WithMany(c => c.Wishlists)
					  .HasForeignKey(w => w.CustomerId)
					  .HasConstraintName("FK_Wishlists.customer_id");
			});

			// ------------------------------
			//  BẢNG WishlistDetail
			// ------------------------------
			modelBuilder.Entity<WishlistDetail>(entity =>
			{
				entity.ToTable("WishlistDetail");
				entity.HasKey(e => e.WishlistdetailId);

				// FK: WishlistDetail -> Wishlist (wishlist_id)
				entity.HasOne(wd => wd.Wishlist)
					  .WithMany(w => w.WishlistDetails)
					  .HasForeignKey(wd => wd.WishlistId)
					  .HasConstraintName("FK_WishlistDetail.wishlist_id");

				// FK: WishlistDetail -> Book (book_id)
				entity.HasOne(wd => wd.Book)
					  .WithMany(b => b.WishlistDetails)
					  .HasForeignKey(wd => wd.BookId)
					  .HasConstraintName("FK_WishlistDetail.book_id");
			});

			// ------------------------------
			//  BẢNG BookShelf
			// ------------------------------
			modelBuilder.Entity<BookShelf>(entity =>
			{
				entity.ToTable("BookShelf");
				entity.HasKey(e => e.BookshelfId);

				// FK: BookShelf -> Customer (customer_id)
				entity.HasOne(bs => bs.Customer)
					  .WithMany(c => c.BookShelves)
					  .HasForeignKey(bs => bs.CustomerId)
					  .HasConstraintName("FK_BookShelf.customer_id");
			});

			// ------------------------------
			//  BẢNG BookShelfDetail
			// ------------------------------
			modelBuilder.Entity<BookShelfDetail>(entity =>
			{
				entity.ToTable("BookShelfDetail");
				entity.HasKey(e => e.BookshelfdetailId);

				// FK: BookShelfDetail -> BookShelf (bookshelf_id)
				entity.HasOne(bsd => bsd.BookShelf)
					  .WithMany(bs => bs.BookShelfDetails)
					  .HasForeignKey(bsd => bsd.BookshelfId)
					  .HasConstraintName("FK_BookShelfDetail.bookshelf_id");

				// FK: BookShelfDetail -> Book (book_id)
				entity.HasOne(bsd => bsd.Book)
					  .WithMany(b => b.BookShelfDetails)
					  .HasForeignKey(bsd => bsd.BookId)
					  .HasConstraintName("FK_BookShelfDetail.book_id");
			});

			// ------------------------------
			//  BẢNG Feedbacks
			// ------------------------------
			modelBuilder.Entity<Feedback>(entity =>
			{
				entity.ToTable("Feedbacks");
				entity.HasKey(e => e.FeedbackId);

				// FK: Feedback -> Book (book_id)
				entity.HasOne(f => f.Book)
					  .WithMany(b => b.Feedbacks)
					  .HasForeignKey(f => f.BookId)
					  .HasConstraintName("FK_Feedbacks.book_id");

				// FK: Feedback -> Customer (customer_id)
				entity.HasOne(f => f.Customer)
					  .WithMany(c => c.Feedbacks)
					  .HasForeignKey(f => f.CustomerId)
					  .HasConstraintName("FK_Feedbacks.customer_id");
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
