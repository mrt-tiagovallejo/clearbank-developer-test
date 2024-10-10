using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data.Interfaces
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore GetAccountDataStore();
    }
}