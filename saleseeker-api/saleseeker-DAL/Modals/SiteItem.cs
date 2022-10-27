using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace saleseeker_DAL.Modals
{
    public class SiteItem
    {
        [Key]
        public int Id { get; set; } // SiteItemID (Primary key)
        
        public int SiteId { get; set; } // SiteID
        [ForeignKey("SiteItemId")]
        public virtual Site? Site { get; set; }

        public int ItemId { get; set; } // ItemID
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        public string ItemUrl { get; set; } // ItemURL (length: 300)

    }
}
