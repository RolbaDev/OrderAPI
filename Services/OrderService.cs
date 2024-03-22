using Microsoft.EntityFrameworkCore;
using SimpleProductOrder.Data;
using SimpleProductOrder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleProductOrder.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateOrder(Order order)
        {        
            _context.Add(order);
            return Save();
        }

        public bool CreateOrderProduct(OrderProduct orderProduct)
        {
            _context.Add(orderProduct);
            return Save();
        }

        public bool OrderExists(int id)
        {
            return _context.Orders.Any(o => o.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteOrder(Order order)
        {
            _context.Remove(order);
            return Save();
        }

        public bool UpdateOrder(Order order)
        {
            _context.Update(order);
            return Save();
        }

        public bool UpdateOrderProductQuantity(int orderId, int orderProductId, int quantity)
        {
            var orderProduct = _context.OrderProducts.FirstOrDefault(op => op.OrderId == orderId && op.ProductId == orderProductId);

            if (orderProduct == null)
            {
                return false; // Nie znaleziono odpowiedniego wpisu OrderProduct
            }

            orderProduct.Quantity = quantity;
            _context.Update(orderProduct);
            return Save();
        }

        public Order GetOrder(int orderId)
        {
            return _context.Orders.Where(o => o.Id == orderId).FirstOrDefault();
        }

        public ICollection<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }

        public ICollection<Product> GetProductsFromOrder(int orderId)
        {
            var order = _context.Orders
                         .Include(o => o.OrderProducts)
                             .ThenInclude(op => op.Product)
                         .FirstOrDefault(o => o.Id == orderId);

            if (order == null || order.OrderProducts == null)
                return new List<Product>();

            var products = order.OrderProducts.Select(op => op.Product).ToList();
         
            foreach (var product in products)
            {
                var orderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == product.Id);
                if (orderProduct != null)
                {
                    product.Qty = orderProduct.Quantity;
                }
            }

            return products;
        }

        public bool DeleteOrderAndAssociatedProducts(int orderId)
        {
            var orderToDelete = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (orderToDelete == null)
            {
                return false;
            }

            // Usuń wszystkie powiązane wpisy w tabeli wiele do wielu OrderProducts
            var orderProductsToDelete = _context.OrderProducts.Where(op => op.OrderId == orderId);
            _context.OrderProducts.RemoveRange(orderProductsToDelete);

            _context.Orders.Remove(orderToDelete);

            return Save();
        }

        public bool RemoveProductFromOrder(int orderId, int productId)
        {                     
            var order = _context.Orders
                         .Include(o => o.OrderProducts)
                         .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return false;
            }

            var orderProductToRemove = order.OrderProducts.FirstOrDefault(op => op.ProductId == productId);

            if (orderProductToRemove != null)
            {
                _context.OrderProducts.Remove(orderProductToRemove);
                return Save();
            }

            return false;
        }

        public bool OrderProductExists(int orderId, int productId)
        {
            var order = _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return false;
            }

            var orderProduct = order.OrderProducts.FirstOrDefault(op => op.ProductId == productId);

            if (orderProduct != null)
            {
                return _context.OrderProducts.Any(o => o.ProductId == productId);
            }

            return false;
        }
    }
}
