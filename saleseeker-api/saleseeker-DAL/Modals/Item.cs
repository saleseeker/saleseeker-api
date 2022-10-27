using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace saleseeker_DAL.Modals
{
    public class Item
    {
        [Key]
        public int Id { get; set; } // ItemID (Primary key)
        
        public int CategoryId { get; set; } // CategoryID
        
        public string Name { get; set; } // ItemName (length: 200)
        
        public string PhotoUrl { get; set; } // ItemPhotoURL (length: 300)
        
        public string Description { get; set; } // ItemDescription
        
        public string Barcode { get; set; } // ItemBarcode (length: 100)
        
        public int PackId { get; set; } // PackID
        
        [ForeignKey("PackId")]
        public virtual Pack? Pack { get; set; }
    }
}
