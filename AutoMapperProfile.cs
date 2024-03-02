using AutoMapper;
using plusminus.Dtos.CategoryExpenses;
using plusminus.Dtos.CategoryIncomes;
using plusminus.Dtos.Expenses;
using plusminus.Dtos.Incomes;
using plusminus.Models;

namespace plusminus
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Expenses, GetExpensesDto>().ForMember("CategoryName", cn => cn.MapFrom(e => e.Category.Name));
            CreateMap<AddExpensesDto, Expenses>();
            CreateMap<UpdateExpensesDto, Expenses>();

            CreateMap<Incomes, GetIncomesDto>();
            CreateMap<AddIncomesDto, Incomes>();
            CreateMap<UpdateIncomesDto, Incomes>();

            CreateMap<CategoryIncomes, GetCategoryIncomesDto>();
            CreateMap<AddCategoryIncomesDto, CategoryIncomes>();
            CreateMap<UpdateCategoryIncomesDto, CategoryIncomes>();

            CreateMap<CategoryExpenses, GetCategoryExpansesDto>();
            CreateMap<AddCategoryExpansesDto, CategoryExpenses>();
            CreateMap<UpdateCategoryExpansesDto, CategoryExpenses>();
        }
    }
}
