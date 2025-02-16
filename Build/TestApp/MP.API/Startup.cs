using System.Text.Json.Serialization;
using MP.API.Extensions;
using MP.API.Middleware;
using MP.BLL.UseCases;
using MP.Domain.Object;

namespace MP.API;

public sealed class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(json => json.JsonSerializerOptions.Converters.Insert(0, new JsonStringEnumConverter()));
        
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        
        services.AddSingleton<IManager>(new TicketManager("default_context"));
        
        services.ConfigureSwagger();
        
        services.RegisterUseCases();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("AllowAllOrigins");
        
        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionsHandlingMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}