using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Narrative.Content.Domain;

namespace Narrative.Content.Data;

internal sealed class ArticleTypeConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.HasKey(article => article.Id);

        builder.Property(article => article.Id).ValueGeneratedNever().IsRequired();
        builder.Property(article => article.CreatedAtUtc).IsRequired();
        builder.Property(article => article.UpdatedAtUtc).IsRequired();
        builder.Property(article => article.Title).HasMaxLength(100).IsRequired();
        builder.Property(article => article.Description).HasMaxLength(500).IsRequired();
        builder.Property(article => article.Content).HasMaxLength(1000).IsRequired();
        builder.Property(article => article.PublishedAtUtc).IsRequired(false);
        builder.Property(article => article.Status).IsRequired();
    }
}