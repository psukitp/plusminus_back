using Microsoft.EntityFrameworkCore;
using plusminus.Data;
using plusminus.Services.CategoryExpansesService;
using plusminus.Services.CategoryIncomesService;
using plusminus.Services.ExpensesService;
using plusminus.Services.IncomesService;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddScoped<IIncomesService, IncomesService>();
builder.Services.AddScoped<ICategoryIncomesService, CategoryIncomesService>();
builder.Services.AddScoped<ICategoryExpansesService, CategoryExpansesService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
