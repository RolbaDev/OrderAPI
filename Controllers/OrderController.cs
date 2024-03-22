using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleProductOrder.Dto;
using SimpleProductOrder.Models;
using SimpleProductOrder.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace SimpleProductOrder.Controllers


{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public OrderController(OrderService orderService, IMapper mapper, ProductService productService, CustomerService customerService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _productService = productService;
            _customerService = customerService;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetOrders()
        {
            var orders = _mapper.Map<List<OrderDto>>(_orderService.GetOrders());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        /// <summary>
        /// Get order with selected id
        /// </summary>
        [HttpGet("{orderId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult GetOrder(int orderId)
        {
            if (!_orderService.OrderExists(orderId))
                return NotFound();

            var order = _mapper.Map<OrderDto>(_orderService.GetOrder(orderId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(order);
        }

        /// <summary>
        /// Get all products from selected order
        /// </summary>
        [HttpGet("{orderId}/products")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult GetProductsFromOrder(int orderId)
        {
            if (!_orderService.OrderExists(orderId))
                return NotFound();

            var products = _mapper.Map<List<ProductDto>>(_orderService.GetProductsFromOrder(orderId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(products);
        }

        /// <summary>
        /// Add order
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Order
        ///     {
        ///        "order_date": "2024-03-22T14:31:52.677Z"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromQuery] int customerId, [FromBody] OrderDto orderCreate)
        {
            if (orderCreate == null)
                return BadRequest(ModelState);

            var id = _customerService.GetCustomer(customerId);

            if (id == null)
            {
                ModelState.AddModelError("", $"customer id:{customerId} not found");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var OrderMap = _mapper.Map<Order>(orderCreate);

            OrderMap.Customer = _customerService.GetCustomer(customerId);

            if (!_orderService.CreateOrder(OrderMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }


        /// <summary>
        /// Add product to selected order
        /// </summary>
        [HttpPost("{orderId}/product/{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddProductToOrder(int productId, int orderId, [FromBody] OrderProductDto orderProduct)
        {
            var product = _productService.GetProduct(productId);

            var order = _orderService.GetOrder(orderId);

            if (product == null)
            {
                ModelState.AddModelError("", $"product id:{productId} not found");
                return StatusCode(422, ModelState);
            }
            if (order == null)
            {
                ModelState.AddModelError("", $"order id:{orderId} not found");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var OrderProductMap = _mapper.Map<OrderProduct>(orderProduct);

            OrderProductMap.Product = _productService.GetProduct(productId);
            OrderProductMap.Order = _orderService.GetOrder(orderId);

            if (!_orderService.CreateOrderProduct(OrderProductMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }


        /// <summary>
        /// Update order
        /// </summary>
        [HttpPut("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOrder(int orderId, [FromBody] OrderDto updatedOrder)
        {
            if (updatedOrder == null)
                return BadRequest(ModelState);

            if (orderId != updatedOrder.Id)
                return BadRequest(ModelState);

            if (!_orderService.OrderExists(orderId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var orderMap = _mapper.Map<Order>(updatedOrder);

            if (!_orderService.UpdateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating order");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Update product in selected order
        /// </summary>
        [HttpPut("{orderId}/products/{orderProductId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProductQtyInOrder(int orderId, int orderProductId, [FromBody] OrderProductDto updatedOrderProduct)
        {
            if (updatedOrderProduct == null)
                return BadRequest(ModelState);

            if (!_orderService.OrderExists(orderId))
                return NotFound();

            if (!_orderService.OrderProductExists(orderId, orderProductId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_orderService.UpdateOrderProductQuantity(orderId, orderProductId, updatedOrderProduct.Quantity))
            {
                ModelState.AddModelError("", "Something went wrong while updating ");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete order
        /// </summary>
        [HttpDelete("{orderId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrder(int orderId)
        {
            var success = _orderService.DeleteOrderAndAssociatedProducts(orderId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Delete product from selected order
        /// </summary>
        [HttpDelete("{orderId}/products/{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult RemoveProductFromOrder(int orderId, int productId)
        {
            var success = _orderService.RemoveProductFromOrder(orderId, productId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }


    }
}
