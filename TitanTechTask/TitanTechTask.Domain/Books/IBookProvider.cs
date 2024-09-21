using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanTechTask.Domain.Books
{
    public interface IBookProvider
    {
        Task<List<BookDomain>> GetAllBooks();

        Task<List<BookDomain>> SearchBooks(string? title, string? author, string? isbn);

        Task BorrowBook(int userId, int bookId);

        Task ReturnBook(int bookId);

        Task<BookDomain> GetBook(int bookId);

        Task<List<BookDomain>> GetBorrowedBooksByUserIdAsync(int userId);
    }
}
