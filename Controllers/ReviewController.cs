using Microsoft.AspNetCore.Mvc;
using SimpleProductOrder.Models;
using SimpleProductOrder.Services;
using System;
using System.Collections.Generic;
using AutoMapper;
using SimpleProductOrder.Dto;

namespace SimpleProductOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        private readonly CustomerService _customerService;

        private readonly ProductService _productService;

        private readonly IMapper _mapper;

        public ReviewController(ReviewService reviewService, CustomerService customerService, ProductService productService, IMapper mapper)
        {
            _reviewService = reviewService;
            _customerService = customerService;
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all reviews
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDto>>(_reviewService.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        /// <summary>
        /// Get review with selected id
        /// </summary>
        [HttpGet("{reviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewService.ReviewExists(reviewId))
                return NotFound();

            var review = _mapper.Map<ReviewDto>(_reviewService.GetReview(reviewId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(review);
        }

        /// <summary>
        /// Add review
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Account
        ///     {
        ///        "title": "string",
        ///        "content": "string"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int customerId, [FromQuery] int productId, [FromBody] ReviewDto reviewCreate)
        {
            if (reviewCreate == null)
                return BadRequest(ModelState);

       

            var idC = _customerService.GetCustomer(customerId);

            if (idC == null)
            {
                ModelState.AddModelError("", $"customer id:{customerId} not found");
                return StatusCode(422, ModelState);
            }

            var idP = _productService.GetProduct(productId);

            if (idP == null)
            {
                ModelState.AddModelError("", $"product id:{productId} not found");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Author = _customerService.GetCustomer(customerId);
            reviewMap.Product = _productService.GetProduct(productId);

            if (!_reviewService.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

      

            return Ok("Successfully created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
                return BadRequest(ModelState);

            if (reviewId != updatedReview.Id)
                return BadRequest(ModelState);

            if (!_reviewService.ReviewExists(reviewId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var reviewMap = _mapper.Map<Review>(updatedReview);

            if (!_reviewService.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAccount(int reviewId)
        {
            if (!_reviewService.ReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewToDelete = _reviewService.GetReview(reviewId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewService.DeleteReview(reviewToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting review");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
