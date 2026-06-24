# Factory Pattern Console Application

A comprehensive C# console application demonstrating the Factory Pattern in multiple implementations: Simple Factory, Factory Method, and Advanced Dictionary-Based Factory.

## Project Structure

```
FactoryPatternApp/
├── Models/
│   └── PaymentMethod.cs           # Enum defining payment methods
├── Services/
│   ├── IPaymentService.cs         # Payment service interface
│   ├── CreditCardPaymentService.cs
│   ├── PayPalPaymentService.cs
│   ├── BankTransferPaymentService.cs
│   ├── ApplePayPaymentService.cs
│   └── GooglePayPaymentService.cs
├── Factories/
│   ├── IPaymentFactory.cs         # Factory interface
│   ├── SimplePaymentServiceFactory.cs  # Simple Factory
│   ├── PaymentFactory.cs          # Parameterized Factory
│   ├── DictionaryBasedPaymentFactory.cs  # Advanced Dictionary-Based Factory
│   ├── CreditCardFactory.cs       # Concrete Factory Method
│   ├── PayPalFactory.cs           # Concrete Factory Method
│   └── BankTransferFactory.cs     # Concrete Factory Method
├── Processors/
│   └── PaymentProcessor.cs        # Payment processor implementations
├── Program.cs                      # Console application entry point
└── FactoryPatternApp.csproj       # Project file
```

## Running the Application

### Prerequisites
- .NET 8.0 SDK or later

### Execution

From the `FactoryPatternApp` directory:

```bash
dotnet run
```

## Demonstrations

The application demonstrates:

1. **Anti-Pattern** - Object creation scattered in business logic (NOT recommended)
2. **Simple Factory** - Static factory method using switch expressions
3. **Factory Method** - Each concrete type has its own factory class
4. **Parameterized Factory** - Runtime selection of payment method
5. **Dictionary-Based Factory** - Enterprise-level factory using delegates
6. **Error Handling** - Proper exception handling and validation

## C# Naming Conventions Used

- **Classes**: PascalCase (e.g., `CreditCardPaymentService`)
- **Interfaces**: PascalCase with `I` prefix (e.g., `IPaymentService`)
- **Methods**: PascalCase (e.g., `ProcessPayment`)
- **Properties**: PascalCase
- **Private fields**: _camelCase (e.g., `_factory`)
- **Enums**: PascalCase (e.g., `PaymentMethod`)
- **Local variables**: camelCase (e.g., `paymentService`)
- **Constants**: PascalCase

## Code Style Features

✓ XML documentation comments on public members
✓ `sealed` keyword on concrete implementations
✓ Null argument validation
✓ Proper use of `using` directives with file-scoped namespaces
✓ `readonly` for immutable fields
✓ Switch expressions for pattern matching
✓ Exception handling with descriptive messages

## Key Takeaways

| Aspect | Benefit |
|--------|---------|
| **Decoupling** | Payment processor doesn't depend on concrete payment classes |
| **Testability** | Easy to mock factories and payment services |
| **Maintainability** | Changes to payment logic isolated to service classes |
| **Extensibility** | New payment methods add minimal code |
| **OCP Compliance** | Open for extension, closed for modification |

## Output Example

```
=== Factory Pattern Demonstration in C# ===

--- Demo 1: Anti-Pattern (Object Creation Scattered) ---
💳 Paid $50.00 using Credit Card
🅿️ Paid $75.50 using PayPal

--- Demo 2: Simple Factory Pattern ---
💳 Paid $100.00 using Credit Card
🅿️ Paid $125.75 using PayPal
🏦 Paid $250.00 using Bank Transfer
...
```

## Further Learning

- Compare each factory implementation to understand trade-offs
- Modify the code to add new payment methods
- Experiment with dependency injection frameworks
- Consider using `System.Reflection` for dynamic factory registration
