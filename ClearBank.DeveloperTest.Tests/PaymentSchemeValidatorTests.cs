using ClearBank.DeveloperTest.Validators;
using NUnit.Framework;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentSchemeValidatorTests
    {
        private PaymentSchemeValidator _paymentSchemeValidator;

        [SetUp]
        public void Setup()
        {
            _paymentSchemeValidator = new PaymentSchemeValidator();
        }

        [Test]
        public void IsValid_ReturnsTrue_When_BacsPaymentScheme()
        {
            // Arrange
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs};

            // Act
            var result = _paymentSchemeValidator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.True);
        }

        [TestCase(666, true)]
        [TestCase(1000, true)]
        [TestCase(300, false)]
        public void IsValid_ReturnsTrue_When_FasterPaymentScheme_And_EnoughBalance(decimal accountBalance, bool expectedResult)
        {
            // Arrange
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments, Amount = 666 };
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments, Balance = accountBalance };

            // Act
            var result = _paymentSchemeValidator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(AccountStatus.Live, true)]
        [TestCase(AccountStatus.InboundPaymentsOnly, false)]
        [TestCase(AccountStatus.Disabled, false)]
        public void IsValid_ReturnsTrue_When_ChapsPaymentScheme_And_AccountLive(AccountStatus status, bool expectedResult)
        {
            // Arrange
            var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };
            var account = new Account() { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = status };

            // Act
            var result = _paymentSchemeValidator.IsValid(request, account);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(PaymentScheme.Bacs, false)]
        [TestCase(PaymentScheme.FasterPayments, false)]
        [TestCase(PaymentScheme.Chaps, false)]
        public void IsValid_ReturnsFalse_When_NullAccount(PaymentScheme scheme, bool expectedResult)
        {
            // Arrange
            var request = new MakePaymentRequest { PaymentScheme = scheme };
 
            // Act
            var result = _paymentSchemeValidator.IsValid(request, null);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
