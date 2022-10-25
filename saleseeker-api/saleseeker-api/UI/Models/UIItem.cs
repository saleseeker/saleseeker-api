using Microsoft.EntityFrameworkCore;
using saleseeker_data;
using System.Linq;

namespace saleseeker_api.UI.Models
{
    public class UIItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? AvePrice { get; set; }
        public List<UISiteItem> SiteItems { get; set; }

        public UIItem()
        {

        }

        public UIItem(int id, string name, string imageUrl)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            AvePrice = 0;
            SiteItems = new List<UISiteItem>();
        }

        public UIItem(int id, string name, string imageUrl, decimal avePrice, List<UISiteItem> siteItems)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            AvePrice = avePrice;
            SiteItems = siteItems;
        }

        public List<UIItem> AllItems(SSDbContext _context)
        {
            return _context
                .Items
                .Select(item =>
                         new UIItem
                         {
                             Id = item.ItemId,
                             Name = item.ItemName,
                             ImageUrl = item.ItemPhotoUrl

                         })
                .ToList() ?? new List<UIItem>();
        }

        public List<UIItem> ItemsWithAvePrices(SSDbContext _context)
        {
            var items3 = (from item in _context.Set<Item>()
                    join siteItem in _context.Set<SiteItem>() on item.ItemId equals siteItem.ItemId
                    join scraped in _context.Set<ScrapedItem>() on siteItem.SiteItemId equals scraped.SiteItemId
                    group new { item, scraped } by new { item.ItemId, item.ItemName, item.ItemPhotoUrl } into g2
                    select new UIItem(
                        g2.Key.ItemId,
                        g2.Key.ItemName,
                        g2.Key.ItemPhotoUrl,
                        g2.Average(x => x.scraped.PriceIncVat),
                        new List<UISiteItem>()
                        )
                            ).ToList();

            return items3;
        }
    }
}
