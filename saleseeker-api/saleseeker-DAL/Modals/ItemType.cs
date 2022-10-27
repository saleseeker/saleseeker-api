using System.ComponentModel.DataAnnotations;

namespace saleseeker_DAL.Modals
{
    public class ItemType
    {
        [Key]
        public int Id { get; set; } // ItemTypeID (Primary key)
        public string Name { get; set; } // ItemTypeName (length: 30)
    }
}
