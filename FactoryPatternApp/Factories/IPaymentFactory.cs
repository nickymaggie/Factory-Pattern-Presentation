using FactoryPatternApp.Models;
using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Interface for payment service factories.
/// Defines the contract for creating payment service instances.
/// </summary>
public interface IPaymentFactory
{
    /// <summary>
    /// Creates a payment service instance for the specified payment method.
    /// </summary>
    /// <param name="paymentMethod">The payment method to create a service for.</param>
    /// <returns>An instance of IPaymentService.</returns>
    IPaymentService Create(PaymentMethod paymentMethod);
}

/// <summary>
/// Extended factory interface for factories that create services without parameters.
/// </summary>
public interface ISimplePaymentFactory
{
    /// <summary>
    /// Creates a payment service instance.
    /// </summary>
    /// <returns>An instance of IPaymentService.</returns>
    IPaymentService Create();
}
