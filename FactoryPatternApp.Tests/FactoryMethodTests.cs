using FactoryPatternApp.Factories;
using FactoryPatternApp.Services;
using Xunit;

namespace FactoryPatternApp.Tests;

/// <summary>
/// Unit tests for individual factory implementations demonstrating the Factory Method pattern.
/// </summary>
public class FactoryMethodTests
{
    [Fact]
    public void CreditCardFactory_Create_ReturnsCreditCardPaymentService()
    {
        // Arrange
        var factory = new CreditCardFactory();

        // Act
        IPaymentService result = factory.Create();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CreditCardPaymentService>(result);
    }

    [Fact]
    public void PayPalFactory_Create_ReturnsPayPalPaymentService()
    {
        // Arrange
        var factory = new PayPalFactory();

        // Act
        IPaymentService result = factory.Create();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PayPalPaymentService>(result);
    }

    [Fact]
    public void BankTransferFactory_Create_ReturnsBankTransferPaymentService()
    {
        // Arrange
        var factory = new BankTransferFactory();

        // Act
        IPaymentService result = factory.Create();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BankTransferPaymentService>(result);
    }

    [Fact]
    public void FactoryMethod_EachCreatesUniqueInstance()
    {
        // Arrange
        var creditCardFactory = new CreditCardFactory();
        var payPalFactory = new PayPalFactory();

        // Act
        var service1 = creditCardFactory.Create();
        var service2 = payPalFactory.Create();

        // Assert
        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.IsType<CreditCardPaymentService>(service1);
        Assert.IsType<PayPalPaymentService>(service2);
        Assert.NotEqual(service1.GetType(), service2.GetType());
    }
}
