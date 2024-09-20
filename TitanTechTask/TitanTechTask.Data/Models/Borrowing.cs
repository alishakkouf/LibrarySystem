using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanTechTask.Data.Models
{
    public class Borrowing
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public int BorrowingId { get; set; } 
        public int UserId { get; set; } 
        public int BookId { get; set; } 
        public DateTime BorrowDate { get; set; }  
        public DateTime? ReturnDate { get; set; }  

        //Navigation Properties
        public Book Book { get; set; }
        public User User { get; set; }
    }

}
