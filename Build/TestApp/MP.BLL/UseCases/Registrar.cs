using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MP.BLL.UseCases.AddTicket;

namespace MP.BLL.UseCases;

public static class Registrar
{
    public static IServiceCollection RegisterUseCases(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Registrar).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        services.AddScoped<IValidator<AddTicketUseCase.Command>, AddTicketUseCase.CommandValidator>();
        
        return services;
    }
}