using FactoryPatternApp.Models;
using FactoryPatternApp.Services;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Parameterized factory that creates payment services based on PaymentMethod enum.
/// Supports runtime selection of implementation.
/// Uses switch expression for clean, maintainable code.
/// </summary>
public sealed class PaymentFactory : IPaymentFactory
{
    /// <summary>
    /// Creates a payment service for the specified payment method.
    /// </summary>
    /// <param name="paymentMethod">The payment method enum value.</param>
    /// <returns>An instance of the appropriate payment service.</returns>
    /// <exception cref="NotSupportedException">Thrown when an unsupported payment method is requested.</exception>
    public IPaymentService Create(PaymentMethod paymentMethod)
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
