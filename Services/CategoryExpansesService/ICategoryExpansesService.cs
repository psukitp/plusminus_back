using plusminus.Dtos.CategoryExpenses;
using plusminus.Models;

namespace plusminus.Services.CategoryExpansesService
{
    public interface ICategoryExpansesService
    {
        Task<ServiceResponse<List<GetCategoryExpansesDto>>> AddCategoryExpanses(AddCategoryExpansesDto newCategoryExpanses);
        Task<ServiceResponse<GetCategoryExpansesDto>> UpdateCategoryExpanses(UpdateCategoryExpansesDto updatedCategoryExpanses);
        Task<ServiceResponse<List<GetCategoryExpansesDto>>> DeleteCategoryExpansesById(int id);
        Task<ServiceResponse<List<GetCategoryExpansesDto>>> GetAllCategories(int userId);
    }
}
