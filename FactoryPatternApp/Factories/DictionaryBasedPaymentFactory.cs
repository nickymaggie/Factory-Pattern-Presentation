using FactoryPatternApp.Models;
using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Advanced factory implementation using a dictionary for factory registration.
/// Eliminates switch/if-else chains and supports dynamic registration.
/// Excellent for enterprise applications with many types.
/// </summary>
public sealed class DictionaryBasedPaymentFactory : IPaymentFactory
{
    private readonly IReadOnlyDictionary<PaymentMethod, Func<IPaymentService>> _factories;

    public DictionaryBasedPaymentFactory()
    {
        _factories = new Dictionary<PaymentMethod, Func<IPaymentService>>
        {
            { PaymentMethod.CreditCard, () => new CreditCardPaymentService() },
            { PaymentMethod.PayPal, () => new PayPalPaymentService() },
            { PaymentMethod.BankTransfer, () => new BankTransferPaymentService() },
            { PaymentMethod.ApplePay, () => new ApplePayPaymentService() },
            { PaymentMethod.GooglePay, () => new GooglePayPaymentService() }
        };
    }

    /// <summary>
    /// Creates a payment service based on the payment method.
    /// </summary>
    /// <param name="paymentMethod">The payment method to create a service for.</param>
    /// <returns>An instance of the appropriate payment service.</returns>
    /// <exception cref="NotSupportedException">Thrown when an unsupported payment method is requested.</exception>
    public IPaymentService Create(PaymentMethod paymentMethod)
    {
        if (_factories.TryGetValue(paymentMethod, out var factory))
        {
            return factory();
        }

        throw new NotSupportedException(
            $"Unsupported payment method: {paymentMethod}");
    }
}
