using Postgrest.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProductOrder.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Customer Customer { get; set; }
    }
}
