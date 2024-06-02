using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.CategoryIncomes;
using plusminus.Models;
using plusminus.Services.CategoryIncomesService;

namespace plusminus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryIncomesController : ControllerBase
    {
        private readonly ICategoryIncomesService _categoryIncomesService;

        public CategoryIncomesController(ICategoryIncomesService categoryIncomesService)
        {
            _categoryIncomesService = categoryIncomesService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryIncomesDto>>>> GetCategories()
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

            return Ok(await _categoryIncomesService.GetAllIncomes(userId));
        }

        [HttpPost("category/incomes/add")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryIncomesDto>>>> AddCategoryIncomes(
            AddCategoryIncomesDto newCategoryIncomes)
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
            
            return Ok(await _categoryIncomesService.AddCategoryIncomes(newCategoryIncomes, userId));
        }


        [HttpPatch("category/incomes/update")]
        public async Task<ActionResult<ServiceResponse<GetCategoryIncomesDto>>> UpdateCategoryIncomes(
            UpdateCategoryIncomesDto updatedCategoryIncomes)
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
            
            return Ok(await _categoryIncomesService.UpdateCategoryIncomes(updatedCategoryIncomes, userId));
        }

        [HttpDelete("category/incomes/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryIncomesDto>>>> DeleteCategoryIncomes(int id)
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
            
            return Ok(await _categoryIncomesService.DeleteCategoryIncomesById(id, userId));
        }
    }
}
