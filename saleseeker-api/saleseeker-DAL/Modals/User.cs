using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saleseeker_DAL.Modals
{
    public class User
    {
        [Key]
        public int Id { get; set; } // UserID (Primary key)
        public string EmailAddress { get; set; } // EmailAddress (length: 100)
        public DateTime SubscribedDate { get; set; } // SubscribedDate
        public DateTime? VerifiedDate { get; set; } // VerifiedDate
        public bool IsActive { get; set; } // CellNumber (length: 20
        
    }
}
