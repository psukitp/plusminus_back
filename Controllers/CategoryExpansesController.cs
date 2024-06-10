using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.CategoryExpenses;
using plusminus.Models;
using plusminus.Services.CategoryExpansesService;
using plusminus.Middlewares;

namespace plusminus.Controllers
{
    [ServiceFilter(typeof(AuthorizeFilter))]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryExpansesController : ControllerBase
    {
        private readonly ICategoryExpansesService _categoryExpansesService;

        public CategoryExpansesController(ICategoryExpansesService categoryExpansesService)
        {
            _categoryExpansesService = categoryExpansesService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryExpansesDto>>>> GetCategories()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _categoryExpansesService.GetAllCategories(userId));
        }

        [HttpPost("category/expenses/add")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryExpansesDto>>>> AddCategoryExpanses(
            AddCategoryExpansesDto newCategoryExpanses)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _categoryExpansesService.AddCategoryExpanses(newCategoryExpanses, userId));
        }


        [HttpPatch("category/expenses/update")]
        public async Task<ActionResult<ServiceResponse<GetCategoryExpansesDto>>> UpdateCategoryExpanses(
            UpdateCategoryExpansesDto updatedCategoryExpanses)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _categoryExpansesService.UpdateCategoryExpanses(updatedCategoryExpanses, userId));
        }

        [HttpDelete("category/expenses/{id}")]
        public async Task<ActionResult<ServiceResponse<int>>> DeleteCategoryExpanses(int id)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            return Ok(await _categoryExpansesService.DeleteCategoryExpansesById(id, userId));
        }
    }
}
