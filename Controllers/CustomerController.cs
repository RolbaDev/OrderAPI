using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleProductOrder.Dto;
using SimpleProductOrder.Models;
using SimpleProductOrder.Services;
using System;
using System.Collections.Generic;

namespace SimpleProductOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly ReviewService _reviewService;
        private readonly IMapper _mapper;

        public CustomerController(CustomerService customerService, ReviewService reviewService, IMapper mapper)
        {
            _customerService = customerService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Customer>))]
        public IActionResult GetCustomers()
        {
            var customers = _mapper.Map<List<CustomerDto>>(_customerService.GetCustomers());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customers);
        }

        /// <summary>
        /// Get customer with selected id
        /// </summary>
        [HttpGet("{customerId}")]
        [ProducesResponseType(200, Type = typeof(Customer))]
        [ProducesResponseType(400)]
        public IActionResult GetCustomer(int customerId)
        {
            if (!_customerService.CustomerExists(customerId))
                return NotFound();

            var customer = _mapper.Map<CustomerDto>(_customerService.GetCustomer(customerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(customer);
        }

        /// <summary>
        /// Add customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///   POST /api/Customer
        ///     {
        ///        "name": "string",
        ///        "surname": "string",
        ///        "email": "string",
        ///        "phone": "string",
        ///        "address": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomer([FromBody] CustomerDto customerCreate)
        {
            if (customerCreate == null)
                return BadRequest(ModelState);

            var customers = _customerService.GetCustomers()
                .Where(c => c.Surname.Trim().ToUpper() == customerCreate.Surname.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (customers != null)
            {
                ModelState.AddModelError("", "Customer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerMap = _mapper.Map<Customer>(customerCreate);


            if (!_customerService.CreateCustomer(customerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update category
        /// </summary>
        [HttpPut("{customerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustomer(int customerId, [FromBody] CustomerDto updatedCustomer)
        {
            if (updatedCustomer == null)
                return BadRequest(ModelState);

            if (customerId != updatedCustomer.Id)
                return BadRequest(ModelState);

            if (!_customerService.CustomerExists(customerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var customerMap = _mapper.Map<Customer>(updatedCustomer);

            if (!_customerService.UpdateCustomer(customerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete category
        /// </summary>
        [HttpDelete("{customerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCustomer(int customerId)
        {
            if (!_customerService.CustomerExists(customerId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewService.GetReviewsOfACustomer(customerId);
            var customerToDelete = _customerService.GetCustomer(customerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewService.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_customerService.DeleteCustomer(customerToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting owner");
            }

            return NoContent();
        }

    }
}