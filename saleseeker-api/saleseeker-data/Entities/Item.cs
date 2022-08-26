namespace saleseeker_data
{
    // ****************************************************************************************************
    // This is not a commercial licence, therefore only a few tables/views/stored procedures are generated.
    // ****************************************************************************************************

    // Item
    public class Item
    {
        public int ItemId { get; set; } // ItemID (Primary key)
        public int CategoryId { get; set; } // CategoryID
        public string ItemName { get; set; } // ItemName (length: 200)
        public string ItemPhotoUrl { get; set; } // ItemPhotoURL (length: 300)
        public string ItemDescription { get; set; } // ItemDescription
        public string ItemBarcode { get; set; } // ItemBarcode (length: 100)
        public int? PackId { get; set; } // PackID

        // Reverse navigation

        /// <summary>
        /// Child SiteItems where [SiteItem].[ItemID] point to this entity (FK_SiteItem_Item)
        /// </summary>
        public virtual ICollection<SiteItem> SiteItems { get; set; } // SiteItem.FK_SiteItem_Item

        /// <summary>
        /// Child SubscribedItems where [SubscribedItem].[ItemID] point to this entity (FK_SubscribedItem_Item)
        /// </summary>
        public virtual ICollection<SubscribedItem> SubscribedItems { get; set; } // SubscribedItem.FK_SubscribedItem_Item

        // Foreign keys

        /// <summary>
        /// Parent Pack pointed by [Item].([PackId]) (FK_Item_Pack)
        /// </summary>
        public virtual Pack Pack { get; set; } // FK_Item_Pack

        public Item()
        {
            SiteItems = new List<SiteItem>();
            SubscribedItems = new List<SubscribedItem>();
        }
    }

}
