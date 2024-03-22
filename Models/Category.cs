using System.ComponentModel.DataAnnotations;

namespace SimpleProductOrder.Models
{
    public class Category 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
