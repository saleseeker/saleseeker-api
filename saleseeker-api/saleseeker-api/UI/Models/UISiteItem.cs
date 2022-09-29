namespace saleseeker_api.UI.Models
{
    public class UISiteItem
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal AvePrice { get; set; }
        public DateTime LastUpdated { get; set; }

        public UISiteItem()
        {
            Name = string.Empty;
            Url = string.Empty;
            ImageUrl = string.Empty;
        }

        public UISiteItem(int id, int siteId, string name, string url, string imageUrl, decimal price, decimal avePrice, DateTime lastUpdated)
        {
            Id = id;
            SiteId = siteId;
            Name = name;
            Url = url;
            ImageUrl = imageUrl;
            Price = price;
            AvePrice = avePrice;
            LastUpdated = lastUpdated;
        }
    }
}
