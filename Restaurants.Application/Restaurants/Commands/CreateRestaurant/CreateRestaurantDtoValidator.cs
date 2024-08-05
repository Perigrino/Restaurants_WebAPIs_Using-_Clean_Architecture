using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantCommand>
{
    public CreateRestaurantDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 25);
        
        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is required");
        
        RuleFor(dto => dto.Category)
            .NotEmpty()
            .WithMessage("Insert Category");
        
        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Provide a valid email address");
        
        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX)");
    }
}