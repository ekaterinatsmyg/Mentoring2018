using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace AsyncTasks.Task3.DBModels
{
    public class DbModelContext : DbContext
    {
        public DbModelContext() : base("name=Shop")
        {
           Database.SetInitializer(new ShopDbInitializer());
        }
        public virtual IDbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<BasketModel>().HasKey(b => b.Id);
            //modelBuilder.Entity<BasketModel>().Property(b => b.Id)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ProductModel>().HasKey(b => b.Id);
            modelBuilder.Entity<ProductModel>().Property(b => b.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //modelBuilder.Entity<BasketModel>()
            //    .HasMany<ProductModel>(b => b.Products)
            //    .WithOptional(p => p.Basket)
            //    .HasForeignKey<int>(s => s.Id);

            //modelBuilder.Entity<ProductModel>()
            //    .HasOptional<BasketModel>(p => p.Basket)
            //    .WithMany(b => b.Products)
            //    .HasForeignKey<int?>(b => b.BasketId);
        }
    }
}