﻿using AutoMapper;
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
            CreateMap<Expenses, GetExpensesDto>()
                .ForMember(e => e.CategoryName, 
                    cn => cn.MapFrom(e => e.Category.Name))
                .ForMember(e => e.CategoryColor, 
                cn => cn.MapFrom(e => e.Category.Color));
            CreateMap<AddExpensesDto, Expenses>();
            CreateMap<UpdateExpensesDto, Expenses>();

            CreateMap<Incomes, GetIncomesDto>()
                .ForMember(i => i.CategoryName,
                    cn => cn.MapFrom(i => i.Category.Name))
                .ForMember(i => i.CategoryColor, 
                    cn => cn.MapFrom(e => e.Category.Color));
            CreateMap<AddIncomesDto, Incomes>();
            CreateMap<UpdateIncomesDto, Incomes>();

            CreateMap<CategoryIncomes, GetCategoryIncomesDto>();
            CreateMap<AddCategoryIncomesDto, CategoryIncomes>();
            CreateMap<UpdateCategoryIncomesDto, CategoryIncomes>();

            CreateMap<CategoryExpenses, GetCategoryExpensesDto>();
            CreateMap<AddCategoryExpensesDto, CategoryExpenses>();
            CreateMap<UpdateCategoryExpensesDto, CategoryExpenses>();
        }
    }
}
