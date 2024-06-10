using plusminus.Dtos.CategoryExpenses;
using plusminus.Models;

namespace plusminus.Services.CategoryExpansesService
{
    public interface ICategoryExpansesService
    {
        Task<ServiceResponse<GetCategoryExpansesDto>> AddCategoryExpanses(AddCategoryExpansesDto newCategoryExpanses, int userId);
        Task<ServiceResponse<GetCategoryExpansesDto>> UpdateCategoryExpanses(UpdateCategoryExpansesDto updatedCategoryExpanses, int userId);
        Task<ServiceResponse<int>> DeleteCategoryExpansesById(int id, int userId);
        Task<ServiceResponse<List<GetCategoryExpansesDto>>> GetAllCategories(int userId);
        Task AddBaseCategories(int userId);
    }
}
