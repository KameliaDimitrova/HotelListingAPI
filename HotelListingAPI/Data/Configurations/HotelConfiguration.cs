﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.Data.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
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
