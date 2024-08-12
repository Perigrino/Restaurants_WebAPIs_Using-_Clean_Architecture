using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.UpdateDish;

public class UpdateDishDtoValidator : AbstractValidator<UpdateDishCommand>
{
    public UpdateDishDtoValidator()
    { 
        RuleFor(dto => dto.Name)
            .Length(3, 25);
        
        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is required");
        
        RuleFor(dto => dto.Price)
            .NotEmpty()
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number or empty");
        
        RuleFor(dto => dto.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCalories must be a non-negative number");
    }
}