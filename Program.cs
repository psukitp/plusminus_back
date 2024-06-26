using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using plusminus.Data;
using plusminus.Middlewares;
using plusminus.Services.CategoryExpansesService;
using plusminus.Services.CategoryIncomesService;
using plusminus.Services.ExpensesService;
using plusminus.Services.IncomesService;
using plusminus.Services.UsersService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "plusminusApi", Version = "v1" });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            //options.Cookie.Domain = "plusminus-app.ru";
            options.Cookie.Name = ".AspNetCore.Cookies";
            options.Cookie.HttpOnly = true; 
            options.ExpireTimeSpan = TimeSpan.FromDays(14); 
            options.SlidingExpiration = true; 
        });

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthorizeFilter>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddScoped<IIncomesService, IncomesService>();
builder.Services.AddScoped<ICategoryIncomesService, CategoryIncomesService>();
builder.Services.AddScoped<ICategoryExpensesService, CategoryExpensesService>();
builder.Services.AddScoped<IUsersService, UsersService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder =>
{
    //TODO �������� ��� ����������, �������� �� ���������� ���������
    builder.AllowCredentials();
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.SetIsOriginAllowed(origin => true) ;
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
