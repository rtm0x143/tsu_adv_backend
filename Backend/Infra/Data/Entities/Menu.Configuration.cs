using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Infra.Data.Entities;

internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.HasOne<Restaurant>()
            .WithMany()
            .HasForeignKey(menu => menu.RestaurantId);

        builder.HasKey(menu => new { menu.Name, menu.RestaurantId });
    }
}