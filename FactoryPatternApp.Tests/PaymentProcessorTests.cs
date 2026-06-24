using FactoryPatternApp.Factories;
using FactoryPatternApp.Models;
using FactoryPatternApp.Processors;
using Xunit;

namespace FactoryPatternApp.Tests;

/// <summary>
/// Unit tests for payment processor implementations demonstrating different factory patterns.
/// </summary>
public class PaymentProcessorTests
{
    [Fact]
    public void PaymentProcessorWithSimpleFactory_ProcessPayment_WithValidMethod_ExecutesSuccessfully()
    {
        // Arrange
        var processor = new PaymentProcessorWithSimpleFactory();
        var paymentMethod = PaymentMethod.CreditCard;
        var amount = 100m;

        // Act & Assert - should not throw
        var exception = Record.Exception(() => processor.ProcessPayment(paymentMethod, amount));
        Assert.Null(exception);
    }

    [Fact]
    public void PaymentProcessorWithSimpleFactory_ProcessPayment_WithInvalidMethod_ThrowsException()
    {
        // Arrange
        var processor = new PaymentProcessorWithSimpleFactory();
        var invalidPaymentMethod = (PaymentMethod)999;
        var amount = 100m;

        // Act & Assert
        Assert.Throws<NotSupportedException>(
            () => processor.ProcessPayment(invalidPaymentMethod, amount));
    }

    [Fact]
    public void PaymentProcessorWithFactoryMethod_ProcessPayment_WithValidFactory_ExecutesSuccessfully()
    {
        // Arrange
        var factory = new CreditCardFactory();
        var processor = new PaymentProcessorWithFactoryMethod(factory);
        var amount = 50m;

        // Act & Assert - should not throw
        var exception = Record.Exception(() => processor.ProcessPayment(amount));
        Assert.Null(exception);
    }

    [Fact]
    public void PaymentProcessorWithFactoryMethod_Constructor_WithNullFactory_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new PaymentProcessorWithFactoryMethod(null!));
    }

    [Fact]
    public void AdvancedPaymentProcessor_ProcessPayment_WithNegativeAmount_ThrowsArgumentException()
    {
        // Arrange
        var factory = new DictionaryBasedPaymentFactory();
        var processor = new AdvancedPaymentProcessor(factory);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => processor.ProcessPayment(PaymentMethod.CreditCard, -50m));
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void AdvancedPaymentProcessor_ProcessPayment_WithZeroAmount_ThrowsArgumentException()
    {
        // Arrange
        var factory = new DictionaryBasedPaymentFactory();
        var processor = new AdvancedPaymentProcessor(factory);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => processor.ProcessPayment(PaymentMethod.PayPal, 0m));
        Assert.Contains("greater than zero", exception.Message);
    }

    [Fact]
    public void AdvancedPaymentProcessor_ProcessPayment_WithValidAmounts_ExecutesSuccessfully()
    {
        // Arrange
        var factory = new DictionaryBasedPaymentFactory();
        var processor = new AdvancedPaymentProcessor(factory);
        var validAmounts = new[] { 1m, 50m, 100.50m, 1000m };

        // Act & Assert
        foreach (var amount in validAmounts)
        {
            var exception = Record.Exception(
                () => processor.ProcessPayment(PaymentMethod.CreditCard, amount));
            Assert.Null(exception);
        }
    }

    [Fact]
    public void AdvancedPaymentProcessor_Constructor_WithNullFactory_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(
            () => new AdvancedPaymentProcessor(null!));
    }
}
