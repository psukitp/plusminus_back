using AutoMapper;
using Microsoft.EntityFrameworkCore;
using plusminus.Data;
using plusminus.Dtos.CategoryExpenses;
using plusminus.Models;

namespace plusminus.Services.CategoryExpansesService
{
    public class CategoryExpansesService : ICategoryExpansesService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CategoryExpansesService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ServiceResponse<List<GetCategoryExpansesDto>>> AddCategoryExpanses(AddCategoryExpansesDto newCategoryExpanses)
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryExpansesDto>>();
            try
            {
                var expansesCategory = _mapper.Map<AddCategoryExpansesDto, CategoryExpenses>(newCategoryExpanses);
                var addedExpanses = await _context.CategoryExpenses.AddAsync(expansesCategory);
                await _context.SaveChangesAsync();

                var dbCategoryExpanses = await _context.CategoryExpenses.ToListAsync();
                serviceResponse.Data = dbCategoryExpanses.Select(ci => _mapper.Map<CategoryExpenses, GetCategoryExpansesDto>(ci)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCategoryExpansesDto>>> DeleteCategoryExpansesById(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryExpansesDto>>();
            try
            {
                var expansesCategory = await _context.CategoryExpenses.FindAsync(id);
                if (expansesCategory is null) throw new Exception("Категория не найдена");

                _context.CategoryExpenses.Remove(expansesCategory);
                await _context.SaveChangesAsync();

                var dbCategoryExpanses = await _context.CategoryExpenses.ToListAsync();
                serviceResponse.Data = dbCategoryExpanses.Select(ci => _mapper.Map<CategoryExpenses, GetCategoryExpansesDto>(ci)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryExpansesDto>> UpdateCategoryExpanses(UpdateCategoryExpansesDto updatedCategoryExpanses)
        {
            var serviceResponse = new ServiceResponse<GetCategoryExpansesDto>();
            try
            {
                var expansesCategory = await _context.CategoryExpenses.FindAsync(updatedCategoryExpanses.Id);
                if (expansesCategory is null) throw new Exception("Категория не найдена");

                expansesCategory.Name = updatedCategoryExpanses.Name;
                expansesCategory.Color = updatedCategoryExpanses.Color;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<CategoryExpenses, GetCategoryExpansesDto>(expansesCategory);
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
