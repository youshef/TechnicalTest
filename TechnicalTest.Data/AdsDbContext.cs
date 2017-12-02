using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Data
{
    public class AdsDbContext : DbContext
    {
        public readonly ILogger _logger;


        //public AdsDbContext()
        //{

        //}
        public AdsDbContext(DbContextOptions options, ILogger<AdsDbContext> logger)
            : base(options)
        {
            _logger = logger;
            Database.EnsureCreated();
            Database.Migrate();

            //Enabled to have an already populated database on runtime
            //this.SeetTestData();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(c => c.Id);
            modelBuilder.Entity<Ad>().HasKey(c => c.Id);
            modelBuilder.Entity<Favorite>().HasKey(c => c.Id);
            modelBuilder.Entity<Favorite>().HasOne(x=>x.Ad).WithMany().HasForeignKey(x=>x.AdId).OnDelete(DeleteBehavior.Cascade);
        }



        public DbSet<User> Users { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
    }
}
