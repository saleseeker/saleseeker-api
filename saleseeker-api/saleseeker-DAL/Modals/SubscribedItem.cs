using System.ComponentModel.DataAnnotations;

namespace saleseeker_DAL.Modals
{
    public class SubscribedItem
    {
        [Key]
        public int Id { get; set; } // SubscribedItemID (Primary key)
        public int UserId { get; set; } // UserID
        public int ItemId { get; set; } // ItemID
        public int? SiteId { get; set; } // SiteID
        public decimal? BasePrice { get; set; } // BasePrice
        public int? PercentDiscount { get; set; } // PercentDiscount
        public decimal NotificationThreshold { get; set; } // NotificationThreshold

    }
}
