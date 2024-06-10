using plusminus.Dtos.Incomes;
using plusminus.Models;

namespace plusminus.Services.IncomesService
{
    public interface IIncomesService
    {
        Task<ServiceResponse<List<GetIncomesDto>>> GetIncomesByUserId(int id, DateOnly date);
        Task<ServiceResponse<List<GetIncomesDto>>> AddIncomes(AddIncomesDto newIncomes, int userId);
        Task<ServiceResponse<GetIncomesDto>> UpdateIncomes(UpdateIncomesDto updatedIncomes);
        Task<ServiceResponse<int>> DeleteIncomesById(int id);
        Task<ServiceResponse<List<IncomesByCategory>>> GetIncomesByCategory(int id, DateOnly date);
        Task<ServiceResponse<GetIncomesThisMonthStat>> GetIncomesSum(int id);
        Task<ServiceResponse<GeThisYearIncomes>> GetIncomesLastFourMonth(int id);
        Task<ServiceResponse<int>> GetTotalDiff(int userId);
    }
}
