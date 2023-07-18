using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDemo.EF;
using WebAPIDemo.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//ÄÚ´æ»º´æ
builder.Services.AddMemoryCache();

//redis»º´æ
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "test_";
});

//¹ýÂËÆ÷
builder.Services.Configure<MvcOptions>(options => {
    options.Filters.Add<RateLimitFilter>();
    options.Filters.Add<MyExceptionFilter>();
    options.Filters.Add<LogExceptionFilter>();
    options.Filters.Add<MyActionFilter1>();
    options.Filters.Add<MyActionFilter2>();
    options.Filters.Add<TransactionScopeFilter>();
});

builder.Services.AddDbContext<MyDbContext>(options =>
{
    string? connectStr = builder.Configuration.GetConnectionString("LocalDb");
    options.UseSqlServer(connectStr);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
