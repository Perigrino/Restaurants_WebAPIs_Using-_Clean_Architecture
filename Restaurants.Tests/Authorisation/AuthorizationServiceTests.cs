using Microsoft.Extensions.Logging.Abstractions;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;
using Restaurants.Infrastructure.Authorisation.Services;

namespace Restaurants.Tests.Authorisation;

public class AuthorizationServiceTests
{
    [Fact]
    public void Authorise_OwnerUpdatingOwnRestaurant_ReturnsTrue()
    {
        var ownerId = Guid.NewGuid().ToString();
        var service = CreateService(new FakeUserContext(ownerId, "Owner"));
        var restaurant = new Restaurant { Id = Guid.NewGuid(), OwnerId = ownerId };

        var result = service.Authorise(restaurant, ResourceOperation.Update);

        Assert.True(result);
    }

    [Fact]
    public void Authorise_NonOwnerUpdatingRestaurant_ReturnsFalse()
    {
        var service = CreateService(new FakeUserContext(Guid.NewGuid().ToString(), "User"));
        var restaurant = new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString() };

        var result = service.Authorise(restaurant, ResourceOperation.Update);

        Assert.False(result);
    }

    [Fact]
    public void Authorise_AdministratorDeletingAnyRestaurant_ReturnsTrue()
    {
        var service = CreateService(new FakeUserContext(Guid.NewGuid().ToString(), "Administrator"));
        var restaurant = new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString() };

        var result = service.Authorise(restaurant, ResourceOperation.Delete);

        Assert.True(result);
    }

    [Fact]
    public void Authorise_ReadOperation_AlwaysAllowed()
    {
        var service = CreateService(new FakeUserContext(Guid.NewGuid().ToString(), "User"));
        var restaurant = new Restaurant { Id = Guid.NewGuid(), OwnerId = Guid.NewGuid().ToString() };

        var result = service.Authorise(restaurant, ResourceOperation.Read);

        Assert.True(result);
    }

    private static IAuthorizationService CreateService(IUserContext userContext) =>
        new AuthorizationService(NullLogger<AuthorizationService>.Instance, userContext);

    private sealed class FakeUserContext(string userId, params string[] roles) : IUserContext
    {
        public CurrentUser? GetCurrentUser() => new(userId, $"{userId}@test.com", roles, null, null);
    }
}
