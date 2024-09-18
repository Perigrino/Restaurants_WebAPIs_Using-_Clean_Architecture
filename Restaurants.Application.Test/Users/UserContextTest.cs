using System;
using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Test.Users;

[TestSubject(typeof(UserContext))]
public class UserContextTest
{

    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        var dateOfBirth = new DateOnly(1970, 1, 1);
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim(ClaimTypes.Role, "User"),
            new Claim("Nationality", "USA"),
            new Claim("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
        {
            User = user
        });
        
        var userContext = new UserContext(httpContextAccessorMock.Object);


        var currentUser = userContext.GetCurrentUser();

        currentUser.Should().NotBeNull();
        currentUser!.Id.Should().Be("123");
        currentUser.Roles.Should().ContainInOrder(UserRoles.Administrator, UserRoles.User);
        currentUser.Email.Should().Be("test@example.com");
        currentUser.Nationality.Should().Be("USA");
        currentUser.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WithUserContextNotPresent_ShouldReturnCurrentUser()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext) null);
        
        var userContext = new UserContext(httpContextAccessorMock.Object);
        
        Action action = () => userContext.GetCurrentUser();

        action.Should().Throw<InvalidOperationException>().WithMessage("User context is not present");
    }
    
    [Fact]
    public void GetCurrentUser_WithUnauthenticatedUser_ShouldReturnNull()
    {
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext) null);
    
        var userContext = new UserContext(httpContextAccessorMock.Object);
    
        var currentUser = userContext.GetCurrentUser();

        currentUser.Should().BeNull();
    }
    

    
}