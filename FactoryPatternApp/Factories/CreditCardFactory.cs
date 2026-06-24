using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Concrete factory for creating CreditCardPaymentService instances.
/// Part of the Factory Method pattern implementation.
/// </summary>
public sealed class CreditCardFactory : ISimplePaymentFactory
{
    public IPaymentService Create() => new CreditCardPaymentService();
}
