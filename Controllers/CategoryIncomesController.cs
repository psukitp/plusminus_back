using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.CategoryIncomes;
using plusminus.Models;
using plusminus.Services.CategoryIncomesService;

namespace plusminus.Controllers
{
    public class CategoryIncomesController: ControllerBase
    {
        private readonly ICategoryIncomesService _categoryIncomesService;

        public CategoryIncomesController(ICategoryIncomesService categoryIncomesService)
        {
            _categoryIncomesService = categoryIncomesService;
        }

        [HttpPost("category/incomes/add")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryIncomesDto>>>> AddCategoryIncomes(AddCategoryIncomesDto newCategoryIncomes)
        {
            return Ok(await _categoryIncomesService.AddCategoryIncomes(newCategoryIncomes));
        }

        [HttpPatch("category/incomes/update")]
        public async Task<ActionResult<ServiceResponse<GetCategoryIncomesDto>>> UpdateCategoryIncomes(UpdateCategoryIncomesDto updatedCategoryIncomes)
        {
            return Ok(await _categoryIncomesService.UpdateCategoryIncomes(updatedCategoryIncomes));
        }

        [HttpDelete("category/incomes/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryIncomesDto>>>> DeleteCategoryIncomes(int id)
        {
            return Ok(await _categoryIncomesService.DeleteCategoryIncomesById(id));
        }
    }
}
