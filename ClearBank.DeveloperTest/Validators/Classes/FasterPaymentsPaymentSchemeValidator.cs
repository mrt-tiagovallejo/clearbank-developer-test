using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators.Interfaces;

namespace ClearBank.DeveloperTest.Validators.Classes;

public class FasterPaymentsPaymentSchemeValidator : IPaymentSchemeValidator
{
    public bool IsValid(MakePaymentRequest request, Account account)
        => account != null &&
           account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
           account.Balance >= request.Amount;
}
