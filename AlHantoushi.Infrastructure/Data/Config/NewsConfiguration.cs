using AlHantoushi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlHantoushi.Infrastructure.Data.Config;

public class NewsConfiguration : IEntityTypeConfiguration<AlHantoushiNews>
{
    public void Configure(EntityTypeBuilder<AlHantoushiNews> builder)
    {
        builder.Property(x => x.TitleAr).IsRequired();
        builder.Property(x => x.TitleEn).IsRequired();
        builder.Property(x => x.DescriptionAr).IsRequired();
        builder.Property(x => x.DescriptionEn).IsRequired();
        builder.Property(x => x.Status).IsRequired();
    }
}
