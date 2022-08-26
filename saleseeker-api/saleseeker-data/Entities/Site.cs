namespace saleseeker_data
{
    // Site
    public class Site
    {
        public int SiteId { get; set; } // SiteID (Primary key)
        public string SiteName { get; set; } // SiteName (length: 200)
        public string SiteHomeUrl { get; set; } // SiteHomeURL (length: 200)
        public string SiteLogoUrl { get; set; } // SiteLogoURL (length: 200)

        // Reverse navigation

        /// <summary>
        /// Child SiteItems where [SiteItem].[SiteID] point to this entity (FK_SiteItem_Site)
        /// </summary>
        public virtual ICollection<SiteItem> SiteItems { get; set; } // SiteItem.FK_SiteItem_Site

        /// <summary>
        /// Child SubscribedItems where [SubscribedItem].[SiteID] point to this entity (FK_SubscribedItem_Site)
        /// </summary>
        public virtual ICollection<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem.FK_SubscribedItem_Site

        public Site()
        {
            SiteItems = new List<SiteItem>();
            SubscribedItems = new List<SubscribedItem>();
        }
    }

}
