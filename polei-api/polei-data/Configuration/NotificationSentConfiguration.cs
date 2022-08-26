// <auto-generated>
// ReSharper disable All

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace saleseeker_data
{
    // NotificationSent
    public class NotificationSentConfiguration : IEntityTypeConfiguration<NotificationSent>
    {
        public void Configure(EntityTypeBuilder<NotificationSent> builder)
        {
            builder.ToTable("NotificationSent", "dbo");
            builder.HasKey(x => x.NotificationSentId).HasName("PK_NotificationSent").IsClustered();

            builder.Property(x => x.NotificationSentId).HasColumnName(@"NotificationSentID").HasColumnType("int").IsRequired().ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(x => x.UserId).HasColumnName(@"UserID").HasColumnType("int").IsRequired();
            builder.Property(x => x.SentDateTIme).HasColumnName(@"SentDateTIme").HasColumnType("datetime").IsRequired();
            builder.Property(x => x.SentAddress).HasColumnName(@"SentAddress").HasColumnType("varchar(100)").IsRequired().IsUnicode(false).HasMaxLength(100);
            builder.Property(x => x.ScrapedItemId).HasColumnName(@"ScrapedItemID").HasColumnType("int").IsRequired();
            builder.Property(x => x.NotifiedPrice).HasColumnName(@"NotifiedPrice").HasColumnType("numeric(10,2)").HasPrecision(10,2).IsRequired();

            // Foreign keys
            builder.HasOne(a => a.ScrapedItem).WithMany(b => b.NotificationSents).HasForeignKey(c => c.ScrapedItemId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_NotificationSent_ScrapedItem");
            builder.HasOne(a => a.SubscribedUser).WithMany(b => b.NotificationSents).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_NotificationSent_SubscribedUser");
        }
    }

}
// </auto-generated>
