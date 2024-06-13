using System.Collections.Generic;
using System.Threading.Tasks;
using SaleAPI.Models;

public interface ISaleRepository
{
    Task<IEnumerable<Sale>> GetAllSalesAsync();
    Task<Sale> GetSaleByIdAsync(int id);
    Task<int> AddSaleAsync(Sale sale);
    Task UpdateSaleAsync(Sale sale);
    Task DeleteSaleAsync(int id);
}
