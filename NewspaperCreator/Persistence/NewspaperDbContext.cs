using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence;

public class NewspaperDbContext(DbContextOptions<NewspaperDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Newspaper> Newspapers { get; set; }

    public DbSet<Article> Articles { get; set; }

    public DbSet<Issue> Issues { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
