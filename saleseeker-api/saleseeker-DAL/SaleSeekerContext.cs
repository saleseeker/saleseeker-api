using Microsoft.EntityFrameworkCore;
using saleseeker_DAL.Modals;
using static saleseeker_DAL.Configurations.SeedTable;

namespace saleseeker_DAL
{
    public class SaleSeekerContext : DbContext
    {
        public SaleSeekerContext(DbContextOptions options) : base(options)
        { }
       
        public DbSet<Item> Items { get; set; } // Item
        public DbSet<ItemType> ItemTypes { get; set; } // ItemType
        public DbSet<NotificationSent> NotificationSents { get; set; } // NotificationSent
        public DbSet<Pack> Packs { get; set; } // Pack
        public DbSet<ScrapedItem> ScrapedItems { get; set; } // ScrapedItem
        public DbSet<Site> Sites { get; set; } // Site
        public DbSet<SiteItem> SiteItems { get; set; } // SiteItem
        public DbSet<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem
        public DbSet<User> Users { get; set; } // User

        public DbSet<Unit> Units { get; set; } // Unit
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SiteConfiguration());
        }
    }
}
