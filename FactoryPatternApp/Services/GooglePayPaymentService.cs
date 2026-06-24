namespace FactoryPatternApp.Services;

/// <summary>
/// Concrete implementation for processing Google Pay payments.
/// </summary>
public sealed class GooglePayPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"🔵 Paid {amount:C} using Google Pay");
    }

    public string GetPaymentMethodName() => "Google Pay";
}
