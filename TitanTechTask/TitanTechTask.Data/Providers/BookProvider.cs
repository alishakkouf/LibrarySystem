using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using TitanTechTask.Data.Models;
using TitanTechTask.Domain.Books;

namespace TitanTechTask.Data.Providers
{
    internal class BookProvider : GenericProvider<Book>, IBookProvider
    {
        private readonly IMapper _mapper;

        public BookProvider(ApplicationDbContext context, IMapper mapper) : base(context) // Pass the context to the base class constructor
        {
            _mapper = mapper;
        }

        public async Task<BookDomain> GetBook(int bookId)
        {
            using (SqlConnection connection = _context.GetConnection())
            {
                var query = "SELECT BookId, Title, Author, ISBN, Available FROM Books WHERE BookId = @BookId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookId", bookId); // Parameterized query to prevent SQL injection

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var book = new Book
                            {
                                BookId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                ISBN = reader.GetString(3),
                                Available = reader.GetBoolean(4)
                            };

                            return _mapper.Map<BookDomain>(book); // Map and return the book
                        }
                        else
                        {
                            // Book not found, handle appropriately
                            return null; // You could also throw an exception if preferred
                        }
                    }
                }
            }
        }


        public async Task<List<BookDomain>> SearchBooks(string title, string author, string isbn)
        {
            var books = new List<Book>();

            using (SqlConnection connection = _context.GetConnection())
            {
                // Start with the base query
                var query = "SELECT BookId, Title, Author, ISBN, Available FROM Books WHERE 1=1";

                // Dynamically append conditions based on provided search parameters
                if (!string.IsNullOrEmpty(title))
                {
                    query += " AND Title LIKE @Title";
                }
                if (!string.IsNullOrEmpty(author))
                {
                    query += " AND Author LIKE @Author";
                }
                if (!string.IsNullOrEmpty(isbn))
                {
                    query += " AND ISBN LIKE @ISBN";
                }

                using (SqlCommand command = new(query, connection))
                {
                    // Add parameters only if values were provided
                    if (!string.IsNullOrEmpty(title))
                    {
                        command.Parameters.AddWithValue("@Title", "%" + title + "%");
                    }
                    if (!string.IsNullOrEmpty(author))
                    {
                        command.Parameters.AddWithValue("@Author", "%" + author + "%");
                    }
                    if (!string.IsNullOrEmpty(isbn))
                    {
                        command.Parameters.AddWithValue("@ISBN", "%" + isbn + "%");
                    }

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            books.Add(new Book
                            {
                                BookId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                ISBN = reader.GetString(3),
                                Available = reader.GetBoolean(4)
                            });
                        }
                    }
                }
            }

            // Assuming you have AutoMapper, if not you can map manually
            return _mapper.Map<List<BookDomain>>(books);
        }


        public async Task BorrowBook(int userId, int bookId)
        {
            using (SqlConnection connection = _context.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Insert a new borrowing record
                        var insertQuery = "INSERT INTO Borrowings (UserId, BookId, BorrowedDate) " +
                            "VALUES (@UserId, @BookId, @BorrowedDate)";

                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction))
                        {
                            insertCommand.Parameters.AddWithValue("@UserId", userId);
                            insertCommand.Parameters.AddWithValue("@BookId", bookId);
                            insertCommand.Parameters.AddWithValue("@BorrowedDate", DateTime.UtcNow);

                            await insertCommand.ExecuteNonQueryAsync();
                        }

                        // Update the availability status in the Books table
                        var updateQuery = "UPDATE Books SET Available = 0 WHERE BookId = @BookId";
                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection, transaction))
                        {
                            updateCommand.Parameters.AddWithValue("@BookId", bookId);

                            await updateCommand.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction in case of error
                        transaction.Rollback();
                        throw; // Rethrow the exception to handle it elsewhere if needed
                    }
                }
            }
        }


        public async Task ReturnBook(int bookId)
        {
            using (SqlConnection connection = _context.GetConnection())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update the returned date in the Borrowings table
                        var updateBorrowingQuery = @" UPDATE Borrowings SET ReturnedDate = @ReturnedDate 
                                                WHERE BookId = @BookId AND ReturnedDate IS NULL"; // Ensure we only update if it hasn't been returned

                        using (SqlCommand updateCommand = new SqlCommand(updateBorrowingQuery, connection, transaction))
                        {
                            updateCommand.Parameters.AddWithValue("@BookId", bookId);
                            updateCommand.Parameters.AddWithValue("@ReturnedDate", DateTime.UtcNow);

                            await updateCommand.ExecuteNonQueryAsync();
                        }

                        // Update the availability status in the Books table
                        var updateBookQuery = "UPDATE Books SET Available = 1 WHERE BookId = @BookId";
                        using (SqlCommand updateBookCommand = new SqlCommand(updateBookQuery, connection, transaction))
                        {
                            updateBookCommand.Parameters.AddWithValue("@BookId", bookId);

                            await updateBookCommand.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Rollback the transaction in case of error
                        transaction.Rollback();
                        throw; // Rethrow the exception to handle it elsewhere if needed
                    }
                }
            }
        }


        public async Task<List<BookDomain>> GetAllBooks()
        {
            var books = new List<Book>();

            // Using ADO.NET to connect to the database
            using (SqlConnection connection = _context.GetConnection())
            {
                // SQL query to get all books
                string query = "SELECT BookId, Title, Author, ISBN, Available FROM Books";

                // Create the SQL command
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    // Execute the command and read the data
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            // Mapping the database record to the Book model
                            var book = new Book
                            {
                                BookId = reader.GetInt32(0),    // BookId
                                Title = reader.GetString(1),     // Title
                                Author = reader.GetString(2),    // Author
                                ISBN = reader.GetString(3),      // ISBN
                                Available = reader.GetBoolean(4) // Available (true/false)
                            };

                            books.Add(book);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle or log the exception (e.g., connection failure)
                    throw new Exception("Error fetching books: " + ex.Message);
                }
            }

            return _mapper.Map<List<BookDomain>>(books);
        }

        public async Task<List<BookDomain>> GetBorrowedBooksByUserIdAsync(int userId)
        {
            var borrowedBooks = new List<BookDomain>();

            using (SqlConnection connection = _context.GetConnection())
            {
                var query = @"SELECT Books.BookId, Books.Title, Books.Author, Borrowings.BorrowedDate 
                      FROM Books 
                      JOIN Borrowings ON Books.BookId = Borrowings.BookId 
                      WHERE Borrowings.UserId = @UserId AND Borrowings.ReturnedDate IS NULL";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            borrowedBooks.Add(new BookDomain
                            {
                                BookId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Author = reader.GetString(2),
                                BorrowedDate = reader.GetDateTime(3)
                            });
                        }
                    }
                }
            }

            return borrowedBooks;
        }


    }
}
