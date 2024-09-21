using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanTechTask.Domain.Books
{
    public interface IBookManager
    {
        Task<List<BookDomain>> GetAllBooks();

        Task<List<BookDomain>> SearchBooks(string? title, string? author, string? isbn);

        Task BorrowBook(int userId, int bookId);

        Task<List<BookDomain>> GetBorrowedBooksByUserIdAsync(int userId);

        Task ReturnBook(int bookId);

        Task<BookDomain> GetBookByIdAsync(int bookId);
    }
}
