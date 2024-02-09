using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.CategoryExpenses;
using plusminus.Models;
using plusminus.Services.CategoryExpansesService;

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

        [HttpPost("category/expanses/add")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryExpansesDto>>>> AddCategoryExpanses(AddCategoryExpansesDto newCategoryExpanses) => Ok(await _categoryExpansesService.AddCategoryExpanses(newCategoryExpanses));


        [HttpPatch("category/expanses/update")]
        public async Task<ActionResult<ServiceResponse<GetCategoryExpansesDto>>> UpdateCategoryExpanses(UpdateCategoryExpansesDto updatedCategoryExpanses) => Ok(await _categoryExpansesService.UpdateCategoryExpanses(updatedCategoryExpanses));

        [HttpDelete("category/expanses/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCategoryExpansesDto>>>> DeleteCategoryExpanses(int id) => Ok(await _categoryExpansesService.DeleteCategoryExpansesById(id));
    }
}
