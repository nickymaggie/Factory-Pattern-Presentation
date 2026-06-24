using FactoryPatternApp.Models;
using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Simple Factory implementation using a switch expression.
/// Good for smaller sets of types; can become unwieldy with many types.
/// </summary>
public static class SimplePaymentServiceFactory
{
    /// <summary>
    /// Creates a payment service based on the specified payment method.
    /// </summary>
    /// <param name="paymentMethod">The payment method enum value.</param>
    /// <returns>An instance of the appropriate payment service.</returns>
    /// <exception cref="NotSupportedException">Thrown when an unsupported payment method is requested.</exception>
    public static IPaymentService Create(PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.CreditCard => new CreditCardPaymentService(),
            PaymentMethod.PayPal => new PayPalPaymentService(),
            PaymentMethod.BankTransfer => new BankTransferPaymentService(),
            PaymentMethod.ApplePay => new ApplePayPaymentService(),
            PaymentMethod.GooglePay => new GooglePayPaymentService(),
            _ => throw new NotSupportedException(
                $"Payment method '{paymentMethod}' is not supported.")
        };
    }
}
