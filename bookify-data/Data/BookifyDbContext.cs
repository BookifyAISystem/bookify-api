using bookify_data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
		public DbSet<Role> Roles { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Bookshelf> Bookshelves { get; set; }
		public DbSet<Promotion> Promotions { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<BookshelfDetail> BookshelfDetails { get; set; }
		public DbSet<News> News { get; set; }
		public DbSet<Voucher> Vouchers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<Wishlist> Wishlists { get; set; }
		public DbSet<WishlistDetail> WishlistDetails { get; set; }
		public DbSet<Payment> Payments { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<BookAuthor> BookAuthors { get; set; }
		public DbSet<BookContentVersion> BookContentVersions { get; set; }
		public DbSet<BookCategory> BookCategories { get; set; }
		public DbSet<Note> Notes { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// ------------------------------
			// BẢNG Promotion
			// ------------------------------
			modelBuilder.Entity<Promotion>(entity =>
			{
				entity.ToTable("Promotion");
				entity.HasKey(e => e.PromotionId);
				// Nếu cần mapping chi tiết các cột:
				// entity.Property(e => e.Content)
				//       .HasColumnName("content")
				//       .HasColumnType("nvarchar(1000)");
			});

			// ------------------------------
			// BẢNG Author
			// ------------------------------
			modelBuilder.Entity<Author>(entity =>
			{
				entity.ToTable("Author");
				entity.HasKey(e => e.AuthorId);
			});

			// ------------------------------
			// BẢNG Category
			// ------------------------------
			modelBuilder.Entity<Category>(entity =>
			{
				entity.ToTable("Category");
				entity.HasKey(e => e.CategoryId);
			});

			// ------------------------------
			// BẢNG Book
			// ------------------------------
			modelBuilder.Entity<Book>(entity =>
			{
				entity.ToTable("Book");
				entity.HasKey(e => e.BookId);

				// FK: Book -> Promotion (promotionId)
				entity.HasOne(e => e.Promotion)
					  .WithMany(p => p.Books)
					  .HasForeignKey(e => e.PromotionId)
					  .HasConstraintName("FK_Book_promotionId");
				entity.HasOne(e => e.Author)
					  .WithMany(p => p.Books)
					  .HasForeignKey(e => e.AuthorId)
					  .HasConstraintName("FK_Book_authorId");

				// Lưu ý: Mặc dù Book có cột categoryId, nhưng trong DB không có ràng buộc FK trực tiếp.
				// Các quan hệ với Author, Collection,... được thể hiện qua các bảng phụ (BookAuthor, v.v).
			});

			// ------------------------------
			// BẢNG Account
			// ------------------------------
			modelBuilder.Entity<Account>(entity =>
			{
				entity.ToTable("Account");
				entity.HasKey(e => e.AccountId);

				// FK: Account -> Role (roleId)
				entity.HasOne(a => a.Role)
					  .WithMany(r => r.Accounts)
					  .HasForeignKey(a => a.RoleId)
					  .HasConstraintName("FK_Account_roleId");
			});


			// ------------------------------
			// BẢNG Bookshelf
			// ------------------------------
			modelBuilder.Entity<Bookshelf>(entity =>
			{
				entity.ToTable("Bookshelf");
				entity.HasKey(e => e.BookshelfId);

				// FK: Bookshelf -> Customer (customerId)
				entity.HasOne(bs => bs.Account)
					  .WithMany(c => c.Bookshelves)
					  .HasForeignKey(bs => bs.AccountId)
					  .HasConstraintName("FK_Bookshelf_accountId");
			});

			// ------------------------------
			// BẢNG BookshelfDetail
			// ------------------------------
			modelBuilder.Entity<BookshelfDetail>(entity =>
			{
				entity.ToTable("BookshelfDetail");
				entity.HasKey(e => e.BookshelfDetailId);

				// FK: BookshelfDetail -> Bookshelf (bookshelfId)
				entity.HasOne(bsd => bsd.Bookshelf)
					  .WithMany(bs => bs.BookshelfDetails)
					  .HasForeignKey(bsd => bsd.BookshelfId)
					  .HasConstraintName("FK_BookshelfDetail_bookshelfId");

				// FK: BookshelfDetail -> Book (bookId)
				entity.HasOne(bsd => bsd.Book)
					  .WithMany(b => b.BookshelfDetails)
					  .HasForeignKey(bsd => bsd.BookId)
					  .HasConstraintName("FK_BookshelfDetail_bookId");
			});

			// ------------------------------
			// BẢNG News
			// ------------------------------
			modelBuilder.Entity<News>(entity =>
			{
				entity.ToTable("News");
				entity.HasKey(e => e.NewsId);

				// FK: News -> Account (accountId)
				entity.HasOne(n => n.Account)
					  .WithMany(a => a.NewsList)
					  .HasForeignKey(n => n.AccountId)
					  .HasConstraintName("FK_News_accountId");
			});

			// ------------------------------
			// BẢNG Voucher
			// ------------------------------
			modelBuilder.Entity<Voucher>(entity =>
			{
				entity.ToTable("Voucher");
				entity.HasKey(e => e.VoucherId);
			});

			// ------------------------------
			// BẢNG Order
			// ------------------------------
			modelBuilder.Entity<Order>(entity =>
			{
				entity.ToTable("Order");
				entity.HasKey(e => e.OrderId);

				// FK: Order -> Voucher (voucherId)
				entity.HasOne(o => o.Voucher)
					  .WithMany(v => v.Orders)
					  .HasForeignKey(o => o.VoucherId)
					  .IsRequired(false)
                      .HasConstraintName("FK_Order_voucherId");

				// FK: Order -> Customer (customerId)
				entity.HasOne(o => o.Account)
					  .WithMany(c => c.Orders)
					  .HasForeignKey(o => o.AccountId)
					  .HasConstraintName("FK_Order_accountId");
			});

			// ------------------------------
			// BẢNG OrderDetail
			// ------------------------------
			modelBuilder.Entity<OrderDetail>(entity =>
			{
				entity.ToTable("OrderDetail");
				entity.HasKey(e => e.OrderDetailId);

				// FK: OrderDetail -> Order (orderId)
				entity.HasOne(od => od.Order)
					  .WithMany(o => o.OrderDetails)
					  .HasForeignKey(od => od.OrderId)
					  .HasConstraintName("FK_OrderDetail_orderId");

				// FK: OrderDetail -> Book (bookId)
				entity.HasOne(od => od.Book)
					  .WithMany(b => b.OrderDetails)
					  .HasForeignKey(od => od.BookId)
					  .HasConstraintName("FK_OrderDetail_bookId");
			});

			// ------------------------------
			// BẢNG Feedback
			// ------------------------------
			modelBuilder.Entity<Feedback>(entity =>
			{
				entity.ToTable("Feedback");
				entity.HasKey(e => e.FeedbackId);

				// FK: Feedback -> Book (bookId)
				entity.HasOne(f => f.Book)
					  .WithMany(b => b.Feedbacks)
					  .HasForeignKey(f => f.BookId)
					  .HasConstraintName("FK_Feedback_bookId");

				// FK: Feedback -> Customer (customerId)
				entity.HasOne(f => f.Account)
					  .WithMany(c => c.Feedbacks)
					  .HasForeignKey(f => f.AccountId)
					  .HasConstraintName("FK_Feedback_accountId");
			});

			// ------------------------------
			// BẢNG Wishlist
			// ------------------------------
			modelBuilder.Entity<Wishlist>(entity =>
			{
				entity.ToTable("Wishlist");
				entity.HasKey(e => e.WishlistId);

				// FK: Wishlist -> Customer (customerId)
				entity.HasOne(w => w.Account)
					  .WithMany(c => c.Wishlists)
					  .HasForeignKey(w => w.AccountId)
					  .HasConstraintName("FK_Wishlist_accountId");
			});
				
			// ------------------------------
			// BẢNG WishlistDetail
			// ------------------------------
			modelBuilder.Entity<WishlistDetail>(entity =>
			{
				entity.ToTable("WishlistDetail");
				entity.HasKey(e => e.WishlistDetailId);

				// FK: WishlistDetail -> Wishlist (wishlistId)
				entity.HasOne(wd => wd.Wishlist)
					  .WithMany(w => w.WishlistDetails)
					  .HasForeignKey(wd => wd.WishlistId)
					  .HasConstraintName("FK_WishlistDetail_wishlistId");

				// FK: WishlistDetail -> Book (bookId)
				entity.HasOne(wd => wd.Book)
					  .WithMany(b => b.WishlistDetails)
					  .HasForeignKey(wd => wd.BookId)
					  .HasConstraintName("FK_WishlistDetail_bookId");
			});

			// ------------------------------
			// BẢNG Payment
			// ------------------------------
			modelBuilder.Entity<Payment>(entity =>
			{
				entity.ToTable("Payment");
				entity.HasKey(e => e.PaymentId);

				// FK: Payment -> Order (orderId)
				entity.HasOne(p => p.Order)
					  .WithMany(o => o.Payments)
					  .HasForeignKey(p => p.OrderId)
					  .HasConstraintName("FK_Payment_orderId");
			});

			// ------------------------------
			// BẢNG Note
			// ------------------------------
			modelBuilder.Entity<Note>(entity =>
			{
				entity.ToTable("Note");
				entity.HasKey(e => e.NoteId);

				// FK: Note -> Account (accountId)
				entity.HasOne(n => n.Account)
					  .WithMany(a => a.Notes)
					  .HasForeignKey(n => n.AccountId)
					  .HasConstraintName("FK_Note_accountId");
			});

			// ------------------------------
			// BẢNG BookAuthor
			// ------------------------------
			modelBuilder.Entity<BookAuthor>(entity =>
			{
				entity.ToTable("BookAuthor");
				entity.HasKey(e => e.BookAuthorId);

				// FK: BookAuthor -> Book (bookId)
				entity.HasOne(ba => ba.Book)
					  .WithMany(b => b.BookAuthors)
					  .HasForeignKey(ba => ba.BookId)
					  .HasConstraintName("FK_BookAuthor_bookId");

				// FK: BookAuthor -> Author (authorId)
				entity.HasOne(ba => ba.Author)
					  .WithMany(a => a.BookAuthors)
					  .HasForeignKey(ba => ba.AuthorId)
					  .HasConstraintName("FK_BookAuthor_authorId");
			});

			// ------------------------------
			// BẢNG BookContentVersion
			// ------------------------------
			modelBuilder.Entity<BookContentVersion>(entity =>
			{
				entity.ToTable("BookContentVersion");
				entity.HasKey(e => e.BookContentVersionId);

				// FK: BookContentVersion -> Book (bookId)
				entity.HasOne(bcv => bcv.Book)
					  .WithMany(b => b.BookContentVersions)
					  .HasForeignKey(bcv => bcv.BookId)
					  .HasConstraintName("FK_BookContentVersion_bookId");
			});

			// ------------------------------
			// BẢNG BookCategory
			// ------------------------------
			modelBuilder.Entity<BookCategory>(entity =>
			{
				entity.ToTable("BookCategory");
				// Nếu tên cột trong DB là "bookCatagoryId" thì chỉnh sửa key tại đây
				entity.HasKey(e => e.BookCategoryId);

				// FK: BookCategory -> Category (categoryId)
				entity.HasOne(bc => bc.Category)
					  .WithMany(c => c.BookCategories)
					  .HasForeignKey(bc => bc.CategoryId)
					  .HasConstraintName("FK_BookCategory_categoryId");

				// FK: BookCategory -> Book (bookId)
				entity.HasOne(bc => bc.Book)
					  .WithMany(b => b.BookCategories)
					  .HasForeignKey(bc => bc.BookId)
					  .HasConstraintName("FK_BookCategory_bookId");
			});

			base.OnModelCreating(modelBuilder);
		}


	}
}
