using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorisation;
using Restaurants.Infrastructure.Authorisation.Requirements;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeder;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureService(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); 
        services.Configure<JsonOptions>(options => { options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });

        services.AddIdentityApiEndpoints<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantDbContext>();
            

        var connectionString = config.GetConnectionString("Default");
        
        services.AddDbContext<RestaurantDbContext>(o => o.UseNpgsql(connectionString).EnableSensitiveDataLogging());
        services.AddScoped<IRestaurantSeeder, RestaurantSeeders>();
        services.AddScoped<IDishRepository, DishRepository>();
        services.AddScoped<IRestaurantRepository, RestaurantsRepository>(); //RestaurantSeeders>();
        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasNationality,
                builder => builder.RequireClaim(AppClaimTypes.Nationality, "Ghanaian", "Brazilian"))
            .AddPolicy(PolicyNames.AtLeast20,
                builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
            //.AddPolicy(PolicyNames.CreatedAtleast2Restaurants, 
             //   builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));
        
        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        // services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>();
        // services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
    }
}