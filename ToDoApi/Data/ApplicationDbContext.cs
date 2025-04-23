using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<ToDoItem>()
                .HasIndex(t => t.Title)
                .IsUnique();
        }

        public DbSet<ToDoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; } = null!;
    }
}
