using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApp.Models;

namespace RestaurantWebApp.Data
{
    public class RestaurantWebAppContext : DbContext
    {
        public RestaurantWebAppContext (DbContextOptions<RestaurantWebAppContext> options)
            : base(options)
        {
        }

        public DbSet<FoodItem> FoodItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodItem>().ToTable("FoodItem");
        }
    }
}
