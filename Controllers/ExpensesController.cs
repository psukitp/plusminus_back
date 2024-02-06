using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Models;
using plusminus.Services.ExpensesService;

namespace plusminus.Controllers
{
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService _expensesService;

        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> GetExpanses(int id)
        {
            return Ok(await _expensesService.GetExpensesByUserId(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> AddExpenses(AddExpensesDto newExpenses)
        {
            return Ok(await _expensesService.AddExpenses(newExpenses));
        }

        [HttpPatch]
        public async Task<ActionResult<ServiceResponse<GetExpensesDto>>> UpdateExpenses(UpdateExpensesDto newExpenses)
        {
            return Ok(await _expensesService.UpdateExpenses(newExpenses));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> DeleteExpenses(int id)
        {
            return Ok(await _expensesService.DeleteExpensesById(id));
        }
    }
}
