namespace FactoryPatternApp.Services;

/// <summary>
/// Interface that all payment services must implement.
/// Defines the contract for processing payments.
/// </summary>
public interface IPaymentService
{
    /// <summary>
    /// Processes a payment for the given amount.
    /// </summary>
    /// <param name="amount">The payment amount in currency units.</param>
    void Pay(decimal amount);

    /// <summary>
    /// Gets the name of the payment method.
    /// </summary>
    string GetPaymentMethodName();
}
