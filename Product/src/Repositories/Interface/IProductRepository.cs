using System.Collections.Generic;
using System.Threading.Tasks;
using Product.Model;  

namespace Product.src.Repository.Interface
{
    public interface IProductRepository
    {
        Task<List<ProductModel>> GetAllProductsAsync();
        Task<ProductModel> GetProductByIdAsync(string id);
        Task<bool> CreateProductAsync(ProductModel product);
        Task<bool> UpdateProductAsync(ProductModel product);
        Task<bool> DeleteProductAsync(string id);
        Task<bool> ProductExistsAsync(string productId);
    }
}
