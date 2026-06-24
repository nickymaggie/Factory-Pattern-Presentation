using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Concrete factory for creating PayPalPaymentService instances.
/// Part of the Factory Method pattern implementation.
/// </summary>
public sealed class PayPalFactory : ISimplePaymentFactory
{
    public IPaymentService Create() => new PayPalPaymentService();
}
