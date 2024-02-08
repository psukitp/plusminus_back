using AutoMapper;
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
            CreateMap<Expenses, GetExpensesDto>();
            CreateMap<AddExpensesDto, Expenses>();
            CreateMap<UpdateExpensesDto, Expenses>();

            CreateMap<Incomes, GetIncomesDto>();
            CreateMap<AddIncomesDto, Incomes>();
            CreateMap<UpdateIncomesDto, Incomes>();

            CreateMap<CategoryIncomes, GetCategoryIncomesDto>();
            CreateMap<AddCategoryIncomesDto, CategoryIncomes>();
            CreateMap<UpdateCategoryIncomesDto, CategoryIncomes>();
        }
    }
}
