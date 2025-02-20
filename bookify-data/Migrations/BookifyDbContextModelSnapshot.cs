﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using bookify_data.Data;

#nullable disable

namespace bookify_data.Migrations
{
    [DbContext(typeof(BookifyDbContext))]
    partial class BookifyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("bookify_data.Entities.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.HasIndex("RoleId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("AuthorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("AuthorId");

                    b.ToTable("Author", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<int?>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("BookContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BookType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParentBookId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("PriceEbook")
                        .HasColumnType("int");

                    b.Property<int>("PromotionId")
                        .HasColumnType("int");

                    b.Property<int>("PublishYear")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentBookId");

                    b.HasIndex("PromotionId");

                    b.ToTable("Book", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.BookAuthor", b =>
                {
                    b.Property<int>("BookAuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookAuthorId"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookAuthorId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("BookId");

                    b.ToTable("BookAuthor", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.BookCategory", b =>
                {
                    b.Property<int>("BookCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookCategoryId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookCategoryId");

                    b.HasIndex("BookId");

                    b.HasIndex("CategoryId");

                    b.ToTable("BookCategory", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.BookContentVersion", b =>
                {
                    b.Property<int>("BookContentVersionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookContentVersionId"));

                    b.Property<string>("AiSummary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("BookContentVersionId");

                    b.HasIndex("BookId");

                    b.ToTable("BookContentVersion", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Bookshelf", b =>
                {
                    b.Property<int>("BookshelfId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookshelfId"));

                    b.Property<string>("BookShelfName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookshelfId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Bookshelf", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.BookshelfDetail", b =>
                {
                    b.Property<int>("BookshelfDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookshelfDetailId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("BookshelfId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("BookshelfDetailId");

                    b.HasIndex("BookId");

                    b.HasIndex("BookshelfId");

                    b.ToTable("BookshelfDetail", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("CustomerId");

                    b.HasIndex("AccountId");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Feedback", b =>
                {
                    b.Property<int>("FeedbackId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("FeedbackContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Star")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("FeedbackId");

                    b.HasIndex("BookId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Feedback", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.News", b =>
                {
                    b.Property<int>("NewsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NewsId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PublishAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Views")
                        .HasColumnType("int");

                    b.HasKey("NewsId");

                    b.HasIndex("AccountId");

                    b.HasIndex("CategoryId");

                    b.ToTable("News", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NoteId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("NoteId");

                    b.HasIndex("AccountId");

                    b.ToTable("Note", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("CancelReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.Property<int>("VoucherId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("VoucherId");

                    b.ToTable("Order", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.OrderDetail", b =>
                {
                    b.Property<int>("OrderDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderDetailId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("OrderDetailId");

                    b.HasIndex("BookId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderDetail", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Method")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Promotion", b =>
                {
                    b.Property<int>("PromotionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PromotionId"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("PromotionId");

                    b.ToTable("Promotion", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("bookify_data.Entities.Voucher", b =>
                {
                    b.Property<int>("VoucherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VoucherId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxDiscount")
                        .HasColumnType("int");

                    b.Property<int>("MinAmount")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("VoucherCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VoucherId");

                    b.ToTable("Voucher", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Wishlist", b =>
                {
                    b.Property<int>("WishlistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WishlistId"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId1")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("WishlistId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("CustomerId1")
                        .IsUnique()
                        .HasFilter("[CustomerId1] IS NOT NULL");

                    b.ToTable("Wishlist", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.WishlistDetail", b =>
                {
                    b.Property<int>("WishlistDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WishlistDetailId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastEdited")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("WishlistId")
                        .HasColumnType("int");

                    b.HasKey("WishlistDetailId");

                    b.HasIndex("BookId");

                    b.HasIndex("WishlistId");

                    b.ToTable("WishlistDetail", (string)null);
                });

            modelBuilder.Entity("bookify_data.Entities.Account", b =>
                {
                    b.HasOne("bookify_data.Entities.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Account_roleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("bookify_data.Entities.Book", b =>
                {
                    b.HasOne("bookify_data.Entities.Author", null)
                        .WithMany("Books")
                        .HasForeignKey("AuthorId");

                    b.HasOne("bookify_data.Entities.Book", "ParentBook")
                        .WithMany()
                        .HasForeignKey("ParentBookId");

                    b.HasOne("bookify_data.Entities.Promotion", "Promotion")
                        .WithMany("Books")
                        .HasForeignKey("PromotionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Book_promotionId");

                    b.Navigation("ParentBook");

                    b.Navigation("Promotion");
                });

            modelBuilder.Entity("bookify_data.Entities.BookAuthor", b =>
                {
                    b.HasOne("bookify_data.Entities.Author", "Author")
                        .WithMany("BookAuthors")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BookAuthor_authorId");

                    b.HasOne("bookify_data.Entities.Book", "Book")
                        .WithMany("BookAuthors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BookAuthor_bookId");

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("bookify_data.Entities.BookCategory", b =>
                {
                    b.HasOne("bookify_data.Entities.Book", "Book")
                        .WithMany("BookCategories")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BookCategory_bookId");

                    b.HasOne("bookify_data.Entities.Category", "Category")
                        .WithMany("BookCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BookCategory_categoryId");

                    b.Navigation("Book");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("bookify_data.Entities.BookContentVersion", b =>
                {
                    b.HasOne("bookify_data.Entities.Book", "Book")
                        .WithMany("BookContentVersions")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BookContentVersion_bookId");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("bookify_data.Entities.Bookshelf", b =>
                {
                    b.HasOne("bookify_data.Entities.Customer", "Customer")
                        .WithMany("Bookshelves")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Bookshelf_customerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("bookify_data.Entities.BookshelfDetail", b =>
                {
                    b.HasOne("bookify_data.Entities.Book", "Book")
                        .WithMany("BookshelfDetails")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BookshelfDetail_bookId");

                    b.HasOne("bookify_data.Entities.Bookshelf", "Bookshelf")
                        .WithMany("BookshelfDetails")
                        .HasForeignKey("BookshelfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_BookshelfDetail_bookshelfId");

                    b.Navigation("Book");

                    b.Navigation("Bookshelf");
                });

            modelBuilder.Entity("bookify_data.Entities.Customer", b =>
                {
                    b.HasOne("bookify_data.Entities.Account", "Account")
                        .WithMany("Customers")
                        .HasForeignKey("AccountId")
                        .HasConstraintName("FK_Customer_accountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("bookify_data.Entities.Feedback", b =>
                {
                    b.HasOne("bookify_data.Entities.Book", "Book")
                        .WithMany("Feedbacks")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Feedback_bookId");

                    b.HasOne("bookify_data.Entities.Customer", "Customer")
                        .WithMany("Feedbacks")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Feedback_customerId");

                    b.Navigation("Book");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("bookify_data.Entities.News", b =>
                {
                    b.HasOne("bookify_data.Entities.Account", "Account")
                        .WithMany("NewsList")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_News_accountId");

                    b.HasOne("bookify_data.Entities.Category", null)
                        .WithMany("NewsList")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("bookify_data.Entities.Note", b =>
                {
                    b.HasOne("bookify_data.Entities.Account", "Account")
                        .WithMany("Notes")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Note_accountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("bookify_data.Entities.Order", b =>
                {
                    b.HasOne("bookify_data.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_customerId");

                    b.HasOne("bookify_data.Entities.Voucher", "Voucher")
                        .WithMany("Orders")
                        .HasForeignKey("VoucherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Order_voucherId");

                    b.Navigation("Customer");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("bookify_data.Entities.OrderDetail", b =>
                {
                    b.HasOne("bookify_data.Entities.Book", "Book")
                        .WithMany("OrderDetails")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_bookId");

                    b.HasOne("bookify_data.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_OrderDetail_orderId");

                    b.Navigation("Book");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("bookify_data.Entities.Payment", b =>
                {
                    b.HasOne("bookify_data.Entities.Order", "Order")
                        .WithMany("Payments")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Payment_orderId");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("bookify_data.Entities.Wishlist", b =>
                {
                    b.HasOne("bookify_data.Entities.Customer", "Customer")
                        .WithMany("Wishlists")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Wishlist_customerId");

                    b.HasOne("bookify_data.Entities.Customer", null)
                        .WithOne("Wishlist")
                        .HasForeignKey("bookify_data.Entities.Wishlist", "CustomerId1");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("bookify_data.Entities.WishlistDetail", b =>
                {
                    b.HasOne("bookify_data.Entities.Book", "Book")
                        .WithMany("WishlistDetails")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_WishlistDetail_bookId");

                    b.HasOne("bookify_data.Entities.Wishlist", "Wishlist")
                        .WithMany("WishlistDetails")
                        .HasForeignKey("WishlistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_WishlistDetail_wishlistId");

                    b.Navigation("Book");

                    b.Navigation("Wishlist");
                });

            modelBuilder.Entity("bookify_data.Entities.Account", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("NewsList");

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("bookify_data.Entities.Author", b =>
                {
                    b.Navigation("BookAuthors");

                    b.Navigation("Books");
                });

            modelBuilder.Entity("bookify_data.Entities.Book", b =>
                {
                    b.Navigation("BookAuthors");

                    b.Navigation("BookCategories");

                    b.Navigation("BookContentVersions");

                    b.Navigation("BookshelfDetails");

                    b.Navigation("Feedbacks");

                    b.Navigation("OrderDetails");

                    b.Navigation("WishlistDetails");
                });

            modelBuilder.Entity("bookify_data.Entities.Bookshelf", b =>
                {
                    b.Navigation("BookshelfDetails");
                });

            modelBuilder.Entity("bookify_data.Entities.Category", b =>
                {
                    b.Navigation("BookCategories");

                    b.Navigation("NewsList");
                });

            modelBuilder.Entity("bookify_data.Entities.Customer", b =>
                {
                    b.Navigation("Bookshelves");

                    b.Navigation("Feedbacks");

                    b.Navigation("Orders");

                    b.Navigation("Wishlist");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("bookify_data.Entities.Order", b =>
                {
                    b.Navigation("OrderDetails");

                    b.Navigation("Payments");
                });

            modelBuilder.Entity("bookify_data.Entities.Promotion", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("bookify_data.Entities.Role", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("bookify_data.Entities.Voucher", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("bookify_data.Entities.Wishlist", b =>
                {
                    b.Navigation("WishlistDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
