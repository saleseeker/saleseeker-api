namespace polei_data
{
    // SiteItem
    public class SiteItem
    {
        public int SiteItemId { get; set; } // SiteItemID (Primary key)
        public int SiteId { get; set; } // SiteID
        public int ItemId { get; set; } // ItemID
        public string ItemUrl { get; set; } // ItemURL (length: 300)

        // Reverse navigation

        /// <summary>
        /// Child ScrapedItems where [ScrapedItem].[SiteItemID] point to this entity (FK_ScrapedItem_SiteItem)
        /// </summary>
        public virtual ICollection<ScrapedItem> ScrapedItems { get; set; } // ScrapedItem.FK_ScrapedItem_SiteItem

        // Foreign keys

        /// <summary>
        /// Parent Item pointed by [SiteItem].([ItemId]) (FK_SiteItem_Item)
        /// </summary>
        public virtual Item Item { get; set; } // FK_SiteItem_Item

        /// <summary>
        /// Parent Site pointed by [SiteItem].([SiteId]) (FK_SiteItem_Site)
        /// </summary>
        public virtual Site Site { get; set; } // FK_SiteItem_Site

        public SiteItem()
        {
            ScrapedItems = new List<ScrapedItem>();
        }
    }

}
