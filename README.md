# ClearBank Developer Test
adhering to DRY and SOLID principles

## First analysis
To refactor the code in according to SOLID principles, while enhancing testability and readability, I broke it down into smaller, more maintainable components. 
The refactoring was done in two key steps detailed below. After these changes, the code adheres to SOLID principles (though the Liskov Substitution Principle was not required in this case).


## First step
First, I started by extracting the logic for creating an AccountDataStore into an AccountDataStoreFactory class. This class is responsible for determining and returning the correct AccountDataStore instance, improving the separation of concerns.
To facilitate unit testing the AccountDataStoreFactory, I introduced a ConfigurationManagerAdapter that implements the IConfigurationManager interface. This adapter abstracts the System.Configuration.ConfigurationManager, allowing to inject a mock version during testing. 
This makes the factory easily testable, as I can control configuration behavior in unit tests.

For the factory itself, I designed it to implement the IAccountDataStoreFactory interface. This follows the Dependency Inversion Principle (DIP), making the PaymentService independent of specific data store implementations. 
By doing this, we ensure that new data stores can be added or swapped out without modifying the PaymentService, and it simplifies unit testing by allowing us to mock the factory.

In this first step, I've also felt the need to re-organize project folders to better help me locate classes/interfaces.

## Second step
The second part of the refactoring focused on handling the validations based on request.PaymentScheme. I extracted an IPaymentSchemeValidator interface and created specific validators for each payment scheme—Bacs, FasterPayments, and Chaps—each implementing the interface. The PaymentSchemeValidator is now responsible for determining which specific validator to use based on the payment scheme.

By abstracting the validation logic into separate classes, the code adheres to the Open/Closed Principle (OCP). This allows for the addition of new payment schemes validations without modifying the PaymentService, making the solution more scalable and maintainable.