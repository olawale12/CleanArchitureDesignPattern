using SmartCleanArchitecture.Application;
using SmartCleanArchitecture.Infrastructure;
using SmartCleanArchitecture.Infrastructure.IRepository;
using SmartCleanArchitecture.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add layer dependency
//builder.Services.AddSingleton<IGenericRepository<T>, GenericRepository<T>>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
object value = builder.Services.ApplicationServices(builder.Configuration);
builder.Services.InfrastructureServices(builder.Configuration);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
