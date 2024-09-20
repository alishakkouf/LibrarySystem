using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanTechTask.Data.Migration
{
    public static class MigrationsExecution
    {
        public static void RunMigrations(string connectionString)
        {
            var migrations = new List<(string MigrationName, string SqlScript)>
            {
                ("InitialCreate", @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Migrations' AND xtype='U')
                                    CREATE TABLE Migrations (
                                        MigrationId INT IDENTITY PRIMARY KEY,
                                        MigrationName NVARCHAR(255) NOT NULL,
                                        AppliedAt DATETIME NOT NULL
                                    );"
                ),

                ("CreateBooksTable", @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Books' AND xtype='U')
                                       CREATE TABLE Books (
                                           BookId INT PRIMARY KEY IDENTITY,
                                           Title NVARCHAR(255) NOT NULL,
                                           Author NVARCHAR(255) NOT NULL,
                                           ISBN NVARCHAR(13) UNIQUE NOT NULL,
                                           Available BIT NOT NULL
                                       );"
                ),

("CreateUsersAndBorrowingsTables", @"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                                    CREATE TABLE Users (
                                        UserId INT PRIMARY KEY IDENTITY,
                                        UserName NVARCHAR(256) NOT NULL,
                                        NormalizedUserName NVARCHAR(256) NOT NULL UNIQUE,
                                        PasswordHash NVARCHAR(MAX) NOT NULL
                                    );

                                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Borrowings' AND xtype='U')
                                    CREATE TABLE Borrowings (
                                        BorrowingId INT PRIMARY KEY IDENTITY,
                                        BookId INT FOREIGN KEY REFERENCES Books(BookId),
                                        UserId INT FOREIGN KEY REFERENCES Users(UserId),
                                        BorrowedDate DATETIME NOT NULL,
                                        ReturnedDate DATETIME NULL
                                    );"
)

            };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var migration in migrations)
                {
                    // Ensure the Migrations table is created first
                    var createMigrationsTable = migrations.First(m => m.MigrationName == "InitialCreate");

                    using (SqlCommand createCommand = new SqlCommand(createMigrationsTable.SqlScript, connection))
                    {
                        createCommand.ExecuteNonQuery();
                    }

                    // Check if migration has already been applied
                    string checkMigrationQuery = @"SELECT COUNT(*) FROM Migrations WHERE MigrationName =
                                                 @MigrationName";

                    using (SqlCommand checkCommand = new SqlCommand(checkMigrationQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@MigrationName", migration.MigrationName);

                        int migrationCount = (int)checkCommand.ExecuteScalar();

                        if (migrationCount == 0) // Migration not applied yet
                        {
                            // Execute migration SQL script
                            using (SqlCommand command = new SqlCommand(migration.SqlScript, connection))
                            {
                                command.ExecuteNonQuery();
                            }

                            // Mark the migration as applied
                            string insertMigrationQuery = @"INSERT INTO Migrations (MigrationName, AppliedAt)
                                                            VALUES (@MigrationName, @AppliedAt)";

                            using (SqlCommand insertCommand = new SqlCommand(insertMigrationQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@MigrationName", migration.MigrationName);
                                insertCommand.Parameters.AddWithValue("@AppliedAt", DateTime.Now);
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

    }
}
