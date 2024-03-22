using SimpleProductOrder.Data;
using SimpleProductOrder.Models;

namespace SimpleProductOrder
{
    public class Seed
    {
        private readonly AppDbContext appDbContext;

        public Seed(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public void SeedAppDbContext()
        {
            if (!appDbContext.Categories.Any() && !appDbContext.Customers.Any() && !appDbContext.Accounts.Any() && !appDbContext.Products.Any() && !appDbContext.Orders.Any() &&
                !appDbContext.OrderProducts.Any() && !appDbContext.Reviews.Any())
            {
                var categories = new List<Category>()
                {
                    new Category { Name = "pasta" },
                    new Category { Name = "meat" },
                    new Category { Name = "spices" },
                    new Category { Name = "sauce" },
                    new Category { Name = "cheese" },
                    new Category { Name = "seafood" },
                    new Category { Name = "coffee" },
                    new Category { Name = "snacks" },
                    new Category { Name = "liquor" }
                };
              
                var customers = new List<Customer>()
                {
                    new Customer { Name = "Wiktor", Surname = "Soursoup", Email = "email1@example.com", Phone = "505606707", Address= "ul. Sezamkowa 16 80-154 Warszawa"},
                    new Customer { Name = "Janusz", Surname = "Kowalczyk", Email = "email2@example.com", Phone = "789606707", Address= "ul. Krokowa 50 80-154 Warszawa"}
                };               

                var accounts = new List<Account>()
                {
                    new Account { Login = "wiktor1", Password = "wiktor1", Customer = customers[0]},
                      new Account { Login = "janusz2", Password = "janusz2", Customer = customers[1]}
                };
              
                var products = new List<Product>()
                {
                      new Product { Name = "Spaghetti", Price = 2.99, Weight = 0.5, Description = "Italian pasta", Qty = 100, Category = categories[0] },
                      new Product { Name = "Fusilli", Price = 3.49, Weight = 0.6, Description = "Spiral pasta", Qty = 80, Category = categories[0] },
                      new Product { Name = "Penne", Price = 3.29, Weight = 0.7, Description = "Tube-shaped pasta", Qty = 90, Category = categories[0] },
                      new Product { Name = "Beef", Price = 9.99, Weight = 1.5, Description = "Fresh meat", Qty = 50, Category = categories[1] },
                      new Product { Name = "Chicken", Price = 6.99, Weight = 1.2, Description = "Skinless chicken breast", Qty = 70, Category = categories[1] },
                      new Product { Name = "Pork", Price = 8.49, Weight = 1.8, Description = "Tenderloin pork", Qty = 60, Category = categories[1] },
                      new Product { Name = "Salt", Price = 1.49, Weight = 0.2, Description = "Sea salt", Qty = 200, Category = categories[2] },
                      new Product { Name = "Pepper", Price = 2.99, Weight = 0.1, Description = "Black pepper", Qty = 150, Category = categories[2] },
                      new Product { Name = "Cinnamon", Price = 3.99, Weight = 0.3, Description = "Ground cinnamon", Qty = 120, Category = categories[2] },
                      new Product { Name = "Tomato Sauce", Price = 1.99, Weight = 0.5, Description = "Italian tomato sauce", Qty = 100, Category = categories[3] },
                      new Product { Name = "Mozzarella Cheese", Price = 4.49, Weight = 0.4, Description = "Fresh mozzarella cheese", Qty = 70, Category = categories[4] },
                      new Product { Name = "Salmon", Price = 12.99, Weight = 0.9, Description = "Fresh salmon fillet", Qty = 40, Category = categories[5] },
                      new Product { Name = "Espresso", Price = 3.49, Weight = 0.4, Description = "Strong Italian coffee", Qty = 50, Category = categories[6] },
                      new Product { Name = "Cappuccino", Price = 4.99, Weight = 0.5, Description = "Italian coffee with milk foam", Qty = 40, Category = categories[6] },
                      new Product { Name = "Latte", Price = 4.79, Weight = 0.6, Description = "Italian coffee with milk", Qty = 45, Category = categories[6] },
                      new Product { Name = "Potato Chips", Price = 1.99, Weight = 0.2, Description = "Crispy potato chips", Qty = 150, Category = categories[7] },
                      new Product { Name = "Chocolate Bar", Price = 2.49, Weight = 0.1, Description = "Milk chocolate bar", Qty = 120, Category = categories[7] },
                      new Product { Name = "Popcorn", Price = 1.79, Weight = 0.3, Description = "Butter-flavored popcorn", Qty = 130, Category = categories[7] },
                      new Product { Name = "Whiskey", Price = 29.99, Weight = 0.7, Description = "Scotch whiskey", Qty = 30, Category = categories[8] },
                      new Product { Name = "Vodka", Price = 19.99, Weight = 0.8, Description = "Russian vodka", Qty = 40, Category = categories[8] },
                      new Product { Name = "Rum", Price = 24.99, Weight = 0.6, Description = "Caribbean rum", Qty = 35, Category = categories[8] }
                };

                var orders = new List<Order>()
                {
                     new Order { Order_date = DateTime.UtcNow.AddDays(-6), Customer = customers[0] }
                };


                var reviews = new List<Review>()
                {
                    new Review { Title = "Great product!", Content = "I'm very satisfied with my purchase.", Author = customers[0], Product = products[0]}

                };

                appDbContext.Categories.AddRange(categories);            

                appDbContext.Customers.AddRange(customers);              

                appDbContext.Accounts.AddRange(accounts);                

                appDbContext.Products.AddRange(products);

                appDbContext.Orders.AddRange(orders);

                appDbContext.Reviews.AddRange(reviews);

                appDbContext.SaveChanges();
            }
        }
        




    }

}

