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

    // Used for logging/auditing the chosen method.
    string GetPaymentMethodName();
}
```

> **Note on async:** real payment I/O is asynchronous. In production this
> contract would be `Task PayAsync(decimal amount)`. We keep it synchronous
> here so the focus stays on object *creation*, not I/O.

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
public static class SimplePaymentServiceFactory
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
            SimplePaymentServiceFactory.Create(paymentMethod);

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
SimplePaymentServiceFactory
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

> **Be honest about OCP here.** The simple factory does *not* fully satisfy the
> Open/Closed Principle — you still edit the factory's `switch` for every new
> method. What it buys you is **localization**: the change lives in one
> well-known place instead of being scattered across the codebase. Genuine OCP
> (extend without editing existing code) comes later, with DI-driven
> registration — see *Keyed Services*.

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

This pattern uses a **parameterless** factory contract — each concrete factory
knows the single product it builds. To avoid clashing with the parameterized
`IPaymentFactory` introduced later (which takes a `PaymentMethod`), we name this
one `ISimplePaymentFactory`:

```csharp
public interface ISimplePaymentFactory
{
    IPaymentService Create();
}
```

> **Two factory interfaces, two jobs.** `ISimplePaymentFactory.Create()` is the
> *Factory Method* shape: one factory per product, no argument. The
> `IPaymentFactory.Create(PaymentMethod)` shape (next section) is a *parameterized*
> factory: one factory that picks a product from runtime input.

---

## Concrete Factory

### Credit Card Factory

```csharp
public sealed class CreditCardFactory
    : ISimplePaymentFactory
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
    : ISimplePaymentFactory
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
    : ISimplePaymentFactory
{
    public IPaymentService Create()
    {
        return new BankTransferPaymentService();
    }
}
```

> **Trade-off:** Factory Method scales by adding *classes*, not switch arms — so
> a system with 50 products needs 50 factory classes. It gives you the cleanest
> per-type extensibility but the most boilerplate. The parameterized and
> keyed-services factories below trade some of that for fewer types.

---

## Client

```csharp
public class PaymentProcessor
{
    private readonly ISimplePaymentFactory _factory;

    public PaymentProcessor(
        ISimplePaymentFactory factory)
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

> **Pre-.NET-8 pattern.** Resolving each type from `IServiceProvider` via a
> `switch` was the classic way to combine DI with a factory. It works and gives
> the services real DI-built dependencies — but the `switch` still grows per
> type. On .NET 8+, *keyed services* (below) remove that switch entirely.

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

> **Still not true OCP.** A dictionary reads as cleaner than a `switch`, but
> adding a payment method *still* means editing this class's constructor, and
> the `() => new XxxService()` lambdas hard-code construction — so dependencies
> can't be injected into the services. The dictionary's real value is when the
> entries are **registered from outside** (e.g. each module registers its own
> creators at startup) rather than hard-coded here. That idea reaches its clean
> form with DI keyed services, next.

---

# The Modern .NET Approach: Keyed Services (DI)

Since **.NET 8**, the DI container can register multiple implementations of the
same interface under a *key*. This is the cleanest way to reach genuine OCP:
adding a payment method is a one-line registration, and the factory contains no
`switch` and no dictionary to edit.

---

## Registration

```csharp
public static class PaymentServiceRegistration
{
    public static IServiceCollection AddPaymentServices(
        this IServiceCollection services)
    {
        services.AddKeyedTransient<IPaymentService, CreditCardPaymentService>(
            PaymentMethod.CreditCard);
        services.AddKeyedTransient<IPaymentService, PayPalPaymentService>(
            PaymentMethod.PayPal);
        services.AddKeyedTransient<IPaymentService, BankTransferPaymentService>(
            PaymentMethod.BankTransfer);
        // New method? Add one line here. Nothing else changes.

        services.AddSingleton<IPaymentFactory, KeyedServicePaymentFactory>();
        return services;
    }
}
```

---

## Factory

```csharp
public sealed class KeyedServicePaymentFactory : IPaymentFactory
{
    private readonly IServiceProvider _serviceProvider;

    public KeyedServicePaymentFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public IPaymentService Create(PaymentMethod paymentMethod)
    {
        IPaymentService? service =
            _serviceProvider.GetKeyedService<IPaymentService>(paymentMethod);

        return service ?? throw new NotSupportedException(
            $"Payment method '{paymentMethod}' is not supported.");
    }
}
```

The factory body never grows. Each service can take its own constructor
dependencies (HTTP clients, loggers, options) because the container builds it —
something the `() => new XxxService()` lambdas could not do.

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

Wires up a dependency graph that is **known at registration time**. The set of
collaborators a class needs is fixed when the container is configured; the
container just supplies them.

```csharp
constructor injection
```

---

### Factory

Chooses an implementation based on **runtime data** the container can't know in
advance — a user selection, a config value, a message payload.

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

(Both resolve *at runtime* — the distinction isn't "startup vs runtime", it's
*who decides which type*: the container from static registration, or the factory
from runtime data.)

A factory is still valuable — and as *Keyed Services* shows, the two compose:
the factory makes the data-driven choice, the container builds the chosen
service with its own dependencies.

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

A practical progression for choosing a variant:

| Variant | Reach for it when |
| ------- | ----------------- |
| Simple (static switch) factory | A handful of types; you want creation in one place |
| Factory Method (one factory per type) | Each product has distinct creation logic; you favor classes over switch arms |
| Parameterized factory | One factory selects from runtime input |
| Dictionary-based factory | Entries are registered from outside, not hard-coded |
| **Keyed services (DI, .NET 8+)** | You want genuine OCP and services with their own injected dependencies |

The earlier variants "localize" change; only registration-driven approaches
(keyed services) let you extend behavior **without editing existing code**.
