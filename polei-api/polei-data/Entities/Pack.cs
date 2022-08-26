namespace polei_data
{
    // Pack
    public class Pack
    {
        public int PackId { get; set; } // PackID (Primary key)
        public string PackName { get; set; } // PackName (length: 100)
        public int ItemTypeId { get; set; } // ItemTypeID
        public int ItemCount { get; set; } // ItemCount
        public decimal? ItemQuantity { get; set; } // ItemQuantity
        public int? ItemQuantityUnitId { get; set; } // ItemQuantityUnitID
        public bool IsSingle { get; set; } // IsSingle

        // Reverse navigation

        /// <summary>
        /// Child Items where [Item].[PackID] point to this entity (FK_Item_Pack)
        /// </summary>
        public virtual ICollection<Item> Items { get; set; } // Item.FK_Item_Pack

        // Foreign keys

        /// <summary>
        /// Parent ItemType pointed by [Pack].([ItemTypeId]) (FK_Pack_ItemType)
        /// </summary>
        public virtual ItemType ItemType { get; set; } // FK_Pack_ItemType

        /// <summary>
        /// Parent Unit pointed by [Pack].([ItemQuantityUnitId]) (FK_Pack_Unit)
        /// </summary>
        public virtual Unit Unit { get; set; } // FK_Pack_Unit

        public Pack()
        {
            ItemQuantity = 1m;
            IsSingle = false;
            Items = new List<Item>();
        }
    }

}
