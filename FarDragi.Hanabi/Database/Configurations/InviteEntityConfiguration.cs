using FarDragi.Hanabi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FarDragi.Hanabi.Database.Configurations;

public class InviteEntityConfiguration : IEntityTypeConfiguration<InviteEntity>
{
    public void Configure(EntityTypeBuilder<InviteEntity> builder)
    {
        builder.ToTable("Invites");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(15).IsRequired();

        builder.Property(x => x.Uses).IsRequired();

        builder.Property(x => x.UserId).IsRequired();
    }
}