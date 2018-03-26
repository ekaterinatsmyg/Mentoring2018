using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AsyncTasks.Logger;
using AsyncTasks.Task3.DBModels;
using AsyncTasks.Task3.Interfaces;
using AsyncTasks.Task3.Mappers;
using AsyncTasks.Task3.Models;

namespace AsyncTasks.Task3.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Get all existing products.
        /// </summary>
        /// <returns>A list of esiting products.</returns>
        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync()
        {
            IEnumerable<ProductModel> products = null;
            try
            {
                products = await productRepository.GetAllProductsAsync();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogMessage(LogMessageType.Error,
                    $"{ex.Message} {Environment.NewLine} {ex.StackTrace}");
            }
            return products != null
                ? Mapper.Map<IEnumerable<ProductModel>, IEnumerable<ProductViewModel>>(products)
                : null;
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="productViewModel">A product thatshould be inserted.</param>
        /// <returns>An identifir=er of the inserted items.</returns>
        public async Task<int?> CreateProductAsync(ProductViewModel productViewModel)
        {
            if (productViewModel == null) return null;
            var product = Mapper.Map<ProductViewModel, ProductModel>(productViewModel);
            var id = await productRepository.CreateProductAsync(product);
            return id;
        }

        /// <summary>
        /// Get an existing product by its identifier.
        /// </summary>
        /// <param name="id">An identifier of the requested product.</param>
        /// <returns></returns>
        public async Task<ProductViewModel> GetProductAsync(int id)
        {
            var product = await productRepository.GetProductByIdAsync(id);

            return product != null ? Mapper.Map<ProductModel, ProductViewModel>(product) : null;
        }

        /// <summary>
        /// Update the product.
        /// </summary>
        /// <param name="productViewModel">Changedversion of the product.</param>
        /// <returns>Changed product.</returns>
        public async Task<ProductViewModel> EditProductAsync(ProductViewModel productViewModel)
        {
            var product =
                await productRepository.EditProductAsync(Mapper.Map<ProductViewModel, ProductModel>(productViewModel));

            return product != null ? Mapper.Map<ProductModel, ProductViewModel>(product) : null;
        }

        /// <summary>
        /// Delete the product.
        /// </summary>
        /// <param name="id"> The identifier of the requested prodct.</param>
        /// <returns>The identifier of the deleted product.</returns>
        public async Task<int?> DeleteAsync(int? id)
        {
            var deletedId = await productRepository.DeleteAsync(id);
            return deletedId;
        }

        /// <summary>
        /// Get all existing products i the busket.
        /// </summary>
        /// <returns>A list of esiting products.</returns>
        public IEnumerable<ProductViewModel> GetAllProductsInBasket()
        {
            IEnumerable<ProductModel> products = null;
            try
            {
                products = productRepository.GetAllProductsInBasket();
            }
            catch (Exception ex)
            {
                ApplicationLogger.LogMessage(LogMessageType.Error,
                    $"{ex.Message} {Environment.NewLine} {ex.StackTrace}");
            }
            return products != null
                ? Mapper.Map<IEnumerable<ProductModel>, IEnumerable<ProductViewModel>>(products)
                : null;
        }
    }
}