using System.ComponentModel.DataAnnotations;

namespace SimpleProductOrder.Models
{
    public class Product
    {
         
        public int Id { get; set; }

         
        public string Name { get; set; }


         
        public double Price { get; set; }

         
        public double Weight { get; set; }

         
        public string Description { get; set; }

         
        public int Qty { get; set; }

         
        public ICollection<OrderProduct> OrderProducts { get; set; }

       
        public Category Category { get; set; }
    }
}
