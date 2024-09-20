using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitanTechTask.Domain;

namespace TitanTechTask.Data.Models
{
    public class Book : IAuditedEntity
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Key]
        public int BookId { get; set; }        
        
        public string Title { get; set; }   
        
        public string Author { get; set; }  
        
        public string ISBN { get; set; }

        /// <summary>
        /// Availability Status (true = available, false = borrowed)
        /// </summary>
        public bool Available { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public long? ModifierUserId { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public List<Borrowing> Borrowings { get; set; }
    }

}
