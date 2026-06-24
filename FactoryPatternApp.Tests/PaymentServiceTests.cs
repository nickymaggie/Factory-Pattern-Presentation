using FactoryPatternApp.Services;
using Xunit;

namespace FactoryPatternApp.Tests;

/// <summary>
/// Unit tests for concrete payment service implementations.
/// </summary>
public class PaymentServiceTests
{
    [Fact]
    public void CreditCardPaymentService_GetPaymentMethodName_ReturnsCorrectName()
    {
        // Arrange
        var service = new CreditCardPaymentService();

        // Act
        var name = service.GetPaymentMethodName();

        // Assert
        Assert.Equal("Credit Card", name);
    }

    [Fact]
    public void PayPalPaymentService_GetPaymentMethodName_ReturnsCorrectName()
    {
        // Arrange
        var service = new PayPalPaymentService();

        // Act
        var name = service.GetPaymentMethodName();

        // Assert
        Assert.Equal("PayPal", name);
    }

    [Fact]
    public void BankTransferPaymentService_GetPaymentMethodName_ReturnsCorrectName()
    {
        // Arrange
        var service = new BankTransferPaymentService();

        // Act
        var name = service.GetPaymentMethodName();

        // Assert
        Assert.Equal("Bank Transfer", name);
    }

    [Fact]
    public void ApplePayPaymentService_GetPaymentMethodName_ReturnsCorrectName()
    {
        // Arrange
        var service = new ApplePayPaymentService();

        // Act
        var name = service.GetPaymentMethodName();

        // Assert
        Assert.Equal("Apple Pay", name);
    }

    [Fact]
    public void GooglePayPaymentService_GetPaymentMethodName_ReturnsCorrectName()
    {
        // Arrange
        var service = new GooglePayPaymentService();

        // Act
        var name = service.GetPaymentMethodName();

        // Assert
        Assert.Equal("Google Pay", name);
    }

    [Fact]
    public void PaymentServices_AllImplementIPaymentService()
    {
        // Arrange
        var services = new IPaymentService[]
        {
            new CreditCardPaymentService(),
            new PayPalPaymentService(),
            new BankTransferPaymentService(),
            new ApplePayPaymentService(),
            new GooglePayPaymentService()
        };

        // Act & Assert
        foreach (var service in services)
        {
            Assert.IsAssignableFrom<IPaymentService>(service);
        }
    }
}
