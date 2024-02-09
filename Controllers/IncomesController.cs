using Microsoft.AspNetCore.Mvc;
using plusminus.Dtos.Expenses;
using plusminus.Dtos.Incomes;
using plusminus.Models;
using plusminus.Services.IncomesService;

namespace plusminus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncomesController : ControllerBase
    {
        private readonly IIncomesService _incomesService;

        public IncomesController(IIncomesService incomesService)
        {
            _incomesService = incomesService;
        }

        [HttpGet("incomes/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> GetExpanses(int id) => Ok(await _incomesService.GetIncomesByUserId(id));

        [HttpPost("incomes/add")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> AddExpenses(AddIncomesDto newIncomes) => Ok(await _incomesService.AddIncomes(newIncomes));

        [HttpPatch("incomes/update")]
        public async Task<ActionResult<ServiceResponse<GetIncomesDto>>> UpdateExpenses(UpdateIncomesDto updatedIncomes) => Ok(await _incomesService.UpdateIncomes(updatedIncomes));

        [HttpDelete("incomes/{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetIncomesDto>>>> DeleteExpenses(int id) => Ok(await _incomesService.DeleteIncomesById(id));
    }
}
