// <auto-generated>
// ReSharper disable All

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace saleseeker_data
{
    // SubscribedUser
    public class SubscribedUserConfiguration : IEntityTypeConfiguration<SubscribedUser>
    {
        public void Configure(EntityTypeBuilder<SubscribedUser> builder)
        {
            builder.ToTable("SubscribedUser", "dbo");
            builder.HasKey(x => x.UserId).HasName("PK_SubscribedUser").IsClustered();

            builder.Property(x => x.UserId).HasColumnName(@"UserID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.EmailAddress).HasColumnName(@"EmailAddress").HasColumnType("varchar(100)").IsRequired().IsUnicode(false).HasMaxLength(100);
            builder.Property(x => x.FirstName).HasColumnName(@"FirstName").HasColumnType("varchar(100)").IsRequired(false).IsUnicode(false).HasMaxLength(100);
            builder.Property(x => x.Surname).HasColumnName(@"Surname").HasColumnType("varchar(100)").IsRequired(false).IsUnicode(false).HasMaxLength(100);
            builder.Property(x => x.SubscribedDate).HasColumnName(@"SubscribedDate").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.VerifiedDate).HasColumnName(@"VerifiedDate").HasColumnType("datetime").IsRequired(false);
            builder.Property(x => x.CellNumber).HasColumnName(@"CellNumber").HasColumnType("varchar(20)").IsRequired(false).IsUnicode(false).HasMaxLength(20);
        }
    }

}
// </auto-generated>
