// <auto-generated>
// ReSharper disable All

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace saleseeker_data
{
    // SiteItem
    public class SiteItemConfiguration : IEntityTypeConfiguration<SiteItem>
    {
        public void Configure(EntityTypeBuilder<SiteItem> builder)
        {
            builder.ToTable("SiteItem", "dbo");
            builder.HasKey(x => x.SiteItemId).HasName("PK_SiteItem").IsClustered();

            builder.Property(x => x.SiteItemId).HasColumnName(@"SiteItemID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.SiteId).HasColumnName(@"SiteID").HasColumnType("int").IsRequired();
            builder.Property(x => x.ItemId).HasColumnName(@"ItemID").HasColumnType("int").IsRequired();
            builder.Property(x => x.ItemUrl).HasColumnName(@"ItemURL").HasColumnType("varchar(300)").IsRequired().IsUnicode(false).HasMaxLength(300);

            // Foreign keys
            builder.HasOne(a => a.Item).WithMany(b => b.SiteItems).HasForeignKey(c => c.ItemId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SiteItem_Item");
            builder.HasOne(a => a.Site).WithMany(b => b.SiteItems).HasForeignKey(c => c.SiteId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SiteItem_Site");
        }
    }

}
// </auto-generated>
