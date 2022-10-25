using saleseeker_api.UI.Models;
using saleseeker_data;

namespace saleseeker_api.Webscraper.Models
{
    public class WebscraperItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public float Price { get; set; }
        public string Selector { get; set; }
        public string PriceRegex { get; set; }

        public List<WebscraperItem> AllItems(SSDbContext _context)
        {
            return _context
                .SiteItems
                .Select(item =>
                         new WebscraperItem
                         {
                             Id = item.ItemId,
                             Name = item.Item.ItemName,
                             Url = item.ItemUrl,
                             Selector = item.Site.CssSelector,
                             PriceRegex = item.Site.PriceRegex
                         })
                .ToList() ?? new List<WebscraperItem>();
        }
    }
}
