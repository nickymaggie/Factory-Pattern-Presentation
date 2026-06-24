using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Concrete factory for creating BankTransferPaymentService instances.
/// Part of the Factory Method pattern implementation.
/// </summary>
public sealed class BankTransferFactory : ISimplePaymentFactory
{
    public IPaymentService Create() => new BankTransferPaymentService();
}
