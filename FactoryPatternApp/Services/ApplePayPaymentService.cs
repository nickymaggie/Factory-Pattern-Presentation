namespace FactoryPatternApp.Services;

/// <summary>
/// Concrete implementation for processing Apple Pay payments.
/// </summary>
public sealed class ApplePayPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"🍎 Paid {amount:C} using Apple Pay");
    }

    public string GetPaymentMethodName() => "Apple Pay";
}
