using FluentAssertions;
using JetBrains.Annotations;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Xunit;

namespace Restaurants.Application.Test.Users;

[TestSubject(typeof(CurrentUser))]
public class CurrentUserTest
{

    [Theory]
    [InlineData(UserRoles.Administrator)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string roleName)
    {
        var currentUser = new CurrentUser("1", "owner@test.com", [UserRoles.Administrator, UserRoles.User], null, null);

        var isInRole = currentUser.IsInRole(roleName);
        
        isInRole.Should().BeTrue();
    }
    
    
    [Fact]
    public void IsInRole_WithNoMatchingRole_ShouldReturnFalse()
    {
        var currentUser = new CurrentUser("1", "owner@test.com", [UserRoles.Administrator, UserRoles.User], null, null);

        var isInRole = currentUser.IsInRole(UserRoles.Owner);
        
        isInRole.Should().BeFalse();
    }
    
    [Fact]
    public void IsInRole_WithMatchingRoleCase_ShouldReturnFalse()
    {
        var currentUser = new CurrentUser("1", "owner@test.com", [UserRoles.Administrator, UserRoles.User], null, null);

        var isInRole = currentUser.IsInRole(UserRoles.Administrator.ToLower());
        
        isInRole.Should().BeFalse();
    }
}