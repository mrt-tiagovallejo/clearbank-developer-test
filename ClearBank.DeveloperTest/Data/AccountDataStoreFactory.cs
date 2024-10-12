using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Data;

public class AccountDataStoreFactory : IAccountDataStoreFactory
{
    private IConfigurationManager _configurationManager;

    public AccountDataStoreFactory(IConfigurationManager configurationManager)
    {
        _configurationManager = configurationManager;
    }

    public IAccountDataStore GetAccountDataStore()
    {
        var dataStoreType = _configurationManager.GetAppSetting(nameof(AppSettings.DataStoreType));

        return dataStoreType switch
        {
            nameof(DataStoryTypes.Backup) => new BackupAccountDataStore(),
            _ => new AccountDataStore()
        };
    }
}