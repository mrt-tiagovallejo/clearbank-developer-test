using ClearBank.DeveloperTest.Data.Interfaces;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Validators.Interfaces;
using Moq;
using NUnit.Framework;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private Mock<IAccountDataStoreFactory> _mockAccountDataStoreFactory;
        private Mock<IAccountDataStore> _mockAccountDataStore;
        private Mock<IPaymentSchemeValidator> _mockPaymentSchemeValidator;
        private PaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            // Initialize mocks
            _mockAccountDataStoreFactory = new Mock<IAccountDataStoreFactory>();
            _mockAccountDataStore = new Mock<IAccountDataStore>();
            _mockPaymentSchemeValidator = new Mock<IPaymentSchemeValidator>();

            // Setup AccountDataStoreFactory to return the mock AccountDataStore
            _mockAccountDataStoreFactory
                .Setup(factory => factory.GetAccountDataStore())
                .Returns(_mockAccountDataStore.Object);

            // Create the PaymentService with the mocked objects
            _paymentService = new PaymentService(
                _mockAccountDataStoreFactory.Object,
                _mockPaymentSchemeValidator.Object);
        }

        [Test]
        public void MakePayment_ReturnsSuccess_When_PaymentIsValid()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "123456",
                Amount = 100,
                PaymentScheme = PaymentScheme.Bacs
            };

            var account = new Account
            {
                AccountNumber = "123456",
                Balance = 500,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };

            // Setup the mock AccountDataStore to return the account
            _mockAccountDataStore
                .Setup(store => store.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            // Setup the mock PaymentSchemeValidator to return true (valid payment)
            _mockPaymentSchemeValidator
                .Setup(validator => validator.IsValid(request, account))
                .Returns(true);

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.True);
            _mockAccountDataStore.Verify(store => store.UpdateAccount(It.Is<Account>(a => a.Balance == 400)), Times.Once);
        }

        [Test]
        public void MakePayment_ReturnsFail_When_PaymentIsInvalid()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "123456",
                Amount = 100,
                PaymentScheme = PaymentScheme.FasterPayments
            };

            var account = new Account
            {
                AccountNumber = "123456",
                Balance = 500,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };

            // Setup the mock AccountDataStore to return the account
            _mockAccountDataStore
                .Setup(store => store.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            // Setup the mock PaymentSchemeValidator to return false (invalid payment)
            _mockPaymentSchemeValidator
                .Setup(validator => validator.IsValid(request, account))
                .Returns(false);

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.False);
            _mockAccountDataStore.Verify(store => store.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }

        [Test]
        public void MakePayment_ReducesBalance_When_PaymentIsValid()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "123456",
                Amount = 100,
                PaymentScheme = PaymentScheme.Bacs
            };

            var account = new Account
            {
                AccountNumber = "123456",
                Balance = 500,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };

            // Setup the mock AccountDataStore to return the account
            _mockAccountDataStore
                .Setup(store => store.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            // Setup the mock PaymentSchemeValidator to return true (valid payment)
            _mockPaymentSchemeValidator
                .Setup(validator => validator.IsValid(request, account))
                .Returns(true);

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(account.Balance, Is.EqualTo(400));
            _mockAccountDataStore.Verify(store => store.UpdateAccount(It.Is<Account>(a => a.Balance == 400)), Times.Once);
        }

        [Test]
        public void MakePayment_DoesNotReduceBalance_When_PaymentFails()
        {
            // Arrange
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "123456",
                Amount = 100,
                PaymentScheme = PaymentScheme.Bacs
            };

            var account = new Account
            {
                AccountNumber = "123456",
                Balance = 500,
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs
            };

            // Setup the mock AccountDataStore to return the account
            _mockAccountDataStore
                .Setup(store => store.GetAccount(request.DebtorAccountNumber))
                .Returns(account);

            // Setup the mock PaymentSchemeValidator to return false (invalid payment)
            _mockPaymentSchemeValidator
                .Setup(validator => validator.IsValid(request, account))
                .Returns(false);

            // Act
            var result = _paymentService.MakePayment(request);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(account.Balance, Is.EqualTo(500));
            _mockAccountDataStore.Verify(store => store.UpdateAccount(It.IsAny<Account>()), Times.Never);
        }
    }
}
