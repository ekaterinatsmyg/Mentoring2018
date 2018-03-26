using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AsyncTasks.Task3.DBModels;
using AsyncTasks.Task3.Interfaces;

namespace AsyncTasks.Task3.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext dbContext;

        public ProductRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets all existing in DB products.
        /// </summary>
        /// <returns>A list of products.</returns>
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await dbContext.Set<ProductModel>().ToListAsync();
        }

        /// <summary>
        /// Get a product by id.
        /// </summary>
        /// <param name="id">The id of the requested product.</param>
        /// <returns>A product with the specified id.</returns>
        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            ProductModel product = await dbContext.Set<ProductModel>().FindAsync(id);
            return product;
        }

        /// <summary>
        /// Insert a new product to a storage.
        /// </summary>
        /// <param name="product">Adding product.</param>
        /// <returns>The id of the created product.</returns>
        public async Task<int?> CreateProductAsync(ProductModel product)
        {
            if (product != null)
            {
                product.IsInBasket = false;
                dbContext.Set<ProductModel>().Add(product);
                await dbContext.SaveChangesAsync();
            }
            return product.Id;
        }

        /// <summary>
        /// Edit a product entity.
        /// </summary>
        /// <param name="product">The editing product.</param>
        /// <returns>Edited product.</returns>
        public async Task<ProductModel> EditProductAsync(ProductModel product)
        {
            var updatedProduct = dbContext.Entry<ProductModel>
            (
                dbContext.Set<ProductModel>().Find(product.Id)
            );
            if (updatedProduct == null)
            {
                return null;
            }
            updatedProduct.State = EntityState.Modified;
            updatedProduct.Entity.Cost = product.Cost;
            updatedProduct.Entity.IsInBasket = product.IsInBasket;
            updatedProduct.Entity.Name = product.Name;

            await dbContext.SaveChangesAsync();

            return product;
        }

        /// <summary>
        /// Delete a product from DB.
        /// </summary>
        /// <param name="id">The identifier of a product.</param>
        /// <returns>The identifier of the removed product.</returns>
        public async Task<int?> DeleteAsync(int? id)
        {
            if (id != null)
            {
                ProductModel product = await dbContext.Set<ProductModel>().FindAsync(id.Value);
                if (product != null) dbContext.Set<ProductModel>().Remove(product);
                id = await dbContext.SaveChangesAsync();
            }
            return id;
        }

        /// <summary>
        /// Get all products in the basket.
        /// </summary>
        /// <returns>Products that are in the basket.</returns>
        public IEnumerable<ProductModel> GetAllProductsInBasket()
        {
            return dbContext.Set<ProductModel>().Where(p => p.IsInBasket);
        }
    }
}