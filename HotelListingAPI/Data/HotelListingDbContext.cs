using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Data;

public class HotelListingDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Hotel> Hotels { get; set; }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "Jamica",
                ShortName = "JM",
                Hotels = []
            },
            new Country
            {
                Id = 2,
                Name = "Bahamas",
                ShortName = "BS",
                Hotels = []
            },
            new Country
            {
                Id = 3,
                Name = "Cayman island",
                ShortName = "CI",
                Hotels = []
            }
        );

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel
            {
                Id = 1,
                Name = "Sandals Resort and Spa",
                Address = "Negril",
                CountryId = 1,
                Rating = 4.5
            },
            new Hotel
            {
                Id = 2,
                Name = "Comfort Suits",
                Address = "George Town",
                CountryId = 3,
                Rating = 4.5
            },
            new Hotel
            {
                Id = 3,
                Name = "Grand palldium",
                Address = "Nassua",
                CountryId = 2,
                Rating = 4
            }
            );
    }
}