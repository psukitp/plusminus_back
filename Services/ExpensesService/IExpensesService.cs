using plusminus.Dtos.Expenses;
using plusminus.Models;

namespace plusminus.Services.ExpensesService
{
    public interface IExpensesService
    {
        Task<ServiceResponse<List<GetExpensesDto>>> GetExpensesByUserId(int id, DateOnly date);
        Task<ServiceResponse<List<GetExpensesDto>>> AddExpenses(AddExpensesDto newExpenses, int userId);
        Task<ServiceResponse<GetExpensesDto>> UpdateExpenses(UpdateExpensesDto newExpenses);
        Task<ServiceResponse<List<GetExpensesDto>>> DeleteExpensesById(int id);
        Task<ServiceResponse<List<ExpensesByCategory>>> GetExpansesByCategory(int id, DateOnly date);
        Task<ServiceResponse<List<ExpensesByCategory>>> GetExpansesByCategoryMonth(int id);
        Task<ServiceResponse<ExpensesThisMonthStat>> GetExpensesSum(int id);
        Task<ServiceResponse<GetThisYearExpenses>> GetExpensesThisYear(int id);
    }
}
