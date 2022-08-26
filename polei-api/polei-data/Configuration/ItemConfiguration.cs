using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace polei_data
{
    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    // Item
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item", "dbo");
            builder.HasKey(x => x.ItemId).HasName("PK_Item").IsClustered();

            builder.Property(x => x.ItemId).HasColumnName(@"ItemID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.CategoryId).HasColumnName(@"CategoryID").HasColumnType("int").IsRequired();
            builder.Property(x => x.ItemName).HasColumnName(@"ItemName").HasColumnType("varchar(200)").IsRequired().IsUnicode(false).HasMaxLength(200);
            builder.Property(x => x.ItemPhotoUrl).HasColumnName(@"ItemPhotoURL").HasColumnType("varchar(300)").IsRequired(false).IsUnicode(false).HasMaxLength(300);
            builder.Property(x => x.ItemDescription).HasColumnName(@"ItemDescription").HasColumnType("varchar(max)").IsRequired(false).IsUnicode(false);
            builder.Property(x => x.ItemBarcode).HasColumnName(@"ItemBarcode").HasColumnType("varchar(100)").IsRequired(false).IsUnicode(false).HasMaxLength(100);
            builder.Property(x => x.PackId).HasColumnName(@"PackID").HasColumnType("int").IsRequired(false);

            // Foreign keys
            builder.HasOne(a => a.Pack).WithMany(b => b.Items).HasForeignKey(c => c.PackId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_Item_Pack");
        }
    }

}
