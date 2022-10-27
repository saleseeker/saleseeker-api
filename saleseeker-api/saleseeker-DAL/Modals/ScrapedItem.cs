using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace saleseeker_DAL.Modals
{
    public class ScrapedItem
    {
        [Key]
        public int Id { get; set; } 
        
        public int SiteItemId { get; set; } // SiteItemID
        
        [ForeignKey("SiteItemId")]
        public virtual SiteItem? SiteItem { get; set; }

        public int ItemId { get; set; } // ItemID
        [ForeignKey("ItemId")]
        public virtual Item? Item { get; set; }

        public decimal PriceIncVat { get; set; } // PriceIncVAT
        
        public decimal? PriceExVat { get; set; } // PriceExVAT
        
        public DateTime ScrapedDateTime { get; set; } // ScrapedDateTime
        public string CssSelector { get; set; }
        public string PriceRegex { get; set; }
    }
}
