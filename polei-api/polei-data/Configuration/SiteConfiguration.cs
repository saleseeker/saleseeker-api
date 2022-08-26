// <auto-generated>
// ReSharper disable All

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace saleseeker_data
{
    // Site
    public class SiteConfiguration : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.ToTable("Site", "dbo");
            builder.HasKey(x => x.SiteId).HasName("PK_Site").IsClustered();

            builder.Property(x => x.SiteId).HasColumnName(@"SiteID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.SiteName).HasColumnName(@"SiteName").HasColumnType("varchar(200)").IsRequired().IsUnicode(false).HasMaxLength(200);
            builder.Property(x => x.SiteHomeUrl).HasColumnName(@"SiteHomeURL").HasColumnType("varchar(200)").IsRequired().IsUnicode(false).HasMaxLength(200);
            builder.Property(x => x.SiteLogoUrl).HasColumnName(@"SiteLogoURL").HasColumnType("varchar(200)").IsRequired(false).IsUnicode(false).HasMaxLength(200);
        }
    }

}
// </auto-generated>
