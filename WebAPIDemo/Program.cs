using Microsoft.AspNetCore.Authentication.JwtBearer;
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
//内存缓存
builder.Services.AddMemoryCache();

//redis缓存
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = "localhost";
    options.InstanceName = "test_";
});

//过滤器
builder.Services.Configure<MvcOptions>(options => {
    options.Filters.Add<RateLimitFilter>();
    options.Filters.Add<MyExceptionFilter>();
    options.Filters.Add<LogExceptionFilter>();
    options.Filters.Add<MyActionFilter1>();
    options.Filters.Add<MyActionFilter2>();
    options.Filters.Add<TransactionScopeFilter>();
});

//DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
{
    string? connectStr = builder.Configuration.GetConnectionString("LocalDb");
    options.UseSqlServer(connectStr);
});

//grpc
//builder.Services.AddGrpc();

//注册身份验证服务和处理程序
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
//    options=>builder.Configuration.Bind("JwtSettings",options));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//添加身份验证中间件
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//app.MapGrpcService<GreeterService>();

app.Run();
