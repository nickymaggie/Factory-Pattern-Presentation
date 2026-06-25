using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Concrete factory for creating GooglePayPaymentService instances.
/// Part of the Factory Method pattern implementation.
/// </summary>
public sealed class GooglePayFactory : ISimplePaymentFactory
{
    public IPaymentService Create() => new GooglePayPaymentService();
}
