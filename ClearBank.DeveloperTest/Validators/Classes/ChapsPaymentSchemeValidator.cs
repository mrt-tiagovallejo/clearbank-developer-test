using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators.Interfaces;

namespace ClearBank.DeveloperTest.Validators.Classes
{
    public class ChapsPaymentSchemeValidator : IPaymentSchemeValidator
    {
        public bool IsValid(MakePaymentRequest request, Account account) 
            => account != null &&
               account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
               account.Status == AccountStatus.Live;
    }
}
