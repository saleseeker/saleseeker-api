namespace saleseeker_api.UI.Models
{
    public class UIItem
    {
        public int Id { get; }
        public string? Name { get; }
        public string? ImageUrl { get; }
        public decimal? AvePrice { get; }
        public List<UISiteItem> SiteItems { get; }

        public UIItem(int id, string name, string imageUrl)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            AvePrice = 0;
            SiteItems = new List<UISiteItem>();
        }

        public UIItem (int id, string name, string imageUrl, decimal avePrice, List<UISiteItem> siteItems)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            AvePrice = avePrice;
            SiteItems = siteItems;
        }
    }
}
