using AutoMapper;
using Microsoft.EntityFrameworkCore;
using plusminus.Data;
using plusminus.Dtos.Expenses;
using plusminus.Models;

namespace plusminus.Services.ExpensesService
{
    public class ExpensesService : IExpensesService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ExpensesService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<List<GetExpensesDto>>> AddExpenses(AddExpensesDto newExpenses)
        {
            var serviceResponse = new ServiceResponse<List<GetExpensesDto>>();

            var expenses = _mapper.Map<Expenses>(newExpenses);
            var addedExpenses = await _context.Expenses.AddAsync(expenses);
            await _context.SaveChangesAsync();
            
            var dbExpenses = await _context.Expenses.ToListAsync();
            serviceResponse.Data = dbExpenses.Select(e => _mapper.Map<GetExpensesDto>(e)).ToList();
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetExpensesDto>>> DeleteExpensesById(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetExpensesDto>>();
            try
            {
                var expenses = await _context.Expenses.FindAsync(id);
                if (expenses is null) throw new Exception("Данные расходы не были найдены");

                _context.Expenses.Remove(expenses);
                await _context.SaveChangesAsync();
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetExpensesDto>>> GetExpensesByUserId(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetExpensesDto>>();

            try
            {
                var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();
                var dbExpenses = expenses.Where(e => e.UserId == id);
                serviceResponse.Data = dbExpenses.Select(e => _mapper.Map<GetExpensesDto>(e)).ToList();
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetExpensesDto>> UpdateExpenses(UpdateExpensesDto newExpenses)
        {
            var serviceResponse = new ServiceResponse<GetExpensesDto>();
            try
            {
                var expenses = await _context.Expenses.FindAsync(newExpenses.Id);
                if (expenses is null) throw new Exception("Данные расходы не были найдены");

                expenses.Date = newExpenses.Date;
                expenses.Amount = newExpenses.Amount;
                expenses.CategoryId = newExpenses.CategoryId;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetExpensesDto>(expenses);
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
