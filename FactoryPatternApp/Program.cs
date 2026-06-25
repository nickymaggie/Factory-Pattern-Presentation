using FactoryPatternApp.Factories;
using FactoryPatternApp.Models;
using FactoryPatternApp.Processors;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Main entry point for the Factory Pattern Demonstration application.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main method that orchestrates all factory pattern demonstrations.
    /// </summary>
    private static void Main()
    {
        DisplayHeader();

        DemoAntiPattern();
        DemoSimpleFactory();
        DemoFactoryMethod();
        DemoParameterizedFactory();
        DemoDictionaryBasedFactory();
        DemoKeyedServicesFactory();
        DemoErrorHandling();

        //DisplaySummary();
    }

    /// <summary>
    /// Displays the application header.
    /// </summary>
    private static void DisplayHeader()
    {
        Console.WriteLine("=== Factory Pattern Demonstration in C# ===\n");
    }

    /// <summary>
    /// Demonstrates the anti-pattern: object creation scattered in business logic.
    /// </summary>
    private static void DemoAntiPattern()
    {
        Console.WriteLine("--- Demo 1: Anti-Pattern (Object Creation Scattered) ---");
        var processorWithoutFactory = new PaymentProcessorWithoutFactory();
        processorWithoutFactory.ProcessPayment("CreditCard", 50.00m);
        processorWithoutFactory.ProcessPayment("PayPal", 75.50m);
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the Simple Factory pattern using static factory methods.
    /// </summary>
    private static void DemoSimpleFactory()
    {
        Console.WriteLine("--- Demo 2: Simple Factory Pattern ---");
        var processorWithSimpleFactory = new PaymentProcessorWithSimpleFactory();
        processorWithSimpleFactory.ProcessPayment(PaymentMethod.CreditCard, 100.00m);
        processorWithSimpleFactory.ProcessPayment(PaymentMethod.PayPal, 125.75m);
        processorWithSimpleFactory.ProcessPayment(PaymentMethod.BankTransfer, 250.00m);
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the Factory Method pattern with individual factories.
    /// </summary>
    private static void DemoFactoryMethod()
    {
        Console.WriteLine("--- Demo 3: Factory Method Pattern ---");
        var creditCardProcessor = new PaymentProcessorWithFactoryMethod(new CreditCardFactory());
        creditCardProcessor.ProcessPayment(99.99m);

        var payPalProcessor = new PaymentProcessorWithFactoryMethod(new PayPalFactory());
        payPalProcessor.ProcessPayment(49.99m);

        var bankTransferProcessor = new PaymentProcessorWithFactoryMethod(new BankTransferFactory());
        bankTransferProcessor.ProcessPayment(500.00m);
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the Parameterized Factory pattern supporting runtime selection.
    /// </summary>
    private static void DemoParameterizedFactory()
    {
        Console.WriteLine("--- Demo 4: Parameterized Factory Pattern ---");
        var parameterizedFactory = new PaymentFactory();
        var processorWithParameterized = new PaymentProcessorWithParameterizedFactory(parameterizedFactory);

        processorWithParameterized.ProcessPayment(PaymentMethod.CreditCard, 150.00m);
        processorWithParameterized.ProcessPayment(PaymentMethod.ApplePay, 75.00m);
        processorWithParameterized.ProcessPayment(PaymentMethod.GooglePay, 60.00m);
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the Dictionary-Based Factory pattern for enterprise scalability.
    /// </summary>
    private static void DemoDictionaryBasedFactory()
    {
        Console.WriteLine("--- Demo 5: Dictionary-Based Factory (Enterprise Pattern) ---");
        var dictionaryFactory = new DictionaryBasedPaymentFactory();
        var advancedProcessor = new AdvancedPaymentProcessor(dictionaryFactory);

        advancedProcessor.ProcessPayment(PaymentMethod.CreditCard, 200.00m);
        advancedProcessor.ProcessPayment(PaymentMethod.PayPal, 150.00m);
        advancedProcessor.ProcessPayment(PaymentMethod.BankTransfer, 1000.00m);
        advancedProcessor.ProcessPayment(PaymentMethod.ApplePay, 85.50m);
        advancedProcessor.ProcessPayment(PaymentMethod.GooglePay, 45.00m);
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the modern .NET 8 approach: a factory backed by keyed
    /// services resolved from the DI container. Adding a payment method only
    /// requires a new registration — the factory has no switch or dictionary.
    /// </summary>
    private static void DemoKeyedServicesFactory()
    {
        Console.WriteLine("--- Demo 6: Keyed-Services Factory (DI, .NET 8) ---");

        ServiceProvider serviceProvider = new ServiceCollection()
            .AddPaymentServices()
            .BuildServiceProvider();

        // In a real app the processor would be resolved from the container too;
        // here we resolve the factory to keep the demo focused.
        var factory = serviceProvider.GetRequiredService<IPaymentFactory>();
        var processor = new PaymentProcessorWithParameterizedFactory(factory);

        processor.ProcessPayment(PaymentMethod.CreditCard, 320.00m);
        processor.ProcessPayment(PaymentMethod.ApplePay, 12.49m);
        processor.ProcessPayment(PaymentMethod.GooglePay, 8.99m);
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates error handling and validation.
    /// </summary>
    private static void DemoErrorHandling()
    {
        Console.WriteLine("--- Demo 7: Error Handling ---");
        var dictionaryFactory = new DictionaryBasedPaymentFactory();
        var advancedProcessor = new AdvancedPaymentProcessor(dictionaryFactory);

        try
        {
            advancedProcessor.ProcessPayment(PaymentMethod.CreditCard, -50);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"   ❌ Error: {ex.Message}");
        }

        try
        {
            var processorBad = new PaymentProcessorWithSimpleFactory();
            processorBad.ProcessPayment((PaymentMethod)999, 100);
        }
        catch (NotSupportedException ex)
        {
            Console.WriteLine($"   ❌ Error: {ex.Message}");
        }
        Console.WriteLine();
    }

    /// <summary>
    /// Displays the summary and key takeaways.
    /// </summary>
    private static void DisplaySummary()
    {
        Console.WriteLine("=== Key Takeaways ===");
        Console.WriteLine("✓ Decouples object creation from usage");
        Console.WriteLine("✓ Centralizes creation logic");
        Console.WriteLine("✓ Improves testability and maintainability");
        Console.WriteLine("✓ Supports Open/Closed Principle (OCP)");
        Console.WriteLine("✓ Enables runtime selection of implementations");
    }
}
