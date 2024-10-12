using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators.Classes;
using ClearBank.DeveloperTest.Validators.Interfaces;

namespace ClearBank.DeveloperTest.Validators;

public class PaymentSchemeValidator : IPaymentSchemeValidator
{
    public bool IsValid(MakePaymentRequest request, Account account)
    {
        return request.PaymentScheme switch
        {
            PaymentScheme.Bacs => new BacsPaymentSchemeValidator().IsValid(request, account),
            PaymentScheme.FasterPayments => new FasterPaymentsPaymentSchemeValidator().IsValid(request, account),
            PaymentScheme.Chaps => new ChapsPaymentSchemeValidator().IsValid(request, account),
            _ => false
        };
    }
}
