using Client.Models;
using Client.src.Repositories.Interface;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<ClientModel>> GetClientsAsync()
        {
            var clients = new List<ClientModel>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM client WHERE deleted = false", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        clients.Add(new ClientModel
                        {
                            Id = reader["id"].ToString(),
                            Name = reader["name"].ToString(),
                            Surname = reader["surname"].ToString(),
                            Email = reader["email"].ToString(),
                            Birthdate = reader["birthdate"] != DBNull.Value ? (DateTime)reader["birthdate"] : null,
                            CreatedAt = (DateTime)reader["created_at"],
                            UpdatedAt = (DateTime)reader["updated_at"],
                            Deleted = (bool)reader["deleted"]
                        });
                    }
                }
            }
            return clients;
        }

        public async Task<ClientModel> GetClientByIdAsync(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM client WHERE id = @Id AND deleted = false", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ClientModel
                        {
                            Id = reader["id"].ToString(),
                            Name = reader["name"].ToString(),
                            Surname = reader["surname"].ToString(),
                            Email = reader["email"].ToString(),
                            Birthdate = reader["birthdate"] != DBNull.Value ? (DateTime)reader["birthdate"] : null,
                            CreatedAt = (DateTime)reader["created_at"],
                            UpdatedAt = (DateTime)reader["updated_at"],
                            Deleted = (bool)reader["deleted"]
                        };
                    }
                }
            }
            return null;
        }

        public async Task<bool> CreateClientAsync(ClientModel client)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand(@"INSERT INTO client (id, name, surname, email, birthdate, created_at, updated_at, deleted) 
                                                VALUES (@Id, @Name, @Surname, @Email, @Birthdate, @CreatedAt, @UpdatedAt, @Deleted)", connection);
                command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Surname", client.Surname);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@Birthdate", client.Birthdate);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@Deleted", false);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateClientAsync(ClientModel client)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand(@"UPDATE client 
                                                SET name = @Name, surname = @Surname, email = @Email, birthdate = @Birthdate, 
                                                updated_at = @UpdatedAt 
                                                WHERE id = @Id", connection);
                command.Parameters.AddWithValue("@Id", client.Id);
                command.Parameters.AddWithValue("@Name", client.Name);
                command.Parameters.AddWithValue("@Surname", client.Surname);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@Birthdate", client.Birthdate);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteClientAsync(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("UPDATE client SET deleted = true, updated_at = @UpdatedAt WHERE id = @Id AND deleted = false", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);  // Atualiza a data de atualização ao deletar

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> ClientExistsAsync(ClientModel client)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT COUNT(1) FROM client WHERE email = @Email AND deleted = false", connection);
                command.Parameters.AddWithValue("@Email", client.Email);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
        }

    }
}
