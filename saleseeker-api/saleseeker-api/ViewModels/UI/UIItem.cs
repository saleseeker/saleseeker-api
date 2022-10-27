using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using saleseeker_data;
using System.Linq;

namespace saleseeker_api.ViewModels.UI
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
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
            // This needs a better way of pulling 
            // Get all the sites items
            // This is still in Tests
            var siteItems = _context.SiteItems
                            .Include(i => i.Item)
                            .Include(si => si.Site)
                            .Join(_context.ScrapedItems,
                            si => si.SiteItemId,
                            sci => sci.SiteItemId,
                            (si, sci) => new
                            {
                                Id = si.Item.ItemId,
                                Name = si.Item.ItemName,
                                ItemUrl = si.ItemUrl,
                                SItemDetails = sci,
                                SiteDetails = si.Site,
                                ItemDetails = si.Item
                            })
                            .Distinct()
                            .GroupBy(item => item.Id,
                                    (key, g) => new { itemid = key, itemDts = g.ToList() })
                            .ToList();
            var allItems = new List<UIItem>();
            foreach (var items in siteItems)
            {
                var uiItem = new UIItem
                {
                    Id = items.itemid,
                    Name = items.itemDts.FirstOrDefault().ItemDetails.ItemName,
                    ImageUrl = items.itemDts.FirstOrDefault().ItemDetails.ItemPhotoUrl,
                    AvePrice = items.itemDts.Select(a => a.SItemDetails.PriceIncVat).Average(),
                    SiteItems = new List<UISiteItem>()
                };
                foreach (var siteItm in items.itemDts)
                {
                    var sItem = new UISiteItem
                    {
                        Id = siteItm.Id,
                        SiteId = siteItm.SiteDetails.SiteId,
                        Name = siteItm.SiteDetails.SiteName,
                        Url = siteItm.ItemUrl,
                        ImageUrl = siteItm.ItemDetails.ItemPhotoUrl,
                        Price = siteItm.SItemDetails.PriceIncVat,
                        LastUpdated = siteItm.SItemDetails.ScrapedDateTime
                    };

                    uiItem.SiteItems.Add(sItem);
                }
                allItems.Add(uiItem);
            }
            //var items3 = (from item in _context.Set<Item>()
            //        join siteItem in _context.Set<SiteItem>() on item.ItemId equals siteItem.ItemId
            //        join scraped in _context.Set<ScrapedItem>() on siteItem.SiteItemId equals scraped.SiteItemId
            //        group new { item, scraped } by new { item.ItemId, item.ItemName, item.ItemPhotoUrl } into g2
            //        select new UIItem(
            //            Id = g2.Key.ItemId,
            //            Name = g2.Key.ItemName,
            //            ImageUrl = g2.Key.ItemPhotoUrl,
            //            AvePrice = g2.Average(x => x.scraped.PriceIncVat),
            //            SiteItems = new List<UISiteItem>())
            //        ).ToList();

            return allItems;
        }
    }
}
