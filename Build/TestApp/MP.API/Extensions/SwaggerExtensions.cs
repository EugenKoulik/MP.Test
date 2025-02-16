using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MP.API.Extensions;

public static class SwaggerExtensions
{
    public static void ConfigureSwagger(
        this IServiceCollection services
    )
    {
        services.AddSwaggerGen();
        
        services.Configure<SwaggerGenOptions>(
            options =>
            {
                options.DescribeAllParametersInCamelCase();
                
                options.IncludeXmlComments(
                    Path.Combine(
                        AppContext.BaseDirectory,
                        "MP.API.xml"
                    )
                );
            }
        );

        services.Configure<SwaggerOptions>(
            options => { options.RouteTemplate = "swagger/{documentName}/swagger.json"; }
        );
    }
}