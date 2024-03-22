using SimpleProductOrder.Models;
using SimpleProductOrder.Dto;
using AutoMapper;

namespace SimpleProductOrder.Mapper
{
    public class Mapping : Profile
    {
        public Mapping() 
        {
            CreateMap<Account, AccountDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<OrderProduct, OrderProductDto>();
            CreateMap<AccountDto, Account>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<OrderDto, Order>();
            CreateMap<ProductDto, Product>();
            CreateMap<ReviewDto, Review>();
            CreateMap<OrderProductDto, OrderProduct>();
        }
    }

}
