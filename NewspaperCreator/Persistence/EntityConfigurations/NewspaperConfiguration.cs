using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Persistence.EntityConfigurations;

internal class NewspaperConfiguration : IEntityTypeConfiguration<Newspaper>
{
    public void Configure(EntityTypeBuilder<Newspaper> builder)
    {
        builder
            .HasOne(x => x.Owner)
            .WithMany(x => x.Newspapers)
            .HasForeignKey(x => x.OwnerId);
    }
}

