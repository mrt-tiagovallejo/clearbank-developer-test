using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services.Interfaces;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators.Interfaces;

namespace ClearBank.DeveloperTest.Services;

public class PaymentService : IPaymentService
{
    private IAccountDataStoreFactory _accountDataStoreFactory;
    private IPaymentSchemeValidator _paymentSchemeValidator;

    public PaymentService(IAccountDataStoreFactory accountDataStoreFactory, IPaymentSchemeValidator paymentSchemeValidator)
    {
        _accountDataStoreFactory = accountDataStoreFactory;
        _paymentSchemeValidator = paymentSchemeValidator;
    }

    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {
        var accountDataStore = _accountDataStoreFactory.GetAccountDataStore();
        var account = accountDataStore.GetAccount(request.DebtorAccountNumber);

        var result = new MakePaymentResult() { Success = true };

        result.Success = _paymentSchemeValidator.IsValid(request, account);

        if (result.Success)
        {
            account.Balance -= request.Amount;
            accountDataStore.UpdateAccount(account);
        }

        return result;
    }
}