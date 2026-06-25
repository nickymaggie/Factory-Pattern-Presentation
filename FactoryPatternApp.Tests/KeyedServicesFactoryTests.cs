using FactoryPatternApp.Factories;
using FactoryPatternApp.Models;
using FactoryPatternApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace FactoryPatternApp.Tests;

/// <summary>
/// Unit tests for the KeyedServicePaymentFactory demonstrating the .NET 8
/// keyed-services Factory backed by Dependency Injection.
/// </summary>
public class KeyedServicesFactoryTests
{
    private static IPaymentFactory BuildFactory()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddPaymentServices()
            .BuildServiceProvider();

        return provider.GetRequiredService<IPaymentFactory>();
    }

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
        IPaymentFactory factory = BuildFactory();

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
        IPaymentFactory factory = BuildFactory();
        var invalidPaymentMethod = (PaymentMethod)999;

        // Act & Assert
        var exception = Assert.Throws<NotSupportedException>(
            () => factory.Create(invalidPaymentMethod));
        Assert.Contains("not supported", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new KeyedServicePaymentFactory(null!));
    }

    [Fact]
    public void Create_IsTransient_CreatesNewInstanceEachTime()
    {
        // Arrange
        IPaymentFactory factory = BuildFactory();

        // Act
        var service1 = factory.Create(PaymentMethod.CreditCard);
        var service2 = factory.Create(PaymentMethod.CreditCard);

        // Assert
        Assert.NotSame(service1, service2);
    }
}
