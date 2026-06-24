using FactoryPatternApp.Factories;
using FactoryPatternApp.Models;
using FactoryPatternApp.Processors;
using FactoryPatternApp.Services;
using Xunit;

namespace FactoryPatternApp.Tests;

/// <summary>
/// Unit tests for the SimplePaymentServiceFactory demonstrating the Simple Factory pattern.
/// </summary>
public class SimpleFactoryTests
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
        // Arrange & Act
        IPaymentService result = SimplePaymentServiceFactory.Create(paymentMethod);

        // Assert
        Assert.NotNull(result);
        Assert.IsType(expectedServiceType, result);
    }

    [Fact]
    public void Create_WithInvalidPaymentMethod_ThrowsNotSupportedException()
    {
        // Arrange
        var invalidPaymentMethod = (PaymentMethod)999;

        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(
            () => SimplePaymentServiceFactory.Create(invalidPaymentMethod));
        Assert.Contains("not supported", exception.Message);
    }
}
