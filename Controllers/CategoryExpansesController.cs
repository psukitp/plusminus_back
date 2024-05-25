using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.CategoryExpenses;
using plusminus.Models;
using plusminus.Services.CategoryExpansesService;
using System.Security.Claims;

namespace plusminus.Controllers
{
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
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Unauthorized();
            }

            if (!int.TryParse(authenticateResult.Principal.FindFirstValue("id"), out int userId))
            {
                return BadRequest("Неверный идентификатор пользователя.");
            }

            return Ok(await _categoryExpansesService.GetAllCategories(userId));
        }

        [HttpPost("category/expenses/add")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryExpansesDto>>>> AddCategoryExpanses(AddCategoryExpansesDto newCategoryExpanses) => Ok(await _categoryExpansesService.AddCategoryExpanses(newCategoryExpanses));


        [HttpPatch("category/expenses/update")]
        public async Task<ActionResult<ServiceResponse<GetCategoryExpansesDto>>> UpdateCategoryExpanses(UpdateCategoryExpansesDto updatedCategoryExpanses) => Ok(await _categoryExpansesService.UpdateCategoryExpanses(updatedCategoryExpanses));

        [HttpDelete("category/expenses/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryExpansesDto>>>> DeleteCategoryExpanses(int id) => Ok(await _categoryExpansesService.DeleteCategoryExpansesById(id));
    }
}
