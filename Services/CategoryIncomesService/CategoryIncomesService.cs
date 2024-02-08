using AutoMapper;
using Microsoft.EntityFrameworkCore;
using plusminus.Data;
using plusminus.Dtos.CategoryIncomes;
using plusminus.Models;

namespace plusminus.Services.CategoryIncomesService
{
    public class CategoryIncomesService : ICategoryIncomesService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CategoryIncomesService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<List<GetCategoryIncomesDto>>> AddCategoryIncomes(AddCategoryIncomesDto newCategoryIncomes)
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryIncomesDto>>();
            try
            {
                var incomesCat = _mapper.Map<AddCategoryIncomesDto, CategoryIncomes>(newCategoryIncomes);
                var addedIncomes = await _context.CategoryIncomes.AddAsync(incomesCat);
                await _context.SaveChangesAsync();

                var dbCategoryIncomes = await _context.CategoryIncomes.ToListAsync();
                serviceResponse.Data = dbCategoryIncomes.Select(ci => _mapper.Map<CategoryIncomes, GetCategoryIncomesDto>(ci)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCategoryIncomesDto>>> DeleteCategoryIncomesById(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryIncomesDto>>();
            try
            {
                var categoryIncomes = await _context.CategoryIncomes.FindAsync(id);
                if (categoryIncomes is null) throw new Exception("Категория не найдена");

                _context.CategoryIncomes.Remove(categoryIncomes);
                await _context.SaveChangesAsync();

                var dbCategoryIncomes = await _context.CategoryIncomes.ToListAsync();
                serviceResponse.Data = dbCategoryIncomes.Select(i => _mapper.Map<CategoryIncomes, GetCategoryIncomesDto>(i)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryIncomesDto>> UpdateCategoryIncomes(UpdateCategoryIncomesDto updatedCategoryIncomes)
        {
            var serviceResponse = new ServiceResponse<GetCategoryIncomesDto>();
            try
            {
                var categoryIncomes = await _context.CategoryIncomes.FindAsync(updatedCategoryIncomes.Id);
                if (categoryIncomes is null) throw new Exception("Категория не найдена");

                categoryIncomes.Name = updatedCategoryIncomes.Name;
                categoryIncomes.Color = updatedCategoryIncomes.Color;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<CategoryIncomes, GetCategoryIncomesDto>(categoryIncomes);
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
