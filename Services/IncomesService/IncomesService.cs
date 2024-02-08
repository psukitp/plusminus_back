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
        public async Task<ServiceResponse<List<GetIncomesDto>>> AddIncomes(AddIncomesDto newIncomes)
        {
            var serviceResponse = new ServiceResponse<List<GetIncomesDto>>();
            try
            {
                var incomes = _mapper.Map<AddIncomesDto, Incomes>(newIncomes);
                var addedIncomes = await _context.Incomes.AddAsync(incomes);
                await _context.SaveChangesAsync();

                var dbIncomes = await _context.Incomes.ToListAsync();
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

        public async Task<ServiceResponse<List<GetIncomesDto>>> GetIncomesByUserId(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetIncomesDto>>();
            try
            {
                var dbIncomes = await _context.Incomes.ToListAsync();
                var incomes = dbIncomes.Where(i => i.Id == id);
                if (incomes is null) throw new Exception("У вас нет таких доходов");

                serviceResponse.Data = incomes.Select(i => _mapper.Map<Incomes, GetIncomesDto>(i)).ToList();
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
    }
}
