using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitanTechTask.Domain.Books;

namespace TitanTechTask.Manager.BooksManager
{
    public class BookManager : IBookManager
    {
        private readonly IBookProvider _bookProvider;

        public BookManager(IBookProvider bookProvider)
        {
            _bookProvider = bookProvider;
        }

        public async Task<List<BookDomain>> SearchBooks(string? title, string? author, string? isbn)
        {
            if (title == null && author == null && isbn == null)
            {
                return await _bookProvider.GetAllBooks();
            }
                return await _bookProvider.SearchBooks(title, author, isbn);
        }

        public async Task BorrowBook(int userId, int bookId)
        {
            var book = await _bookProvider.GetBook(bookId);

            await _bookProvider.BorrowBook(userId, bookId);
        }

        public async Task ReturnBook(int bookId)
        {
           await _bookProvider.ReturnBook(bookId);
        }

        public async Task<List<BookDomain>> GetAllBooks()
        {
           return await _bookProvider.GetAllBooks();
        }

        public async Task<List<BookDomain>> GetBorrowedBooksByUserIdAsync(int userId)
        {
            return await _bookProvider.GetBorrowedBooksByUserIdAsync(userId);
        }

        public async Task<BookDomain> GetBookByIdAsync(int bookId)
        {
            return await _bookProvider.GetBook(bookId);
        }
    }
}
