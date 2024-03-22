using System.ComponentModel.DataAnnotations;

namespace SimpleProductOrder.Models
{
    public class Order
    {
         
        public int Id { get; set; }

         
        public DateTime Order_date { get; set; }

         
        public Customer Customer { get; set; }

         
        public ICollection<OrderProduct> OrderProducts { get; set; }
}
}
