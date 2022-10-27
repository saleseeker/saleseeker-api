using System.ComponentModel.DataAnnotations;

namespace saleseeker_DAL.Modals
{
    public class NotificationSent
    {
        [Key]
        public int Id { get; set; } // NotificationSentID (Primary key)
        public int UserId { get; set; } // UserID
        public DateTime SentDateTIme { get; set; } // SentDateTIme
        public string SentAddress { get; set; } // SentAddress (length: 100)
        public int ScrapedItemId { get; set; } // ScrapedItemID
        public decimal NotifiedPrice { get; set; } // NotifiedPrice
    }
}
