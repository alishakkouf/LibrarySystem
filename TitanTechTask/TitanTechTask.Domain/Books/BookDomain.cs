using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanTechTask.Domain.Books
{
    public class BookDomain
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string ISBN { get; set; }

        public bool Available { get; set; }

        public DateTime? BorrowedDate { get; set; }
    }
}
