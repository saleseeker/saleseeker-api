using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace polei_data
{
    // Unit
    public class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            builder.ToTable("Unit", "dbo");
            builder.HasKey(x => x.UnitId).HasName("PK_Unit").IsClustered();

            builder.Property(x => x.UnitId).HasColumnName(@"UnitID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.UnitName).HasColumnName(@"UnitName").HasColumnType("varchar(30)").IsRequired().IsUnicode(false).HasMaxLength(30);
        }
    }

}
