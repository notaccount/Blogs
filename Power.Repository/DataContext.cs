using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Power.Models;

namespace Power.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        public DbSet<PowerUser> PowerUser { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<BlogTag> BlogTag { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<PowerUser>().ToTable("PowerUser");
 
        }
    }
}
