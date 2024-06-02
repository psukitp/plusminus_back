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
        public async Task<ServiceResponse<GetCategoryExpansesDto>> AddCategoryExpanses(AddCategoryExpansesDto newCategoryExpanses, int userId)
        {
            var serviceResponse = new ServiceResponse<GetCategoryExpansesDto>();
            try
            {
                var expansesCategory = _mapper.Map<AddCategoryExpansesDto, CategoryExpenses>(newCategoryExpanses);
                expansesCategory.UserId = userId;
                var addedExpanses = await _context.CategoryExpenses.AddAsync(expansesCategory);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data = _mapper.Map<CategoryExpenses, GetCategoryExpansesDto>(addedExpanses.Entity);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> DeleteCategoryExpansesById(int id, int userId)
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                var expansesCategory = await _context.CategoryExpenses.FindAsync(id);
                if (expansesCategory is null || expansesCategory.UserId != userId) throw new Exception("Категория не найдена");

                var currentCategoryId = expansesCategory.Id;

                _context.CategoryExpenses.Remove(expansesCategory);
                await _context.SaveChangesAsync();
                
                serviceResponse.Data = currentCategoryId;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCategoryExpansesDto>> UpdateCategoryExpanses(UpdateCategoryExpansesDto updatedCategoryExpanses, int userId)
        {
            var serviceResponse = new ServiceResponse<GetCategoryExpansesDto>();
            try
            {
                var expansesCategory = await _context.CategoryExpenses.FindAsync(updatedCategoryExpanses.Id);
                if (expansesCategory is null || expansesCategory.UserId != userId) throw new Exception("Категория не найдена");

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

        public async Task<ServiceResponse<List<GetCategoryExpansesDto>>> GetAllCategories(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetCategoryExpansesDto>>();
            try
            {
                var dbCategoryExpanses = await _context.CategoryExpenses.ToListAsync();
                var categories = dbCategoryExpanses.Where(c => c.UserId == userId).ToList();
                serviceResponse.Data = categories.Select(ci => _mapper.Map<CategoryExpenses, GetCategoryExpansesDto>(ci)).ToList();
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
