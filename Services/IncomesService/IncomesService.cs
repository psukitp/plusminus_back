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
                serviceResponse.Data = dbIncomes.Select(i => _mapper.Map<Incomes, GetIncomesDto>(i)).ToList();
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetIncomesDto>>> DeleteIncomesById(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetIncomesDto>>();
            try
            {
                var incomes = await _context.Incomes.FindAsync(id);
                if (incomes is null) throw new Exception("Данные доходы не были найдены");

                _context.Incomes.Remove(incomes);
                await _context.SaveChangesAsync();

                var dbIncomes = await _context.Incomes.ToListAsync();
                serviceResponse.Data = dbIncomes.Select(i => _mapper.Map<Incomes, GetIncomesDto>(i)).ToList();
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

        public async Task<ServiceResponse<GetIncomesDto>> UpdateIncomes(UpdateIncomesDto updatedIncomes)
        {
            var serviceResponse = new ServiceResponse<GetIncomesDto>();
            try
            {
                var incomes = await _context.Incomes.FindAsync(updatedIncomes.Id);
                if (incomes is null) throw new Exception("Не удалось найти данные доходы");

                incomes.Date = updatedIncomes.Date;
                incomes.Amount = updatedIncomes.Amount;
                incomes.CategoryId = updatedIncomes.CategoryId;

                await _context.SaveChangesAsync();

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
    }
}
