using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Restaurants.Application.Extensions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Tests.Mappings;

public class ProfilesTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(
            cfg => cfg.AddMaps(typeof(ServiceCollectionExtensions).Assembly),
            NullLoggerFactory.Instance);
        return config.CreateMapper();
    }

    [Fact]
    public void MapperConfiguration_IsValid()
    {
        var mapper = CreateMapper();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void CreateRestaurantCommand_ToRestaurant_MapsAddress()
    {
        var mapper = CreateMapper();
        var command = new CreateRestaurantCommand
        {
            Name = "KFC",
            Description = "Fried chicken",
            Category = "Fast Food",
            City = "London",
            Street = "Cork St",
            PostalCode = "12-345"
        };

        var restaurant = mapper.Map<Restaurant>(command);

        Assert.Equal("London", restaurant.Address!.City);
        Assert.Equal("Cork St", restaurant.Address.Street);
        Assert.Equal("12-345", restaurant.Address.PostalCode);
    }

    [Fact]
    public void UpdateRestaurantCommand_MapOntoExisting_UpdatesPropertiesAndAddress()
    {
        var mapper = CreateMapper();
        var restaurant = new Restaurant
        {
            Id = Guid.NewGuid(),
            Name = "Old name",
            Description = "Old description",
            Category = "Old category",
            Address = new Address { City = "Paris", Street = "Rue X", PostalCode = "75-000" }
        };
        var command = new UpdateRestaurantCommand
        {
            Name = "New name",
            Description = "New description",
            Category = "New category",
            City = "London",
            Street = "Cork St",
            PostalCode = "11-111"
        };

        mapper.Map(command, restaurant);

        Assert.Equal("New name", restaurant.Name);
        Assert.Equal("London", restaurant.Address!.City);
        Assert.Equal("Cork St", restaurant.Address.Street);
    }
}
