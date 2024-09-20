using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TitanTechTask.Data.Models;
using TitanTechTask.Domain.Account;

namespace TitanTechTask.Data.Providers
{
    internal class UserProvider : GenericProvider<User>, IUserProvider
    {
        private readonly IMapper _mapper;

        public UserProvider(ApplicationDbContext context, IMapper mapper) : base(context) // Pass the context to the base class constructor
        {
            _mapper = mapper;
        }

        public async Task<UserDomain> FindByEmailAsync(string userName)
        {
            UserDomain user = null;
            using (SqlConnection conn = _context.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE UserName = @UserName";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserName", userName);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    user = new UserDomain
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        PasswordHash = reader.GetString(3)
                        // ... map other fields
                    };
                }
            }
            return user;
        }

        public async Task<UserDomain> FindByIdAsync(string userid)
        {
            UserDomain user = null;
            using (SqlConnection conn = _context.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userid);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    user = new UserDomain
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        PasswordHash = reader.GetString(3)
                        // ... map other fields
                    };
                }
            }
            return user;
        }

        public async Task<IdentityResult> CreateUserAsync(UserDomain user)
        {
            using (SqlConnection connection = _context.GetConnection())
            {
                string query = @"INSERT INTO Users (UserName, NormalizedUserName, PasswordHash)
                         VALUES (@UserName, @NormalizedUserName, @PasswordHash)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@NormalizedUserName", user.NormalizedUserName);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);


                    var result = await command.ExecuteNonQueryAsync(); // Execute the SQL insert query

                    // Check if the insert was successful
                    if (result > 0)
                    {
                        return IdentityResult.Success; // Static method for successful result
                    }
                    else
                    {
                        // Return a failed IdentityResult with an error message
                        return IdentityResult.Failed(new IdentityError
                        {
                            Description = "Failed to insert user into the database."
                        });
                    }

                }
            }
        }

    }
}
