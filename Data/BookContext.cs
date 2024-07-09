using Microsoft.EntityFrameworkCore;
using ApiBooks.Models;

namespace ApiBooks.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }
        
        public DbSet<Book> Book { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<Book>().HasNoKey();
            modelBuilder.Entity<Book>().HasNoKey().ToView("sp_GetAllBooks");
            modelBuilder.Entity<Book>().HasNoKey().ToView("sp_GetBookById");
            modelBuilder.Entity<Book>().HasNoKey().ToView("sp_AddDigitalBook");
            modelBuilder.Entity<Book>().HasNoKey().ToView("sp_UpdateDigitalBook");
            modelBuilder.Entity<Book>().HasNoKey().ToView("sp_DeleteDigitalBook");
        }
    }
}