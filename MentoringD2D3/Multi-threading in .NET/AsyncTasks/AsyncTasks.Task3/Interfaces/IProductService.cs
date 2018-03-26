using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncTasks.Task3.Models;

namespace AsyncTasks.Task3.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProductsAsync();

        Task<int?> CreateProductAsync(ProductViewModel productViewModel);

        Task<ProductViewModel> GetProductAsync(int id);

        Task<ProductViewModel> EditProductAsync(ProductViewModel productViewModel);

        Task<int?> DeleteAsync(int? id);

        IEnumerable<ProductViewModel> GetAllProductsInBasket();
    }
}
