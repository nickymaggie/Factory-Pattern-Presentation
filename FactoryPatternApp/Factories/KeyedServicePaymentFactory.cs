using FactoryPatternApp.Models;
using FactoryPatternApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Factory that resolves payment services from the DI container using
/// .NET 8 keyed services. This is the most "open/closed" of the factory
/// variants: adding a new payment method only requires registering a new
/// keyed service (see <see cref="PaymentServiceRegistration"/>) — there is
/// no switch or dictionary inside the factory to edit.
/// </summary>
public sealed class KeyedServicePaymentFactory : IPaymentFactory
{
    private readonly IServiceProvider _serviceProvider;

    public KeyedServicePaymentFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Resolves the payment service registered under the given payment method key.
    /// </summary>
    /// <param name="paymentMethod">The payment method to resolve a service for.</param>
    /// <returns>An instance of the appropriate payment service.</returns>
    /// <exception cref="NotSupportedException">Thrown when no service is registered for the payment method.</exception>
    public IPaymentService Create(PaymentMethod paymentMethod)
    {
        IPaymentService? service = _serviceProvider.GetKeyedService<IPaymentService>(paymentMethod);

        return service ?? throw new NotSupportedException(
            $"Payment method '{paymentMethod}' is not supported.");
    }
}
