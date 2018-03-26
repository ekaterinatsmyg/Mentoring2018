using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AsyncTasks.Task3.Models
{
    public class BasketViewModel
    {
        public int BasketId { get; set; }

        [Display(Name = "Total")]
        public decimal TotalPrice { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}