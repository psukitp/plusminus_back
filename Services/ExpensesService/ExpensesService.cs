﻿using System.Globalization;
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

        public async Task<ServiceResponse<List<GetExpensesDto>>> GetExpensesByUserId(int id, DateOnly date)
        {
            var serviceResponse = new ServiceResponse<List<GetExpensesDto>>();

            try
            {
                var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();
                var dbExpenses = expenses.Where(e => e.UserId == id && e.Date == date);
                serviceResponse.Data = dbExpenses.Select(e => _mapper.Map<GetExpensesDto>(e)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetExpensesDto>>> AddExpenses(AddExpensesDto newExpenses, int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetExpensesDto>>();

            Expenses mappedExpenses = _mapper.Map<Expenses>(newExpenses);

            Expenses expensesToAdd = new Expenses
            {
                Amount = mappedExpenses.Amount,
                Category = mappedExpenses.Category,
                UserId = userId,
                Date = mappedExpenses.Date,
                CategoryId = mappedExpenses.CategoryId,
                Id = mappedExpenses.Id,
                User = mappedExpenses.User,
            };
            var addedExpenses = await _context.Expenses.AddAsync(expensesToAdd);
            await _context.SaveChangesAsync();

            var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();
            var dbExpenses = expenses.Where(e => e.UserId == userId && e.Date == expensesToAdd.Date);
            serviceResponse.Data = dbExpenses.Select(e => _mapper.Map<GetExpensesDto>(e)).ToList();
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> DeleteExpensesById(int id, int userId)
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                var expenses = await _context.Expenses.FindAsync(id);
                if (expenses is null || expenses.UserId != userId) throw new Exception("Данные расходы не были найдены");

                var currentId = expenses.Id;
                
                _context.Expenses.Remove(expenses);
                await _context.SaveChangesAsync();

                serviceResponse.Data = currentId;
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
        
        public async Task<ServiceResponse<List<ExpensesByCategory>>> GetExpansesByCategoryMonth(int userId)
        {
            var serviceResponse = new ServiceResponse<List<ExpensesByCategory>>();
            try
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();

                var dbExpenses = expenses
                    .Where(e => e.UserId == userId && e.Date.Month == currentMonth && e.Date.Year == currentYear)
                    .GroupBy(e => e.CategoryId)
                    .Select(g => new
                    {
                        categoryId = g.Key,
                        amount = g.Sum(e => e.Amount)
                    })
                    .ToList();

                var result = new List<ExpensesByCategory>();
                foreach (var expense in dbExpenses)
                {
                    var category = await _context.CategoryExpenses.FirstOrDefaultAsync(c => c.Id == expense.categoryId);
                    if (category != null)
                    {
                        result.Add(new ExpensesByCategory
                        {
                            CategoryName = category.Name,
                            Color = category.Color,
                            Amount = expense.amount
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

        public async Task<ServiceResponse<List<ExpensesByCategory>>> GetExpansesByCategory(int userId, DateOnly date)
        {
            var serviceResponse = new ServiceResponse<List<ExpensesByCategory>>();
            try
            {
                var expenses = await _context.Expenses.Include(e => e.Category).ToListAsync();

                var dbExpenses = expenses
                    .Where(e => e.UserId == userId && e.Date.Month == date.Month && e.Date.Year == date.Year)
                    .GroupBy(e => e.CategoryId)
                    .Select(g => new
                    {
                        categoryId = g.Key,
                        amount = g.Sum(e => e.Amount)
                    })
                    .ToList();

                var result = new List<ExpensesByCategory>();
                foreach (var expense in dbExpenses)
                {
                    var category = await _context.CategoryExpenses.FirstOrDefaultAsync(c => c.Id == expense.categoryId);
                    if (category != null)
                    {
                        result.Add(new ExpensesByCategory
                        {
                            Id = category.Id,
                            CategoryName = category.Name,
                            Color = category.Color,
                            Amount = expense.amount
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

        public async Task<ServiceResponse<GetExpensesDto>> UpdateExpenses(UpdateExpensesDto newExpenses, int userId)
        {
            var serviceResponse = new ServiceResponse<GetExpensesDto>();
            try
            {
                var expenses = await _context.Expenses.FindAsync(newExpenses.Id);
                if (expenses is null || expenses.UserId != userId) throw new Exception("Данные расходы не были найдены");
                
                if (newExpenses.Amount != null) expenses.Amount = newExpenses.Amount;
                
                if (newExpenses.CategoryId != null) expenses.CategoryId = newExpenses.CategoryId;

                await _context.SaveChangesAsync();

                var category = await _context.CategoryExpenses.FindAsync(expenses.CategoryId);
                expenses.Category = category;
                serviceResponse.Data = _mapper.Map<GetExpensesDto>(expenses);
            } catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<ExpensesThisMonthStat>> GetExpensesSum(int id)
        {
            var serviceResponse = new ServiceResponse<ExpensesThisMonthStat>();
            try
            {
                var currentDate = DateTime.Now;
                var expensesThisMonth = await _context.Expenses
                    .Where(e => e.Date.Month == currentDate.Month && e.Date.Year == currentDate.Year && e.UserId == id)
                    .SumAsync(e => e.Amount);

                var prevDate = currentDate.AddMonths(-1);
                var expensesPrevMonth = await _context.Expenses
                    .Where(e => e.Date.Month == prevDate.Month && e.Date.Year == prevDate.Year && e.UserId == id)
                    .SumAsync(e => e.Amount);

                ExpensesThisMonthStat result = new ExpensesThisMonthStat
                {
                    ExpensesDiff = expensesThisMonth - expensesPrevMonth,
                    ExpensesTotal = expensesThisMonth
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

        public async Task<ServiceResponse<GetThisYearExpenses>> GetExpensesThisYear(int id)
        {
            var serviceResponse = new ServiceResponse<GetThisYearExpenses>();
            try
            {
                var currentDate = DateTime.Now;
                GetThisYearExpenses result = new GetThisYearExpenses();
                result.Monthes = new List<string>();
                result.Values = new List<double>();
                
                for (var i = 0; i < currentDate.Month; i++)
                {
                    var month = currentDate.AddMonths(-i);
                    var monthExpenses = await _context.Expenses
                        .Where(e => e.Date.Month == month.Month && e.Date.Year == month.Year && e.UserId == id)
                        .SumAsync(e => e.Amount);

                    var currentMonthName = DateTimeFormatInfo.CurrentInfo.MonthNames[month.Month - 1];
                    result.Monthes.Add(char.ToUpper(currentMonthName[0]) + currentMonthName.Substring(1).ToLower());
                    result.Values.Add(monthExpenses == 0 ? -1 : monthExpenses);
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

    }
}
