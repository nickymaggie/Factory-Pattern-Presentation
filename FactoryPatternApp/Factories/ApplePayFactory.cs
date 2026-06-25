using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Concrete factory for creating ApplePayPaymentService instances.
/// Part of the Factory Method pattern implementation.
/// </summary>
public sealed class ApplePayFactory : ISimplePaymentFactory
{
    public IPaymentService Create() => new ApplePayPaymentService();
}
