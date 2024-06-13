using Product.Model;
using MySql.Data.MySqlClient;
using Product.src.Repository.Interface;  


namespace Product.src.Repositories.Interface
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var products = new List<ProductModel>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM product WHERE deleted = 0", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new ProductModel
                        {
                            Id = reader["id"].ToString(),
                            Name = reader["name"].ToString(),
                            Description = reader["dest"].ToString(),
                            Quantity = Convert.ToInt32(reader["quantity"]),
                            Price = Convert.ToDecimal(reader["price"]),
                            CreatedAt = Convert.ToDateTime(reader["created_at"]),
                            UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                            Deleted = Convert.ToBoolean(reader["deleted"])
                        });
                    }
                }
            }
            return products;
        }

        public async Task<ProductModel> GetProductByIdAsync(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT * FROM product WHERE id = @Id AND deleted = 0", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new ProductModel
                        {
                            Id = reader["id"].ToString(),
                            Name = reader["name"].ToString(),
                            Description = reader["dest"].ToString(),
                            Quantity = Convert.ToInt32(reader["quantity"]),
                            Price = Convert.ToDecimal(reader["price"]),
                            CreatedAt = Convert.ToDateTime(reader["created_at"]),
                            UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                            Deleted = Convert.ToBoolean(reader["deleted"])
                        };
                    }
                }
            }
            return null;
        }

        public async Task<bool> CreateProductAsync(ProductModel product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand(@"INSERT INTO product (id, name, dest, quantity, price, created_at, updated_at, deleted) 
                                                VALUES (@Id, @Name, @Description, @Quantity, @Price, @CreatedAt, @UpdatedAt, @Deleted)", connection);
                command.Parameters.AddWithValue("@Id", Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@Deleted", false);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> UpdateProductAsync(ProductModel product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand(@"UPDATE product 
                                                SET name = @Name, dest = @Description, quantity = @Quantity, price = @Price, updated_at = @UpdatedAt 
                                                WHERE id = @Id AND deleted = 0", connection);
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Quantity", product.Quantity);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("UPDATE product SET deleted = 1, updated_at = @UpdatedAt WHERE id = @Id AND deleted = 0", connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);  // Atualiza a data de atualização ao deletar

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> ProductExistsAsync(string productId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new MySqlCommand("SELECT COUNT(1) FROM product WHERE id = @ProductId AND deleted = 0", connection);
                command.Parameters.AddWithValue("@ProductId", productId);

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result) > 0;
            }
        }
    }

    
}
