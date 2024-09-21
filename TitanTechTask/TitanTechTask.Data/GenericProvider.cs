using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitanTechTask.Domain;

namespace TitanTechTask.Data
{
    internal class GenericProvider<TEntity> where TEntity : class, IAuditedEntity, new()
    {
        protected ApplicationDbContext _context;

        public GenericProvider(ApplicationDbContext context)
        {
            _context = context;
        }

        // ActiveDbSet equivalent in ADO.NET (returns non-deleted records)
        protected virtual async Task<List<TEntity>> GetActiveEntitiesAsync()
        {
            var entities = new List<TEntity>();
            var query = $"SELECT * FROM {typeof(TEntity).Name} WHERE IsDeleted = 0";

            using (var connection = _context.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var entity = MapReaderToEntity(reader);
                            entities.Add(entity);
                        }
                    }
                }
            }

            return entities;
        }

        // Soft delete for a list of entities
        protected async Task SoftDeleteListAsync(List<TEntity> list)
        {
            if (list.Any())
            {
                using (var connection = _context.GetConnection())
                {
                    await connection.OpenAsync();

                    foreach (var entity in list)
                    {
                        var query = $"UPDATE {typeof(TEntity).Name} SET IsDeleted = 1 WHERE Id = @Id";

                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", GetEntityId(entity));
                            await command.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
        }

        protected async Task SoftDeleteAsync(TEntity entity)
        {
            var query = $"UPDATE {typeof(TEntity).Name} SET IsDeleted = 1 WHERE Id = @Id";

            using (var connection = _context.GetConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", GetEntityId(entity));
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Method to map SqlDataReader to TEntity
        private TEntity MapReaderToEntity(SqlDataReader reader)
        {
            var entity = new TEntity();
            // Map each field to the entity (you'll need to implement this based on your model)
            // Example:
            // entity.Id = reader.GetInt32(reader.GetOrdinal("Id"));
            // entity.IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted"));
            return entity;
        }

        // Method to get the Id of the entity (assumes TEntity has an Id property)
        private int GetEntityId(TEntity entity)
        {
            var propertyInfo = typeof(TEntity).GetProperty("Id");
            return (int)propertyInfo.GetValue(entity);
        }
    }
}