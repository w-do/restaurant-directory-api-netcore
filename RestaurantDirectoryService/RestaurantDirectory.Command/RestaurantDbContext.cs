using Microsoft.EntityFrameworkCore;
using RestaurantDirectory.Command.Models;

namespace RestaurantDirectory.Command
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RestaurantCuisineModel>()
                .HasKey(x => new { x.CuisineId, x.RestaurantId });
        }

        public DbSet<CityModel> Cities { get; set; }
        public DbSet<CuisineModel> Cuisines { get; set; }
        public DbSet<RestaurantCuisineModel> RestaurantCuisines { get; set; }
        public DbSet<RestaurantModel> Restaurants { get; set; }
    }
}
