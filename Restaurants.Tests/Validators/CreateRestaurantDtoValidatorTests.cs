using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Tests.Validators;

public class CreateRestaurantDtoValidatorTests
{
    private readonly CreateRestaurantDtoValidator _validator = new();

    [Theory]
    [InlineData("AB")]
    [InlineData("A restaurant name that is way too long to be valid")]
    public void Validate_NameOutOfRange_ReturnsError(string name)
    {
        var command = ValidCommand();
        command.Name = name;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("not-an-email")]
    public void Validate_InvalidContactEmail_ReturnsError(string email)
    {
        var command = ValidCommand();
        command.ContactEmail = email;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.ContactEmail);
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("123456")]
    public void Validate_InvalidPostalCode_ReturnsError(string postalCode)
    {
        var command = ValidCommand();
        command.PostalCode = postalCode;

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.PostalCode);
    }

    [Fact]
    public void Validate_ValidCommand_ReturnsNoErrors()
    {
        var result = _validator.TestValidate(ValidCommand());

        result.ShouldNotHaveAnyValidationErrors();
    }

    private static CreateRestaurantCommand ValidCommand() => new()
    {
        Name = "KFC",
        Description = "Fried chicken fast food chain",
        Category = "Fast Food",
        ContactEmail = "contact@kfc.com",
        PostalCode = "12-345"
    };
}
