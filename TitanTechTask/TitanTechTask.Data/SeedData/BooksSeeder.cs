using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanTechTask.Data.SeedData
{
    public class BooksSeeder : IDataSeeder
    {
        public void Seed(ApplicationDbContext _context)
        {
            using (SqlConnection connection = _context.GetConnection())
            {
                // Define the SQL query to check if books are already seeded
                string checkBooksExistQuery = "SELECT COUNT(1) FROM Books";

                using (SqlCommand checkCommand = new(checkBooksExistQuery, connection))
                {
                    // Execute the query and check if there are any records in the Books table
                    int bookCount = (int)checkCommand.ExecuteScalar();

                    // If no records exist, insert new books
                    if (bookCount == 0)
                    {
                        // Insert multiple books
                        string insertBooksQuery = @"
                        INSERT INTO Books (Title, Author, ISBN, Available) 
                        VALUES 
                        ('Book Title 1', 'Author 1', 'ISBN001', 1),
                        ('Book Title 2', 'Author 2', 'ISBN002', 1),
                        ('Book Title 3', 'Author 3', 'ISBN003', 1);
                    ";

                        using (SqlCommand insertCommand = new SqlCommand(insertBooksQuery, connection))
                        {
                            // Execute the insert command
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
