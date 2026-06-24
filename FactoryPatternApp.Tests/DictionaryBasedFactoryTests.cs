using FactoryPatternApp.Factories;
using FactoryPatternApp.Models;
using FactoryPatternApp.Services;
using Xunit;

namespace FactoryPatternApp.Tests;

/// <summary>
/// Unit tests for the DictionaryBasedPaymentFactory demonstrating the enterprise-level Factory pattern.
/// </summary>
public class DictionaryBasedFactoryTests
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
        var factory = new DictionaryBasedPaymentFactory();

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
        var factory = new DictionaryBasedPaymentFactory();
        var invalidPaymentMethod = (PaymentMethod)999;

        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(
            () => factory.Create(invalidPaymentMethod));
        Assert.Contains("Unsupported", exception.Message);
    }

    [Fact]
    public void Create_AllPaymentMethods_AreSupported()
    {
        // Arrange
        var factory = new DictionaryBasedPaymentFactory();
        var paymentMethods = new[]
        {
            PaymentMethod.CreditCard,
            PaymentMethod.PayPal,
            PaymentMethod.BankTransfer,
            PaymentMethod.ApplePay,
            PaymentMethod.GooglePay
        };

        // Act & Assert
        foreach (var method in paymentMethods)
        {
            var result = factory.Create(method);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IPaymentService>(result);
        }
    }
}
