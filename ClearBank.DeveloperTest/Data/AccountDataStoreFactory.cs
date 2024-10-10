using ClearBank.DeveloperTest.Data.Interfaces;
using System.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class AccountDataStoreFactory : IAccountDataStoreFactory
    {
        private IConfigurationManager _configurationManager;

        public AccountDataStoreFactory(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public IAccountDataStore GetAccountDataStore()
        {
            // TODO: move hardcoded string somewhere
            var dataStoreType = _configurationManager.GetAppSetting("DataStoreType");

            return dataStoreType switch
            {
                // TODO: enum this instead of hardcoded data store types
                "Backup" => new BackupAccountDataStore(),
                _ => new AccountDataStore()
            };
        }
    }
}
