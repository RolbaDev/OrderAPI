using SimpleProductOrder.Data;
using SimpleProductOrder.Dto;
using SimpleProductOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleProductOrder.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateCustomer(Customer customer)
        {
            _context.Add(customer);
            return Save();
        }

        public bool DeleteCustomer(Customer customer)
        {
            _context.Remove(customer);
            return Save();
        }

        public Customer GetCustomer(int customerId)
        {
            return _context.Customers.Where(o => o.Id == customerId).FirstOrDefault();
        }

        public ICollection<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public bool CustomerExists(int customerId)
        {
            return _context.Customers.Any(o => o.Id == customerId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCustomer(Customer customer)
        {
            _context.Update(customer);
            return Save();
        }
    }
}
