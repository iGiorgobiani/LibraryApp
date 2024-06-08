using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Database;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<BookGenre> BookGenres { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=IG\\SQLEXPRESS;Database=Library;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Author");

            entity.Property(e => e.Birthdate).HasColumnType("datetime");
            entity.Property(e => e.Firstname).HasMaxLength(100);
            entity.Property(e => e.Lastname).HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Book");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.ToTable("BookAuthor");

            entity.HasIndex(e => e.AuthorId, "IX_BookAuthor_AuthorId");

            entity.HasIndex(e => e.BookId, "IX_BookAuthor_BookId");

            entity.HasOne(d => d.Author).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("FK_BookAuthor_Author");

            entity.HasOne(d => d.Book).WithMany(p => p.BookAuthors)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_BookAuthor_Book");
        });

        modelBuilder.Entity<BookGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BookGenre");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
