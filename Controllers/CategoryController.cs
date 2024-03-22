using Microsoft.AspNetCore.Mvc;
using SimpleProductOrder.Models;
using SimpleProductOrder.Services;
using SimpleProductOrder.Dto;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleProductOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        private readonly IMapper _mapper;

        public CategoryController(CategoryService categoryService, ProductService productService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryService.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        /// <summary>
        /// Get category with selected id
        /// </summary>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryService.CategoryExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryService.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        /// <summary>
        /// Add category
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Category
        ///     {
        ///        "name": "food"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            var category = _categoryService.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

         

            if (category != null)
            {
                ModelState.AddModelError("", "category already exists");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryService.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        /// <summary>
        /// Update category
        /// </summary>
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            // Jeśli id w ciele żądania jest różne od id w adresie URL, zwróć BadRequest
            if (updatedCategory.Id != 0 && updatedCategory.Id != categoryId)
                return BadRequest("Id in URL does not match id in request body.");

            // Jeśli id w ciele żądania jest zerowe lub puste, użyj id z adresu URL
            if (updatedCategory.Id == 0)
                updatedCategory.Id = categoryId;

            if (!_categoryService.CategoryExists(categoryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!_categoryService.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete category
        /// </summary>
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int category_id)
        {
            if (!_categoryService.CategoryExists(category_id))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryService.GetCategory(category_id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryService.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return BadRequest(ModelState);
            }

            return NoContent();
        }
    }
}
