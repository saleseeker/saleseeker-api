using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace saleseeker_data
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category", "dbo");
            builder.HasKey(x => x.CategoryId).HasName("PK_Category").IsClustered();

            builder.Property(x => x.CategoryId).HasColumnName(@"CategoryID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.ParentCategoryId).HasColumnName(@"ParentCategoryID").HasColumnType("int");
            builder.Property(x => x.CategoryName).HasColumnName(@"CategoryName").HasColumnType("varchar(100)").IsRequired().IsUnicode(false).HasMaxLength(100);
            builder.Property(x => x.CategoryDescription).HasColumnName(@"CategoryDescription").HasColumnType("varchar(200)").IsUnicode(false).HasMaxLength(200);
        }
    }

}
