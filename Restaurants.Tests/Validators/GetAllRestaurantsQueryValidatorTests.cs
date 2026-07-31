using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

namespace Restaurants.Tests.Validators;

public class GetAllRestaurantsQueryValidatorTests
{
    private readonly GetAllRestaurantsQueryValidator _validator = new();

    [Fact]
    public void Validate_ValidQuery_ReturnsNoErrors()
    {
        var query = new GetAllRestaurantsQuery { PageNumber = 1, PageSize = 10, SortBy = "Name" };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidPageNumber_ReturnsError(int pageNumber)
    {
        var query = new GetAllRestaurantsQuery { PageNumber = pageNumber, PageSize = 10 };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.PageNumber);
    }

    [Theory]
    [InlineData(7)]
    [InlineData(11)]
    [InlineData(100)]
    public void Validate_PageSizeNotAllowed_ReturnsError(int pageSize)
    {
        var query = new GetAllRestaurantsQuery { PageNumber = 1, PageSize = pageSize };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.PageSize);
    }

    [Fact]
    public void Validate_InvalidSortBy_ReturnsError()
    {
        var query = new GetAllRestaurantsQuery { PageNumber = 1, PageSize = 10, SortBy = "InvalidColumn" };

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.SortBy);
    }

    [Fact]
    public void Validate_NullSortBy_ReturnsNoErrors()
    {
        var query = new GetAllRestaurantsQuery { PageNumber = 1, PageSize = 10, SortBy = null };

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
