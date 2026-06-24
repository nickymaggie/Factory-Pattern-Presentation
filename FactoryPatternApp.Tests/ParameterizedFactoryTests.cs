using FactoryPatternApp.Factories;
using FactoryPatternApp.Models;
using FactoryPatternApp.Services;
using Xunit;

namespace FactoryPatternApp.Tests;

/// <summary>
/// Unit tests for the PaymentFactory demonstrating the Parameterized Factory pattern.
/// </summary>
public class ParameterizedFactoryTests
{
    [Theory]
    [InlineData(PaymentMethod.CreditCard, typeof(CreditCardPaymentService))]
    [InlineData(PaymentMethod.PayPal, typeof(PayPalPaymentService))]
    [InlineData(PaymentMethod.BankTransfer, typeof(BankTransferPaymentService))]
    [InlineData(PaymentMethod.ApplePay, typeof(ApplePayPaymentService))]
    [InlineData(PaymentMethod.GooglePay, typeof(GooglePayPaymentService))]
    public void Create_WithValidPaymentMethod_ReturnsCorrectServiceType(
        PaymentMethod paymentMethod,
        Type expectedServiceType)
    {
        // Arrange
        var factory = new PaymentFactory();

        // Act
        IPaymentService result = factory.Create(paymentMethod);

        // Assert
        Assert.NotNull(result);
        Assert.IsType(expectedServiceType, result);
    }

    [Fact]
    public void Create_WithInvalidPaymentMethod_ThrowsNotSupportedException()
    {
        // Arrange
        var factory = new PaymentFactory();
        var invalidPaymentMethod = (PaymentMethod)999;

        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(
            () => factory.Create(invalidPaymentMethod));
        Assert.Contains("not supported", exception.Message);
    }

    [Fact]
    public void Create_MultipleTimes_CreatesNewInstanceEachTime()
    {
        // Arrange
        var factory = new PaymentFactory();

        // Act
        var service1 = factory.Create(PaymentMethod.CreditCard);
        var service2 = factory.Create(PaymentMethod.CreditCard);

        // Assert
        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.NotSame(service1, service2);
    }
}
