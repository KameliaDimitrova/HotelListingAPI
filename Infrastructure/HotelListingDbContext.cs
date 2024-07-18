using HotelListingAPI.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Infrastructure;

public class HotelListingDbContext : IdentityDbContext<User>
{
    public HotelListingDbContext(DbContextOptions options) : base(options)
    {
            
    }
    public DbSet<Hotel> Hotels { get; set; }

    public DbSet<Country> Countries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RoleConfiguraion());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new HotelConfiguration());
    }
}