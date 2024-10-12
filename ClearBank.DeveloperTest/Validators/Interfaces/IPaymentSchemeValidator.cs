using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Validators.Interfaces;

public interface IPaymentSchemeValidator
{
    bool IsValid(MakePaymentRequest request, Account account);
}
