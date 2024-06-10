using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using plusminus.Data;
using plusminus.Dtos.Incomes;
using plusminus.Models;

namespace plusminus.Services.IncomesService
{
    public class IncomesService : IIncomesService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public IncomesService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<List<GetIncomesDto>>> AddIncomes(AddIncomesDto newIncomes, int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetIncomesDto>>();
            try
            {
                var incomes = _mapper.Map<AddIncomesDto, Incomes>(newIncomes);
                Incomes incomesToAdd = new Incomes
                {
                    Amount = incomes.Amount,
                    Category = incomes.Category,
                    UserId = userId,
                    CategoryId = incomes.CategoryId,
                    Date = incomes.Date,
                    Id = incomes.Id,
                    User = incomes.User
                };
                var addedIncomes = await _context.Incomes.AddAsync(incomesToAdd);
                await _context.SaveChangesAsync();

                var dbIncomes = await _context.Incomes.Include(i => i.Category).ToListAsync();
                var incomesWithDate = dbIncomes.Where(e => e.UserId == userId && e.Date == incomesToAdd.Date);
                serviceResponse.Data = incomesWithDate.Select(i => _mapper.Map<Incomes, GetIncomesDto>(i)).ToList();
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> DeleteIncomesById(int id, int userId)
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                var incomes = await _context.Incomes.FindAsync(id);
                if (incomes is null || incomes.UserId != userId) throw new Exception("Данные доходы не были найдены");
                
                var currentId = incomes.Id;

                _context.Incomes.Remove(incomes);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data = currentId;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetIncomesDto>>> GetIncomesByUserId(int id, DateOnly date)
        {
            var serviceResponse = new ServiceResponse<List<GetIncomesDto>>();
            try
            {
                var dbIncomes = await _context.Incomes.Include(i => i.Category).ToListAsync();
                var incomes = dbIncomes.Where(i => i.UserId == id && i.Date == date);
                if (incomes is null) throw new Exception("У вас нет таких доходов");

                serviceResponse.Data = incomes.Select(_mapper.Map<Incomes, GetIncomesDto>).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetIncomesDto>> UpdateIncomes(UpdateIncomesDto updatedIncomes, int userId)
        {
            var serviceResponse = new ServiceResponse<GetIncomesDto>();
            try
            {
                var incomes = await _context.Incomes.FindAsync(updatedIncomes.Id);
                if (incomes is null || incomes.UserId != userId) throw new Exception("Не удалось найти данные доходы");
                
                if (incomes.Amount != null) incomes.Amount = updatedIncomes.Amount;
                if (incomes.CategoryId != null) incomes.CategoryId = updatedIncomes.CategoryId;

                await _context.SaveChangesAsync();
                
                var category = await _context.CategoryIncomes.FindAsync(incomes.CategoryId);
                incomes.Category = category;
                serviceResponse.Data = _mapper.Map<Incomes, GetIncomesDto>(incomes);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<IncomesByCategory>>> GetIncomesByCategory(int id, DateOnly date)
        {
            var serviceResponse = new ServiceResponse<List<IncomesByCategory>>();
            try
            {
                var incomes = await _context.Incomes.Include(i => i.Category).ToListAsync();

                var dbIncomes = incomes
                    .Where(i => i.UserId == id && i.Date.Month == date.Month && i.Date.Year == date.Year)
                    .GroupBy(i => i.CategoryId)
                    .Select(g => new
                    {
                        categoryId = g.Key,
                        amount = g.Sum(i => i.Amount)
                    })
                    .ToList();

                var result = new List<IncomesByCategory>();
                foreach (var income in dbIncomes)
                {
                    var category = await _context.CategoryIncomes.FirstOrDefaultAsync(c => c.Id == income.categoryId);
                    if (category != null)
                    {
                        result.Add(new IncomesByCategory
                        {
                            Id = category.Id,
                            CategoryName = category.Name,
                            Color = category.Color,
                            Amount = income.amount
                        });
                    }
                }

                serviceResponse.Data = result;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
        
        public async Task<ServiceResponse<GetIncomesThisMonthStat>> GetIncomesSum(int id)
        {
            var serviceResponse = new ServiceResponse<GetIncomesThisMonthStat>();
            try
            {
                var currentDate = DateTime.Now;
                var incomesThisMonth = await _context.Incomes
                    .Where(i => i.Date.Month == currentDate.Month && i.Date.Year == currentDate.Year && i.UserId == id)
                    .SumAsync(i => i.Amount);

                var prevDate = currentDate.AddMonths(-1);
                var incomesPrevMonth = await _context.Incomes
                    .Where(i => i.Date.Month == prevDate.Month && i.Date.Year == prevDate.Year && i.UserId == id)
                    .SumAsync(i => i.Amount);

                GetIncomesThisMonthStat result = new GetIncomesThisMonthStat
                {
                    IncomesDiff = incomesThisMonth - incomesPrevMonth,
                    IncomesTotal = incomesThisMonth
                };
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
        
        public async Task<ServiceResponse<GeThisYearIncomes>> GetIncomesLastFourMonth(int id)
        {
            var serviceResponse = new ServiceResponse<GeThisYearIncomes>();
            try
            {
                var currentDate = DateTime.Now;
                GeThisYearIncomes result = new GeThisYearIncomes();
                result.Monthes = new List<string>();
                result.Values = new List<double>();
                
                for (var i = 0; i < currentDate.Month; i++)
                {
                    var month = currentDate.AddMonths(-i);
                    var monthIncomes = await _context.Incomes
                        .Where(e => e.Date.Month == month.Month && e.Date.Year == month.Year && e.UserId == id)
                        .SumAsync(e => e.Amount);

                    var currentMonthName = DateTimeFormatInfo.CurrentInfo.MonthNames[month.Month - 1];
                    result.Monthes.Add(char.ToUpper(currentMonthName[0]) + currentMonthName.Substring(1).ToLower());
                    result.Values.Add(monthIncomes == 0 ? -1 : monthIncomes);
                }

                result.Monthes.Reverse();
                result.Values.Reverse();
                
                serviceResponse.Data = result;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> GetTotalDiff(int userId)
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                var incomesSum = _context.Incomes
                    .Where(i => i.UserId == userId)
                    .Select(i => i.Amount)
                    .Sum();
                var expensesSum = _context.Expenses
                    .Where(e => e.UserId == userId)
                    .Select(e => e.Amount)
                    .Sum();
                serviceResponse.Data = incomesSum - expensesSum;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
