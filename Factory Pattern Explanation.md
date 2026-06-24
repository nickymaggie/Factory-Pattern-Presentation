# Factory Pattern in C#: From Code Smell to Clean Design

When explaining the Factory Pattern to senior engineers or mentoring developers, it's often most effective to start with **real code that has design problems**, then progressively refactor it.

The goal is not simply to create objects. The goal is to:

* Remove tight coupling
* Improve maintainability
* Support Open/Closed Principle (OCP)
* Simplify testing
* Centralize object creation logic
* Enable future extensibility

---

# 1. The Problem: Object Creation Scattered Everywhere

Imagine a payment processing system.

## Initial Implementation (Code Smell)

```csharp
public class PaymentProcessor
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
```

### Concrete Services

```csharp
public class CreditCardPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using Credit Card");
    }
}

public class PayPalPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using PayPal");
    }
}

public class BankTransferPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using Bank Transfer");
    }
}
```

---

## Problems

### 1. Tight Coupling

`PaymentProcessor` knows every payment implementation.

```csharp
new CreditCardPaymentService();
new PayPalPaymentService();
new BankTransferPaymentService();
```

Every new payment method requires modifying this class.

---

### 2. Violates Open/Closed Principle

Suppose business requests:

```text
Apple Pay
Google Pay
Crypto
```

You must modify:

```csharp
PaymentProcessor
```

every single time.

---

### 3. Difficult to Test

Unit testing requires real implementations.

Mocking becomes difficult because dependencies are hidden inside the method.

---

### 4. Duplicated Creation Logic

Imagine 10 classes doing:

```csharp
new CreditCardPaymentService()
```

throughout the application.

Now creation logic is everywhere.

---

# Step 1: Introduce an Abstraction

Create a common contract.

```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

Implementations:

```csharp
public sealed class CreditCardPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"Paid {amount:C} using Credit Card");
    }
}
```

```csharp
public sealed class PayPalPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"Paid {amount:C} using PayPal");
    }
}
```

```csharp
public sealed class BankTransferPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine(
            $"Paid {amount:C} using Bank Transfer");
    }
}
```

---

# Step 2: Extract Object Creation

Instead of creating objects inside business logic:

```csharp
new CreditCardPaymentService();
```

create a factory.

---

# First Factory Implementation (Simple Factory)

## Enum

```csharp
public enum PaymentMethod
{
    CreditCard,
    PayPal,
    BankTransfer
}
```

---

## Factory

```csharp
public static class PaymentServiceFactory
{
    public static IPaymentService Create(
        PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.CreditCard =>
                new CreditCardPaymentService(),

            PaymentMethod.PayPal =>
                new PayPalPaymentService(),

            PaymentMethod.BankTransfer =>
                new BankTransferPaymentService(),

            _ => throw new NotSupportedException(
                $"Payment method {paymentMethod} is not supported.")
        };
    }
}
```

---

## Refactored Processor

```csharp
public class PaymentProcessor
{
    public void ProcessPayment(
        PaymentMethod paymentMethod,
        decimal amount)
    {
        IPaymentService paymentService =
            PaymentServiceFactory.Create(paymentMethod);

        paymentService.Pay(amount);
    }
}
```

---

# Before vs After

## Before

```csharp
PaymentProcessor
    -> CreditCardPaymentService

PaymentProcessor
    -> PayPalPaymentService

PaymentProcessor
    -> BankTransferPaymentService
```

Dependencies everywhere.

---

## After

```csharp
PaymentProcessor
    -> Factory
         -> Concrete Service
```

Creation responsibility moved.

---

# Advantages Achieved

### Centralized Creation

One place:

```csharp
PaymentServiceFactory
```

controls instantiation.

---

### Cleaner Business Logic

```csharp
paymentService.Pay(amount);
```

instead of giant if-else chains.

---

### Easier Maintenance

New service:

```csharp
ApplePayPaymentService
```

Add only:

```csharp
PaymentMethod.ApplePay
```

and factory mapping.

---

# Step 3: Growing System Problem

Imagine 50 payment types.

Factory becomes:

```csharp
switch(...)
{
   case ...
   case ...
   case ...
   case ...
   case ...
}
```

Now the factory itself violates OCP.

We need a better approach.

---

# Factory Method Pattern

Instead of one giant factory:

Each payment type gets its own factory.

---

## Product Interface

```csharp
public interface IPaymentService
{
    void Pay(decimal amount);
}
```

---

## Factory Interface

```csharp
public interface IPaymentFactory
{
    IPaymentService Create();
}
```

---

## Concrete Factory

### Credit Card Factory

```csharp
public sealed class CreditCardFactory
    : IPaymentFactory
{
    public IPaymentService Create()
    {
        return new CreditCardPaymentService();
    }
}
```

---

### PayPal Factory

```csharp
public sealed class PayPalFactory
    : IPaymentFactory
{
    public IPaymentService Create()
    {
        return new PayPalPaymentService();
    }
}
```

---

### Bank Transfer Factory

```csharp
public sealed class BankTransferFactory
    : IPaymentFactory
{
    public IPaymentService Create()
    {
        return new BankTransferPaymentService();
    }
}
```

---

## Client

```csharp
public class PaymentProcessor
{
    private readonly IPaymentFactory _factory;

    public PaymentProcessor(
        IPaymentFactory factory)
    {
        _factory = factory;
    }

    public void ProcessPayment(decimal amount)
    {
        var paymentService = _factory.Create();

        paymentService.Pay(amount);
    }
}
```

---

## Usage

```csharp
var processor =
    new PaymentProcessor(
        new CreditCardFactory());

processor.ProcessPayment(100);
```

---

# Dependency Injection Integration

Modern .NET applications rarely instantiate factories manually.

---

## Registration

```csharp
builder.Services.AddTransient<
    CreditCardPaymentService>();

builder.Services.AddTransient<
    PayPalPaymentService>();

builder.Services.AddTransient<
    BankTransferPaymentService>();
```

---

## Factory Using IServiceProvider

```csharp
public interface IPaymentFactory
{
    IPaymentService Create(
        PaymentMethod method);
}
```

---

```csharp
public sealed class PaymentFactory
    : IPaymentFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentFactory(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentService Create(
        PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.CreditCard =>
                _serviceProvider
                    .GetRequiredService<
                        CreditCardPaymentService>(),

            PaymentMethod.PayPal =>
                _serviceProvider
                    .GetRequiredService<
                        PayPalPaymentService>(),

            PaymentMethod.BankTransfer =>
                _serviceProvider
                    .GetRequiredService<
                        BankTransferPaymentService>(),

            _ => throw new NotSupportedException()
        };
    }
}
```

---

# Advanced Refactoring: Dictionary-Based Factory

A common enterprise approach.

Avoid switch statements entirely.

---

## Registration

```csharp
public sealed class PaymentFactory
    : IPaymentFactory
{
    private readonly IReadOnlyDictionary<
        PaymentMethod,
        Func<IPaymentService>> _services;

    public PaymentFactory()
    {
        _services =
            new Dictionary<
                PaymentMethod,
                Func<IPaymentService>>
            {
                {
                    PaymentMethod.CreditCard,
                    () => new CreditCardPaymentService()
                },
                {
                    PaymentMethod.PayPal,
                    () => new PayPalPaymentService()
                },
                {
                    PaymentMethod.BankTransfer,
                    () => new BankTransferPaymentService()
                }
            };
    }

    public IPaymentService Create(
        PaymentMethod paymentMethod)
    {
        if (_services.TryGetValue(
                paymentMethod,
                out var creator))
        {
            return creator();
        }

        throw new NotSupportedException(
            $"Unsupported payment method {paymentMethod}");
    }
}
```

---

# Real Enterprise Example

Suppose an export module.

---

Without Factory:

```csharp
if (format == "PDF")
{
    exporter = new PdfExporter();
}
else if (format == "Excel")
{
    exporter = new ExcelExporter();
}
else if (format == "Csv")
{
    exporter = new CsvExporter();
}
```

---

With Factory:

```csharp
IExporter exporter =
    _exportFactory.Create(format);

exporter.Export(data);
```

Cleaner.

Scalable.

Testable.

---

# Unit Testing Benefits

Without Factory:

```csharp
new CreditCardPaymentService()
```

inside code.

Impossible to substitute.

---

With Factory:

```csharp
var factoryMock =
    new Mock<IPaymentFactory>();
```

```csharp
factoryMock
    .Setup(x => x.Create(It.IsAny<PaymentMethod>()))
    .Returns(mockPaymentService.Object);
```

Testing becomes straightforward.

---

# Factory Pattern vs Dependency Injection

Many developers ask:

> "If I have DI, why do I need Factory?"

DI and Factory solve different problems.

### DI

Resolves dependencies at application startup.

```csharp
constructor injection
```

---

### Factory

Resolves dependencies dynamically at runtime.

Example:

```csharp
PaymentMethod chosen by user
```

Only then can we decide:

```csharp
CreditCardPaymentService
or
PayPalPaymentService
```

A factory is still valuable.

---

# Indicators That You Need a Factory

You repeatedly see:

```csharp
new SomeClass()
```

inside business logic.

---

Large switch statements:

```csharp
switch(type)
```

creating objects.

---

Many implementations of the same interface.

```csharp
INotificationService
```

```csharp
EmailNotificationService
SmsNotificationService
PushNotificationService
```

---

Runtime selection of implementation.

```csharp
if(configuration == ...)
```

---

# Summary

| Without Factory                       | With Factory                           |
| ------------------------------------- | -------------------------------------- |
| Tight coupling                        | Loose coupling                         |
| Object creation scattered             | Centralized creation                   |
| Large if/switch statements            | Encapsulated creation logic            |
| Hard to test                          | Easy to mock                           |
| Violates OCP                          | Better OCP compliance                  |
| Difficult to extend                   | Easier extension                       |
| Business logic knows concrete classes | Business logic depends on abstractions |

The Factory Pattern is fundamentally about **separating object creation from object usage**. In mature C# applications, it often appears alongside Dependency Injection, Strategy Pattern, and configuration-driven runtime selection, making systems significantly easier to extend and maintain as requirements evolve.
