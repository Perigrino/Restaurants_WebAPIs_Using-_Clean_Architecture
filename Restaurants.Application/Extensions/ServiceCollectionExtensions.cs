using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Users;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        
        services.AddMediatR(config => config.RegisterServicesFromAssembly(applicationAssembly));
        services.AddAutoMapper(applicationAssembly);
        services.AddValidatorsFromAssembly(applicationAssembly).AddFluentValidationAutoValidation();

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

    }
}