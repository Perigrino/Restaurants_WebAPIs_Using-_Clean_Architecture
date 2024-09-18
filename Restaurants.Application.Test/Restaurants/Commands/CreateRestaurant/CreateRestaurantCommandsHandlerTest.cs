using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.IRepository;
using Xunit;

namespace Restaurants.Application.Test.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandsHandler))]
public class CreateRestaurantCommandsHandlerTest
{

    [Fact]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        // arrange
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandsHandler>>();
        var mapperMock = new Mock<IMapper>();

        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantRepositoryMock
            .Setup(repo => repo.CreateRestaurantAsync(It.IsAny<Restaurant>()))
            .ReturnsAsync(restaurant.Id);

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@test.com", [], null, null);
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);


        var commandHandler = new CreateRestaurantCommandsHandler(loggerMock.Object, 
            mapperMock.Object, 
            restaurantRepositoryMock.Object,
            userContextMock.Object);

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(restaurant.Id);
        restaurant.OwnerId.Should().Be("owner-id");
        restaurantRepositoryMock.Verify(r => r.CreateRestaurantAsync(restaurant), Times.Once);
    }
}