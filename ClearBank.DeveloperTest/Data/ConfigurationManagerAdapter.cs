using ClearBank.DeveloperTest.Data.Interfaces;
using System.Configuration;

namespace ClearBank.DeveloperTest.Data
{
    public class ConfigurationManagerAdapter : IConfigurationManager
    {
        public string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
