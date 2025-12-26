using Microsoft.EntityFrameworkCore;
using Narrative.Content.Domain;

namespace Narrative.Content.Data;

internal sealed class ContentDbContext(DbContextOptions<ContentDbContext> options) : DbContext(options)
{
    internal DbSet<Article> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Content");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContentDbContext).Assembly);
    }
}