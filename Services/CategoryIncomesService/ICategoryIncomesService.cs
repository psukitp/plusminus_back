using plusminus.Dtos.CategoryIncomes;
using plusminus.Models;

namespace plusminus.Services.CategoryIncomesService
{
    public interface ICategoryIncomesService
    {
        Task<ServiceResponse<List<GetCategoryIncomesDto>>> AddCategoryIncomes(AddCategoryIncomesDto newCategoryIncomes);
        Task<ServiceResponse<GetCategoryIncomesDto>> UpdateCategoryIncomes(UpdateCategoryIncomesDto updatedCategoryIncomes);
        Task<ServiceResponse<List<GetCategoryIncomesDto>>> DeleteCategoryIncomesById(int id);
        Task<ServiceResponse<List<GetCategoryIncomesDto>>> GetAllIncomes(int userId);
    }
}
