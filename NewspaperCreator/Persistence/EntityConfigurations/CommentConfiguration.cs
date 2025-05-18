using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Persistence.EntityConfigurations;

internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasOne(x => x.Article)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ArticleId);

        builder
           .HasOne(x => x.User)
           .WithMany(x => x.Comments)
           .HasForeignKey(x => x.UserId);
    }
}
