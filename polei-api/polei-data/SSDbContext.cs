using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Configuration.FileExtensions;

namespace saleseeker_data
{

    public class SSDbContext : DbContext, ISSDbContext
    {
        public SSDbContext()
        {
            
        }

        public SSDbContext(DbContextOptions<SSDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } // Category
        public DbSet<Item> Items { get; set; } // Item
        public DbSet<ItemType> ItemTypes { get; set; } // ItemType
        public DbSet<NotificationSent> NotificationSents { get; set; } // NotificationSent
        public DbSet<Pack> Packs { get; set; } // Pack
        public DbSet<ScrapedItem> ScrapedItems { get; set; } // ScrapedItem
        public DbSet<Site> Sites { get; set; } // Site
        public DbSet<SiteItem> SiteItems { get; set; } // SiteItem
        public DbSet<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem
        public DbSet<SubscribedUser> SubscribedUsers { get; set; } // SubscribedUser

        public DbSet<Unit> Units { get; set; } // Unit

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    IConfigurationRoot configuration = new ConfigurationBuilder()
            //       .SetBasePath(Directory.GetCurrentDirectory())
            //       .AddJsonFile("appsettings.json")
            //       .Build();
            //    var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
            //    optionsBuilder.UseSqlServer(connectionString);
            //}
        }

        public bool IsSqlParameterNull(SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as INullable;
            if (nullableValue != null)
                return nullableValue.IsNull;
            return (sqlValue == null || sqlValue == DBNull.Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationSentConfiguration());
            modelBuilder.ApplyConfiguration(new PackConfiguration());
            modelBuilder.ApplyConfiguration(new ScrapedItemConfiguration());
            modelBuilder.ApplyConfiguration(new SiteConfiguration());
            modelBuilder.ApplyConfiguration(new SiteItemConfiguration());
            modelBuilder.ApplyConfiguration(new SubscribedItemConfiguration());
            modelBuilder.ApplyConfiguration(new SubscribedUserConfiguration());
            modelBuilder.ApplyConfiguration(new UnitConfiguration());
        }

    }
}
