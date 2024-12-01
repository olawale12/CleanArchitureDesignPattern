using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartCleanArchitecture.Api.Filters
{
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            if (operation == null)
            {
                operation = new OpenApiOperation()
                {
                    Parameters = new List<OpenApiParameter>()
                };
            }

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            //operation.Parameters ??= new List<OpenApiParameter>();

            //operation.Parameters.Add(new OpenApiParameter()
            //{
            //    Name = "LanguageCode",
            //    In = ParameterLocation.Header,
            //    Required = true,
            //    Schema = new OpenApiSchema { Type = "string" }
            //});
        }
    }
}
