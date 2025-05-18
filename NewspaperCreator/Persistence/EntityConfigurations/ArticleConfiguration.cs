using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Persistence.EntityConfigurations;

internal class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder
            .HasOne(x => x.Issue)
            .WithMany(x => x.Articles)
            .HasForeignKey(x => x.IssueId);

        builder
            .HasOne(x => x.Author)
            .WithMany(x => x.Articles)
            .HasForeignKey(x => x.AuthorId)
            
            .OnDelete(DeleteBehavior.Cascade);
    }
}
