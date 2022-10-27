using System.ComponentModel.DataAnnotations.Schema;

namespace saleseeker_DAL.Modals
{
    public class Pack
    {
        public int Id { get; set; } // PackID (Primary key)
        
        public string Name { get; set; } // PackName (length: 100)
        
        public int ItemTypeId { get; set; } // ItemTypeID
        
        [ForeignKey("ItemTypeId")]
        public virtual ItemType? ItemType { get; set; }

        public int Count { get; set; } // ItemCount
        
        public decimal? Quantity { get; set; } // ItemQuantity
        
        public int UnitId { get; set; } // ItemQuantityUnitID
        
        [ForeignKey("UnitId")]
        public virtual Unit? Unit { get; set; }
        
        public bool IsSingle { get; set; } // IsSingle

    }
}
