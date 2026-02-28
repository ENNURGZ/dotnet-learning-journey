# Transaction Processor

An intermediate-level task for practicing mock testing.

Estimated time to complete the task: 2 hour.

The task requires .NET 8 SDK installed.

# Task Description

This project contains an implementation of a transaction processing system that needs to be covered with comprehensive unit tests using mocking techniques. Your task is to write tests for the existing implementation using Moq framework to mock dependencies.

## Important Note
### DO NOT CHANGE THE FOLLOWING NAMES:

Project names: MockTesting and MockTesting.Tests

Type names: IAccountService, ILogger, IPermissionService, ITransactionService, TransactionProcessor

Test class name: TransactionProcessorTests

Namespaces: MockTesting, MockTesting.Tests

The correctness of the implementation will be verified using hidden tests (not included in the student's repository), and test coverage will be evaluated based on the tests written by the student. Changing any of these names will result in test failures.

## Project Structure

The solution consists of two projects:
- `MockTesting` - Contains the implementation of the transaction processing system
- `MockTesting.Tests` - Contains unit tests for the transaction processing system

## Implementation Requirements

### Core Components
- `TransactionProcessor` - Main class for processing transactions
- `IPermissionService` - Interface for permission checking
- `IAccountService` - Interface for account operations
- `ITransactionService` - Interface for transaction operations
- `ILogger` - Interface for logging

### Required Functionality
1. Transaction processing with validation
2. Permission checking
3. Account balance verification
4. Transaction execution
5. Logging of operations
6. Error handling
7. Input validation

## Testing Requirements

### Test Coverage
- Transaction processing success scenarios
  - Basic successful transfer
  - Transfer with maximum valid values
  - Transfer with minimum valid values
- Permission validation
  - Successful permission check
  - Permission denied scenario
- Balance verification
  - Sufficient balance
  - Insufficient balance
- Error handling
  - Exception handling
  - Error message formatting
- Input validation
  - Invalid user IDs
  - Invalid amount
  - Same user IDs
  - Null dependencies
- Logging verification
  - Log message content
  - Log message formatting
  - Error message formatting
- Mock object setup and verification
  - Proper mock setup
  - Method call verification
  - Transaction ID uniqueness

### Test Cases Should Cover
- Successful transactions
- Permission denied scenarios
- Insufficient balance cases
- Invalid input handling
- Exception handling
- Logging verification
- Edge cases
  - Maximum and minimum valid values
  - Transaction ID uniqueness
  - Message formatting

## Example Test Cases

```csharp
// Example of testing a successful scenario
[Test]
public void ProcessTransfer_WhenAllConditionsMet_ShouldSucceed()
{
    // Arrange
    // Setup mocks for successful scenario

    // Act
    // Call the method under test

    // Assert
    // Verify the result and mock interactions
}

// Example of testing a failure scenario
[Test]
public void ProcessTransfer_WhenPermissionDenied_ShouldFail()
{
    // Arrange
    // Setup mocks for permission denied scenario

    // Act
    // Call the method under test

    // Assert
    // Verify the result and mock interactions
}
```