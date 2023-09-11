using FarDragi.Hanabi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarDragi.Hanabi.Database.Configurations;

public class TreatingEntityConfiguration : IEntityTypeConfiguration<TreatingEntity>
{
    public void Configure(EntityTypeBuilder<TreatingEntity> builder)
    {
        builder.ToTable("Treating");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Count).IsRequired();
    }
}