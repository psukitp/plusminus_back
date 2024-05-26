using plusminus.Dtos.Incomes;
using plusminus.Models;

namespace plusminus.Services.IncomesService
{
    public interface IIncomesService
    {
        Task<ServiceResponse<List<GetIncomesDto>>> GetIncomesByUserId(int id, DateOnly date);
        Task<ServiceResponse<List<GetIncomesDto>>> AddIncomes(AddIncomesDto newIncomes, int userId);
        Task<ServiceResponse<GetIncomesDto>> UpdateIncomes(UpdateIncomesDto updatedIncomes);
        Task<ServiceResponse<List<GetIncomesDto>>> DeleteIncomesById(int id);
        Task<ServiceResponse<List<IncomesByCategory>>> GetIncomesByCategory(int id, DateOnly date);

    }
}
