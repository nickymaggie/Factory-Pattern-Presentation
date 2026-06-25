using FactoryPatternApp.Models;
using FactoryPatternApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FactoryPatternApp.Factories;

/// <summary>
/// Dependency Injection registration for the keyed-services factory approach.
/// Each payment service is registered under its <see cref="PaymentMethod"/> key,
/// and the factory itself is registered as <see cref="IPaymentFactory"/>.
/// Adding a new payment method is a one-line registration here — the factory
/// and the processors never change.
/// </summary>
public static class PaymentServiceRegistration
{
    public static IServiceCollection AddPaymentServices(this IServiceCollection services)
    {
        services.AddKeyedTransient<IPaymentService, CreditCardPaymentService>(PaymentMethod.CreditCard);
        services.AddKeyedTransient<IPaymentService, PayPalPaymentService>(PaymentMethod.PayPal);
        services.AddKeyedTransient<IPaymentService, BankTransferPaymentService>(PaymentMethod.BankTransfer);
        services.AddKeyedTransient<IPaymentService, ApplePayPaymentService>(PaymentMethod.ApplePay);
        services.AddKeyedTransient<IPaymentService, GooglePayPaymentService>(PaymentMethod.GooglePay);

        services.AddSingleton<IPaymentFactory, KeyedServicePaymentFactory>();

        return services;
    }
}
