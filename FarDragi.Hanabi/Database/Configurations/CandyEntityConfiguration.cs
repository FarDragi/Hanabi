using FarDragi.Hanabi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarDragi.Hanabi.Database.Configurations;

public class CandyEntityConfiguration : IEntityTypeConfiguration<CandyEntity>
{
    public void Configure(EntityTypeBuilder<CandyEntity> builder)
    {
        builder.ToTable("Candies");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever().IsRequired();

        builder.Property(x => x.Count).IsRequired();
    }
}