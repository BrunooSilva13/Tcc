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

    public async Task<IEnumerable<Sale>> GetAllSalesAsync()
    {
        var sales = new List<Sale>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("SELECT * FROM Sales", connection);
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    sales.Add(new Sale
                    {
                        SaleId = reader.GetInt32("SaleId"),
                        ClientId = reader.GetInt32("ClientId"),
                        TotalPrice = reader.GetDecimal("TotalPrice"),
                        SaleDate = reader.GetDateTime("SaleDate")
                    });
                }
            }
        }
        return sales;
    }

    public async Task<Sale> GetSaleByIdAsync(int id)
    {
        Sale sale = null;
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("SELECT * FROM Sales WHERE SaleId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    sale = new Sale
                    {
                        SaleId = reader.GetInt32("SaleId"),
                        ClientId = reader.GetInt32("ClientId"),
                        TotalPrice = reader.GetDecimal("TotalPrice"),
                        SaleDate = reader.GetDateTime("SaleDate")
                    };
                }
            }
        }
        return sale;
    }

    public async Task<int> AddSaleAsync(Sale sale)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("INSERT INTO Sales (ClientId, TotalPrice, SaleDate) VALUES (@ClientId, @TotalPrice, @SaleDate)", connection);
            command.Parameters.AddWithValue("@ClientId", sale.ClientId);
            command.Parameters.AddWithValue("@TotalPrice", sale.TotalPrice);
            command.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
            await command.ExecuteNonQueryAsync();
            sale.SaleId = (int)command.LastInsertedId;
        }
        return sale.SaleId;
    }

    public async Task UpdateSaleAsync(Sale sale)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("UPDATE Sales SET ClientId = @ClientId, TotalPrice = @TotalPrice, SaleDate = @SaleDate WHERE SaleId = @id", connection);
            command.Parameters.AddWithValue("@ClientId", sale.ClientId);
            command.Parameters.AddWithValue("@TotalPrice", sale.TotalPrice);
            command.Parameters.AddWithValue("@SaleDate", sale.SaleDate);
            command.Parameters.AddWithValue("@id", sale.SaleId);
            await command.ExecuteNonQueryAsync();
        }
    }
    public async Task DeleteSaleAsync(int id)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new MySqlCommand("DELETE FROM Sales WHERE SaleId = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            await command.ExecuteNonQueryAsync();
        }
    }
}