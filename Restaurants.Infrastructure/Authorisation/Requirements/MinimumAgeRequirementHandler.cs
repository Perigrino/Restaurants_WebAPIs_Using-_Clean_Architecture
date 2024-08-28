using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Serilog;

namespace Restaurants.Infrastructure.Authorisation.Requirements;

internal class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger, IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("User: {Email}, date of birth {DoB} - Handling MinimumAgeRequirement", currentUser.Email, currentUser.DateOfBirth);

        if (currentUser.DateOfBirth == null)
        {
            logger.LogWarning("User does not have a date of birth");
            context.Fail();
            return Task.CompletedTask;
        }
        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime((DateTime.Today)))
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
        return Task.CompletedTask;
    }
}