using MediatR;

namespace Restaurants.Application.Users.Commands.AssignRole;

public class AssignRoleCommand : IRequest
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}