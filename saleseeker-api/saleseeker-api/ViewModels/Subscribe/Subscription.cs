namespace saleseeker_api.ViewModels.Subscribe
{
    public class Subscription
    {
        public int ItemId { get; set; }
        
        public string? ItemName { get; set; }
        public int? SiteId { get; set; }
        public string? SiteName { get; set; }
        public decimal? BasePrice { get; set; }
        public int? PercentDiscount { get; set; }
        public decimal NotificationThreshold { get; set; }
        public bool? IsEnabled { get; set; }
    }
}
