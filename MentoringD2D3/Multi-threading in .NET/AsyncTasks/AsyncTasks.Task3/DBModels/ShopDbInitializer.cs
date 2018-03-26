using System.Collections.Generic;
using System.Data.Entity;

namespace AsyncTasks.Task3.DBModels
{
    public class ShopDbInitializer: DropCreateDatabaseIfModelChanges<DbModelContext>
    {
        protected override void Seed(DbModelContext context)
        {
            IList<ProductModel> products = new List<ProductModel>()
            {
                new ProductModel() {Name = "Milk", Cost = 1.2m, IsInBasket = false},
                new ProductModel() {Name = "Bread", Cost = 0.7m, IsInBasket = false},
                new ProductModel() {Name = "Sugar", Cost = 0.5m, IsInBasket = false}
            };
            foreach (ProductModel product in products)
            {
                context.Set<ProductModel>().Add(product);
            }

            base.Seed(context);
        }
    }
}