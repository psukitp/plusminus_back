using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using plusminus.Data;
using plusminus.Middlewares;
using plusminus.Services.CategoryExpansesService;
using plusminus.Services.CategoryIncomesService;
using plusminus.Services.ExpensesService;
using plusminus.Services.IncomesService;
using plusminus.Services.UserSettingsService;
using plusminus.Services.UsersService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

builder.Services.AddCors();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Plusminus API", Version = "v1" });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            //options.Cookie.Domain = builder.Environment.IsDevelopment() ? null : "plusminus-app.ru";
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
builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Plusminus API V1");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(appBuilder =>
{
    appBuilder.AllowCredentials();
    appBuilder.AllowAnyHeader();
    appBuilder.AllowAnyMethod();
    appBuilder.SetIsOriginAllowed(origin => true);
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
