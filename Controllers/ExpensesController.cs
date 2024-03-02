 using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Models;
using plusminus.Services.ExpensesService;

namespace plusminus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesService _expensesService;

        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }

        [HttpGet("expanses/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> GetExpanses(int id) => Ok(await _expensesService.GetExpensesByUserId(id));

        [HttpPost("expanses/add")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> AddExpenses(AddExpensesDto newExpenses) => Ok(await _expensesService.AddExpenses(newExpenses));
        

        [HttpPatch("expanses/update")]
        public async Task<ActionResult<ServiceResponse<GetExpensesDto>>> UpdateExpenses(UpdateExpensesDto newExpenses) => Ok(await _expensesService.UpdateExpenses(newExpenses));

        [HttpDelete("expanses/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetExpensesDto>>>> DeleteExpenses(int id) => Ok(await _expensesService.DeleteExpensesById(id));
    }
}
