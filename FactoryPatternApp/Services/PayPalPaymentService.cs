namespace FactoryPatternApp.Services;

/// <summary>
/// Concrete implementation for processing PayPal payments.
/// </summary>
public sealed class PayPalPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"🅿️ Paid {amount:C} using PayPal");
    }

    public string GetPaymentMethodName() => "PayPal";
}
