using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using saleseeker_DAL.Modals;

namespace saleseeker_DAL.Configurations
{
    internal sealed class SeedTable
    {
        internal class SiteConfiguration : IEntityTypeConfiguration<Site>
        {
            public void Configure(EntityTypeBuilder<Site> builder)
            {
                builder.ToTable("Sites");

                foreach (var site in Classes.Constants.SiteSeed.SiteList)
                {
                    builder.HasData(site);
                }
            }
        }
    }
}
