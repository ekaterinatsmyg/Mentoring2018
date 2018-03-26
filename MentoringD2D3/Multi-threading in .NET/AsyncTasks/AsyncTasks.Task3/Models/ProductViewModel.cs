using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AsyncTasks.Task3.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cost is requared")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "Valid decimal number")]
        public decimal Cost { get; set; }

        [Display(Name = "In Basket")]
        public bool IsInBasket { get; set; }
    }
}