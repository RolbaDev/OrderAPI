using System.ComponentModel.DataAnnotations;

namespace SimpleProductOrder.Models
{
    public class Review
    {

         
        public int Id { get; set; }

         
        public string Title { get; set; }

         
        public string Content { get; set; }

         
        public Customer Author { get; set; }

         
        public Product Product { get; set; }
    }
}
