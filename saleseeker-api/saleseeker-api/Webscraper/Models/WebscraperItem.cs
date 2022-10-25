using saleseeker_api.UI.Models;
using saleseeker_data;

namespace saleseeker_api.Webscraper.Models
{
    public class WebscraperItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public float price { get; set; }
        public string selector { get; set; }
        public string priceRegex { get; set; }

        public List<WebscraperItem> AllItems(SSDbContext _context)
        {
            return _context
                .SiteItems
                .Select(item =>
                         new WebscraperItem
                         {
                             id = item.ItemId,
                             name = item.Item.ItemName,
                             url = item.ItemUrl,
                             selector = item.Site.CssSelector,
                             priceRegex = item.Site.PriceRegex
                         })
                .ToList() ?? new List<WebscraperItem>();
        }

        public int UpdateItem(SSDbContext _context)
        {
            var vatPrice = price * 0.85;

            var scrapedItem = new ScrapedItem() {
                SiteItemId = id,
                PriceIncVat = (decimal)price,
                PriceExVat = (decimal?)vatPrice,
                ScrapedDateTime = DateTime.Now
            };

            var result = _context.SiteItems.First(a => a.ItemId == id);
            result.ScrapedItems.Add(scrapedItem);
            return _context.SaveChanges();
        }
    }
}
