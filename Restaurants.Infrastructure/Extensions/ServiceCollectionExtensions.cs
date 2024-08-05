using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeder;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); 
        services.Configure<JsonOptions>(options => { options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
        services.AddDbContext<RestaurantDbContext>(o => o.UseNpgsql(config.GetConnectionString("Default")));
        services.AddScoped<IRestaurantSeeder, RestaurantSeeders>();
        services.AddScoped<IRestaurantRepository, RestaurantsRepository>(); //RestaurantSeeders>();

        return services;
    }
}