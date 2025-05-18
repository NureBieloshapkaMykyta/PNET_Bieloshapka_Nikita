using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EntityConfigurations;

internal class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder
            .HasOne(x => x.Newspaper)
            .WithMany(x => x.Issues)
            .HasForeignKey(x => x.NewspaperId);
    }
}
