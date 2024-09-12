using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private readonly int[] _allowedPageSizes = [5, 10, 15, 20, 25, 30, 35, 40, 45];
    private readonly string[] _allowedSortByColumnNames = [nameof(RestaurantDto.Name), nameof(RestaurantDto.Category), nameof(RestaurantDto.Description)];
    
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
        
        RuleFor(r => r.PageSize).Must(value => _allowedPageSizes
            .Contains(value))
            .WithMessage($"The page size must be in [{string.Join(",", _allowedPageSizes)}]");
        
        RuleFor(r => r.SortBy)
            .Must(value => _allowedSortByColumnNames.Contains(value))
            .When(q=>q.SortBy != null)
            .WithMessage($"Sort by is optional or must be one of [{string.Join(",", _allowedSortByColumnNames)}]");
    }
}