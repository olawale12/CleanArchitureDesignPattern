using Microsoft.AspNetCore.Mvc;
using SmartCleanArchitecture.Api.Filters;
using SmartCleanArchitecture.Api.Middlewares;
using SmartCleanArchitecture.Application;
using SmartCleanArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add services to the container.

// add layer dependency
builder.Services.AddScoped(typeof(DecryptRequestDataFilter<>));
object value = builder.Services.ApplicationServices(builder.Configuration);
builder.Services.InfrastructureServices(builder.Configuration);

builder.Services.AddControllers(options => options.Filters.Add(new ApiExceptionFilterAttribute()));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<EncryptResponseDataMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
