using FactoryPatternApp.Factories;
using FactoryPatternApp.Models;
using FactoryPatternApp.Services;

namespace FactoryPatternApp.Processors;

/// <summary>
/// Demonstrates the anti-pattern: object creation scattered in business logic.
/// AVOID THIS APPROACH - shown for educational purposes.
/// </summary>
public class PaymentProcessorWithoutFactory
{
    public void ProcessPayment(string paymentMethod, decimal amount)
    {
        if (paymentMethod == "CreditCard")
        {
            var paymentService = new CreditCardPaymentService();
            paymentService.Pay(amount);
        }
        else if (paymentMethod == "PayPal")
        {
            var paymentService = new PayPalPaymentService();
            paymentService.Pay(amount);
        }
        else if (paymentMethod == "BankTransfer")
        {
            var paymentService = new BankTransferPaymentService();
            paymentService.Pay(amount);
        }
        else
        {
            throw new NotSupportedException(
                $"Payment method '{paymentMethod}' is not supported.");
        }
    }
}

/// <summary>
/// Demonstrates the improved approach using Simple Factory.
/// Business logic is cleaner and object creation is centralized.
/// </summary>
public class PaymentProcessorWithSimpleFactory
{
    public void ProcessPayment(PaymentMethod paymentMethod, decimal amount)
    {
        IPaymentService paymentService = Factories.SimplePaymentServiceFactory.Create(paymentMethod);
        paymentService.Pay(amount);
    }
}

/// <summary>
/// Demonstrates Factory Method pattern.
/// Each concrete factory is injected via constructor.
/// Enables better testability and flexibility.
/// </summary>
public class PaymentProcessorWithFactoryMethod
{
    private readonly ISimplePaymentFactory _factory;

    public PaymentProcessorWithFactoryMethod(ISimplePaymentFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void ProcessPayment(decimal amount)
    {
        IPaymentService paymentService = _factory.Create();
        paymentService.Pay(amount);
    }
}

/// <summary>
/// Demonstrates using the parameterized factory interface.
/// Supports runtime selection of payment method.
/// </summary>
public class PaymentProcessorWithParameterizedFactory
{
    private readonly IPaymentFactory _factory;

    public PaymentProcessorWithParameterizedFactory(IPaymentFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void ProcessPayment(PaymentMethod paymentMethod, decimal amount)
    {
        IPaymentService paymentService = _factory.Create(paymentMethod);
        paymentService.Pay(amount);
    }
}

/// <summary>
/// Advanced processor using dictionary-based factory.
/// Scales well with many payment types.
/// </summary>
public sealed class AdvancedPaymentProcessor
{
    private readonly IPaymentFactory _factory;

    public AdvancedPaymentProcessor(IPaymentFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public void ProcessPayment(PaymentMethod paymentMethod, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        IPaymentService paymentService = _factory.Create(paymentMethod);
        paymentService.Pay(amount);

        LogTransaction(paymentMethod, amount);
    }

    private static void LogTransaction(PaymentMethod paymentMethod, decimal amount)
    {
        Console.WriteLine($"   ✓ Transaction logged: {paymentMethod} - {amount:C}");
    }
}
