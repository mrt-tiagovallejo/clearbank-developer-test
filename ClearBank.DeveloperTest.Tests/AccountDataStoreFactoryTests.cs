using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Data.Interfaces;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    public class AccountDataStoreFactoryTests
    {
        private Mock<IConfigurationManager> _mockConfigurationManager;
        private AccountDataStoreFactory _accountDataStoreFactory;

        [SetUp]
        public void Setup()
        {
            _mockConfigurationManager = new Mock<IConfigurationManager>();
            _accountDataStoreFactory = new AccountDataStoreFactory(_mockConfigurationManager.Object);
        }

        [Test]
        public void GetAccountDataStore_ReturnsBackupAccountDataStore_WhenDataStoreTypeIsBackup()
        {
            // Arrange
            _mockConfigurationManager
                .Setup(cm => cm.GetAppSetting("DataStoreType"))
                .Returns("Backup");

            // Act
            var result = _accountDataStoreFactory.GetAccountDataStore();

            // Assert
            Assert.That(result, Is.TypeOf<BackupAccountDataStore>());
        }

        [Test]
        public void GetAccountDataStore_ReturnsAccountDataStore_WhenUnknownDataStoreType()
        {
            // Arrange
            _mockConfigurationManager
                .Setup(cm => cm.GetAppSetting("DataStoreType"))
                .Returns(string.Empty);

            // Act
            var result = _accountDataStoreFactory.GetAccountDataStore();

            // Assert
            Assert.That(result, Is.TypeOf<AccountDataStore>());
        }
    }
}
