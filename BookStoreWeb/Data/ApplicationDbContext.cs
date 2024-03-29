﻿using BookStoreWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Product> Products { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Product>(p =>
        //    {
        //        p.Property(b => b.Description).HasColumnType("ntext");
        //    });
        //}
    }
}
