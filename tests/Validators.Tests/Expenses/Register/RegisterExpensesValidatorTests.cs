using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpensesValidatorTests
{
    [Fact]
    public void Success()
    {
        // Arrange - Configura as instancias de tudo que precisa para realizar o teste
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        
        // Act - Ação de validar os dados da requisção
        var result = validator.Validate(request);

        // Assert - Compara a ação com o resultado esperado
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        // Arrange - Configura as instancias de tudo que precisa para realizar o teste
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = title;

        // Act - Ação de validar os dados da requisção
        var result = validator.Validate(request);

        // Assert - Compara a ação com o resultado esperado
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void Error_Date_Future()
    {
        // Arrange - Configura as instancias de tudo que precisa para realizar o teste
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);

        // Act - Ação de validar os dados da requisção
        var result = validator.Validate(request);

        // Assert - Compara a ação com o resultado esperado
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.DATE_CANNOT_FOR_THE_FUTURE));
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        // Arrange - Configura as instancias de tudo que precisa para realizar o teste
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)700;

        // Act - Ação de validar os dados da requisção
        var result = validator.Validate(request);

        // Assert - Compara a ação com o resultado esperado
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-7)]
    public void Error_Amount_Invalid(decimal amount)
    {
        // Arrange - Configura as instancias de tudo que precisa para realizar o teste
        var validator = new RegisterExpenseValidator();
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Amount = amount;

        // Act - Ação de validar os dados da requisção
        var result = validator.Validate(request);

        // Assert - Compara a ação com o resultado esperado
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
}