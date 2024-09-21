using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TitanTechTask.Domain.Books;
using TitanTechTask.Shared.Enums;

namespace TitanTechTask.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookManager _bookManager;

        public BookController(IBookManager bookManager)
        {
            _bookManager = bookManager;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookManager.GetAllBooks();
            return View(books);
        }

        /// <summary>
        /// Search for books by title, author, or ISBN
        /// </summary>
        /// <param name="title"></param>
        /// <param name="author"></param>
        /// <param name="isbn"></param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string title, string author, string isbn)
        {
            // Call the service layer to get the books based on search criteria
            var books = await _bookManager.SearchBooks(title, author, isbn);

            // Pass the books list to the view
            return View("Index", books);
        }

        [HttpGet]
        public async Task<IActionResult> Borrow(int bookId)
        {
            // Fetch book details using the bookId to display on the Borrow view
            var book = await _bookManager.GetBookByIdAsync(bookId);

            if (book == null || book.AvailabilityStatus != nameof(AvailabilityStatus.Available))
            {
                // Handle cases where the book is not available or doesn't exist
                return RedirectToAction("Index");
            }

            // Pass the book details to the Borrow view
            return View(book);
        }


        /// <summary>
        /// Borrow a book
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ConfirmBorrow(int bookId)
        {
            // Get the user ID from the claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                // Handle the case when the user ID is not found
                ModelState.AddModelError("", "User is not authenticated.");
                return RedirectToAction("Index"); // Redirect or return an error view
            }

            var userId = int.Parse(userIdClaim.Value); // Assuming the user ID is stored as an integer

            await _bookManager.BorrowBook(userId, bookId);

            return RedirectToAction("Index"); // Redirect to Index page after borrowing
        }

        /// <summary>
        /// Display the list of borrowed books for the current user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MyBorrowedBooks()
        {
            // Get the current user's ID from the HttpContext
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch borrowed books for the user
            var borrowedBooks = await _bookManager.GetBorrowedBooksByUserIdAsync(int.Parse(userId));

            return View(borrowedBooks);
        }

        /// <summary>
        /// Return a book
        /// </summary>
        /// <param name="bookId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ReturnBook(int bookId)
        {
            await _bookManager.ReturnBook(bookId);

            return RedirectToAction("Index");
        }
    }
}
