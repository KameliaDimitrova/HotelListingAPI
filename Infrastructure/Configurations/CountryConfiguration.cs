using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.Infrastructure.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
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
    }
}
