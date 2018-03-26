using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AsyncTasks.Logger;
using AsyncTasks.Task3.Interfaces;
using AsyncTasks.Task3.Models;

namespace AsyncTasks.Task3.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private static BasketViewModel basket = new BasketViewModel() {Products = new List<ProductViewModel>()};

        public ProductController(IProductService productService)
        {
            this.productService = productService;
            InitializeBasket();
        }

        /// <summary>
        /// Displays a list of the products and a current state of the basket.
        /// </summary>
        /// <returns>A fiew of the product list.</returns>
        public async Task<ActionResult> Index()
        {
            var products = await productService.GetAllProductsAsync();
            ViewBag.Basket = basket;

            return View(products);
        }

        /// <summary>
        /// Dispays a view for creation of a product.
        /// </summary>
        /// <returns> A view for creation of a product.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Redirect to a view that diaplys a view for redaction of the seleted product.
        /// </summary>
        /// <param name="id">An identifier of the product.</param>
        /// <returns>The redaction of the product view.</returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return HttpNotFound();
            var productViewModel = await productService.GetProductAsync(id.Value);

            if (productViewModel == null)
                return HttpNotFound();

            return View(productViewModel);
        }

        /// <summary>
        /// Delete a product and update the basket appropriately.
        /// </summary>
        /// <param name="id">The identifier of a removable product.</param>
        /// <returns>Updated list of the products.</returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var product = await productService.GetProductAsync(id.Value);
                basket.Products.Remove(product);
                basket.TotalPrice -= product.Cost;
            }
            var deletedId = await productService.DeleteAsync(id);
            if (deletedId != null)
                return RedirectToAction("Index", "Product");

            ApplicationLogger.LogMessage(LogMessageType.Warn, "Product was not deleted.");
            return HttpNotFound();
        }

        /// <summary>
        /// Aend a request for product creation.
        /// </summary>
        /// <param name="model">Product that should be inserted.</param>
        /// <returns>>Updated list of the products.</returns>
        [HttpPost]
        public async Task<ActionResult> СreateProduct(ProductViewModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var id = await productService.CreateProductAsync(model);
            if (id == null)
            {
                ApplicationLogger.LogMessage(LogMessageType.Warn, "Product was not created.");
            }
            return RedirectToAction("Index", "Product");
        }

        /// <summary>
        /// Send a request for updating a product.
        /// </summary>
        /// <param name="product">Editrd product.</param>
        /// <param name="originalCost">A cost of a product before redaction.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> EditProduct(ProductViewModel product, decimal originalCost)
        {
            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (product.Cost != originalCost && product.IsInBasket)
            {
                var difference = originalCost - product.Cost;
                basket.TotalPrice -= difference;
            }

            var updatedProduct = await productService.EditProductAsync(product);
            if (updatedProduct == null)
            {
                ApplicationLogger.LogMessage(LogMessageType.Warn, "Product was not edited.");
            }

            return RedirectToAction("Index", "Product");
        }

        /// <summary>
        /// Send a request for adding a product to the busket.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>The updated a list of products.</returns>
        public async Task<ActionResult> AddToBasket(int id)
        {
            var product = await productService.GetProductAsync(id);

            product.IsInBasket = true;
            var updatedProduct = await productService.EditProductAsync(product);
            basket.Products.Add(updatedProduct);
            basket.TotalPrice += product.Cost;

            if (updatedProduct == null)
            {
                ApplicationLogger.LogMessage(LogMessageType.Error, "The product was not added to basket.");
            }
            return RedirectToAction("Index", "Product");
        }

        /// <summary>
        /// Initialie basket.
        /// </summary>
        private void InitializeBasket()
        {
            var products = productService.GetAllProductsInBasket();

            if (products == null) return;

            basket.Products.AddRange(products);
            foreach (var product in basket.Products)
            {
                basket.TotalPrice += product.Cost;
            }
        }
    }
}