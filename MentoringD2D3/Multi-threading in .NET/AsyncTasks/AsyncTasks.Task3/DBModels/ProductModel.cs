using System.ComponentModel.DataAnnotations.Schema;

namespace AsyncTasks.Task3.DBModels
{
    [Table("Product")]
    public  class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public bool IsInBasket { get; set; }
    }
}