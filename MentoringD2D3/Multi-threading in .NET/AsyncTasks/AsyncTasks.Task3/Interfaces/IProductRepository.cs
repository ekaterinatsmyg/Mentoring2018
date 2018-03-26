using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncTasks.Task3.DBModels;

namespace AsyncTasks.Task3.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();

        Task<ProductModel> GetProductByIdAsync(int id);

        Task<int?> CreateProductAsync(ProductModel product);

        Task<ProductModel> EditProductAsync(ProductModel product);

        Task<int?> DeleteAsync(int? id);

        IEnumerable<ProductModel> GetAllProductsInBasket();

    }
}