using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SaleRepository : ISaleRepository
{
    private readonly string _connectionString;

    public SaleRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Sale>> GetAllSale()
    {
        var sales = new List<Sale>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("SELECT * FROM Sale", connection);
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    sales.Add(new Sale
                    {
                        SaleId = reader.GetInt32(0),
                        ClientId = reader.GetInt32(1),
                        ProductId = reader.GetInt32(2),
                        Quantity = reader.GetInt32(3),
                        TotalPrice = reader.GetDecimal(4),
                        SaleDate = reader.GetDateTime(5)
                    });
                }
            }
        }
        return sale;
    }

    public async Task<Sale> GetSaleById(int id)
    {
        Sale sale = null;
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("SELECT * FROM Sale WHERE SaleId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    sale = new Sale
                    {
                        SaleId = reader.GetInt32(0),
                        ClientId = reader.GetInt32(1),
                        ProductId = reader.GetInt32(2),
                        Quantity = reader.GetInt32(3),
                        TotalPrice = reader.GetDecimal(4),
                        SaleDate = reader.GetDateTime(5)
                    };
                }
            }
        }
        return sale;
    }

    public async Task AddSale(Sale sale)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("INSERT INTO Sales (ClientId, ProductId, Quantity, TotalPrice, SaleDate) VALUES (@ClientId, @ProductId, @Quantity, @TotalPrice, @SaleDate)", connection);
            command.Parameters.AddWithValue("@ClientId", sale.ClientId);
            command.Parameters.AddWithValue("@ProductId", sale.ProductId);
            command.Parameters.AddWithValue("@Quantity", sale.Quantity);
            command.Parameters.AddWithValue("@TotalPrice", sale.TotalPrice);
            command.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
            await command.ExecuteNonQueryAsync();
            sale.SaleId = (int)command.LastInsertedId;
        }
    }

    public async Task UpdateSale(Sale sale)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("UPDATE Sale SET ClientId = @ClientId, ProductId = @ProductId, Quantity = @Quantity, TotalPrice = @TotalPrice, SaleDate = @SaleDate WHERE SaleId = @id", connection);
            command.Parameters.AddWithValue("@ClientId", sale.ClientId);
            command.Parameters.AddWithValue("@ProductId", sale.ProductId);
            command.Parameters.AddWithValue("@Quantity", sale.Quantity);
            command.Parameters.AddWithValue("@TotalPrice", sale.TotalPrice);
            command.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
            command.Parameters.AddWithValue("@id", sale.SaleId);
            await command.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteSale(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("DELETE FROM Sale WHERE SaleId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}
