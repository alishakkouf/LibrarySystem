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
    internal class RoleProvider : GenericProvider<User>, IRoleProvider
    {
        private readonly IMapper _mapper;

        public RoleProvider(ApplicationDbContext context, IMapper mapper) : base(context) // Pass the context to the base class constructor
        {
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            // Insert the role into the database
            // You can use ADO.NET or any other method to persist the role in your database
            var query = @"INSERT INTO Roles (Id, Name, NormalizedName) VALUES (@Id, @Name, @NormalizedName)";
            using (var connection = _context.GetConnection())
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", role.Id);
                command.Parameters.AddWithValue("@Name", role.Name);
                command.Parameters.AddWithValue("@NormalizedName", role.NormalizedName);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<IdentityRole?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityRole?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(IdentityRole role, string? roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
