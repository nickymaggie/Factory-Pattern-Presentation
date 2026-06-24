namespace FactoryPatternApp.Services;

/// <summary>
/// Concrete implementation for processing bank transfer payments.
/// </summary>
public sealed class BankTransferPaymentService : IPaymentService
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"🏦 Paid {amount:C} using Bank Transfer");
    }

    public string GetPaymentMethodName() => "Bank Transfer";
}
