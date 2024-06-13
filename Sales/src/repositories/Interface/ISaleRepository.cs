using System.Collections.Generic;
using System.Threading.Tasks;
using SaleAPI.Models;

public interface ISaleRepository
{
    Task<IEnumerable<Sale>> GetAllSaleAsync();
    Task<Sale> GetSaleByIdAsync(int id);
    Task<int> AddSaleAsync(Sale sale);
}
