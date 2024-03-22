using System.ComponentModel.DataAnnotations;

namespace SimpleProductOrder.Models
{
    public class Customer
    {
        public int Id { get; set; }


        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
