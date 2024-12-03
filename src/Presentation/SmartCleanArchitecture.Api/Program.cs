using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SmartCleanArchitecture.Api.Filters;
using SmartCleanArchitecture.Api.Middlewares;
using SmartCleanArchitecture.Application;
using SmartCleanArchitecture.Application.Common.Interfaces;
using SmartCleanArchitecture.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add services to the container.
builder.Services.AddScoped<LanguageFilter>();
// add layer dependency
builder.Services.AddScoped(typeof(DecryptRequestDataFilter<>));
object value = builder.Services.ApplicationServices(builder.Configuration);
builder.Services.InfrastructureServices(builder.Configuration);

builder.Services.AddControllers(options => options.Filters.Add(new ApiExceptionFilterAttribute()));




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Change the title to service title",
        Description = "Service description",
        Version = "v1"
    });


    s.OperationFilter<SwaggerHeaderFilter>();
    s.AddSecurityDefinition("LanguageCode", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Language Code format : en for Engilish",
        Name = "LanguageCode",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "LanguageCode"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement() {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "LanguageCode"
                    },
                    Scheme = "LanguageCode",
                    Name = "LanguageCode",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

});

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
