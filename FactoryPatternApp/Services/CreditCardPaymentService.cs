namespace FactoryPatternApp.Services;

/// <summary>
/// Concrete implementation for processing credit card payments.
/// </summary>
public sealed class CreditCardPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"💳 Paid {amount:C} using Credit Card");
    }

    public string GetPaymentMethodName() => "Credit Card";
}
