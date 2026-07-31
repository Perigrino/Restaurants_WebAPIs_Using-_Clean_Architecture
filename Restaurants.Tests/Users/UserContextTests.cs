using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Restaurants.Application.Users;

namespace Restaurants.Tests.Users;

public class UserContextTests
{
    [Fact]
    public void GetCurrentUser_ParsesClaimsCorrectly()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, "user-123"),
            new Claim(ClaimTypes.Email, "user@test.com"),
            new Claim(ClaimTypes.Role, "Owner"),
            new Claim(ClaimTypes.Role, "Administrator"),
            new Claim("Nationality", "Ghanaian"),
            new Claim("DateOfBirth", "1995-05-10")
        ], "Test"));

        var userContext = new UserContext(new HttpContextAccessor { HttpContext = new DefaultHttpContext { User = principal } });

        var currentUser = userContext.GetCurrentUser();

        Assert.NotNull(currentUser);
        Assert.Equal("user-123", currentUser.Id);
        Assert.Equal("user@test.com", currentUser.Email);
        Assert.Equal("Ghanaian", currentUser.Nationality);
        Assert.Equal(new DateOnly(1995, 5, 10), currentUser.DateOfBirth);
        Assert.Contains("Owner", currentUser.Roles);
        Assert.True(currentUser.IsInRole("Administrator"));
    }

    [Fact]
    public void GetCurrentUser_NoNationalityOrDateOfBirth_ReturnsNulls()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, "user-123"),
            new Claim(ClaimTypes.Email, "user@test.com")
        ], "Test"));

        var userContext = new UserContext(new HttpContextAccessor { HttpContext = new DefaultHttpContext { User = principal } });

        var currentUser = userContext.GetCurrentUser();

        Assert.NotNull(currentUser);
        Assert.Null(currentUser.Nationality);
        Assert.Null(currentUser.DateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_Unauthenticated_ReturnsNull()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity());
        var userContext = new UserContext(new HttpContextAccessor { HttpContext = new DefaultHttpContext { User = principal } });

        var currentUser = userContext.GetCurrentUser();

        Assert.Null(currentUser);
    }

    [Fact]
    public void GetCurrentUser_NoHttpContext_Throws()
    {
        var userContext = new UserContext(new HttpContextAccessor());

        Assert.Throws<InvalidOperationException>(() => userContext.GetCurrentUser());
    }
}
