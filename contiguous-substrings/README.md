# Contiguous Substrings Task

Intermediate level task for practicing enums and bit flags.

Estimated time to complete the task - 1.5h.

The task requires .NET 8 SDK installed.

## Task Description
Implement the `GetSubstrings` method in the `Sequences` class that finds all contiguous substrings of a given length in a string of digits. The method should be implemented in C# and covered with comprehensive unit tests using the NUnit testing framework.

## Requirements

### Implementation Requirements
1. Implement the `GetSubstrings` method in the `Sequences` class that takes two parameters:
   - `numbers`: A string containing only digits
   - `length`: The length of the substrings to extract

2. The method should return all contiguous substrings of the specified length in the order they appear in the input string.

3. The method should throw `ArgumentException` in the following cases:
   - If the length parameter is less than or equal to zero
   - If the length parameter is greater than the length of the input string
   - If the input string contains any non-digit characters
   - If the input string is null, empty, or consists only of whitespace

### Test Coverage Requirements
The solution must achieve at least 90% code coverage, including:
- All branches of the code
- All exception handling paths
- All edge cases
- All input validation logic

#### Test Categories
1. **Positive Test Cases**
   - Basic functionality tests
   - Boundary value tests
   - Various input combinations

2. **Negative Test Cases**
   - Input validation tests
   - Edge case tests
   - Exception handling tests

## Example Test Cases

### Basic Cases
```csharp
GetSubstrings("1", 1) → ["1"]
GetSubstrings("12", 1) → ["1", "2"]
GetSubstrings("35", 2) → ["35"]
GetSubstrings("9142", 2) → ["91", "14", "42"]
GetSubstrings("777777", 3) → ["777", "777", "777", "777"]
GetSubstrings("918493904243", 5) → ["91849", "18493", "84939", "49390", "93904", "39042", "90424", "04243"]
```

### Error Cases
```csharp
GetSubstrings("123a", 1) → ArgumentException
GetSubstrings("12345", 6) → ArgumentException
GetSubstrings("12345", 0) → ArgumentException
GetSubstrings("123", -1) → ArgumentException
GetSubstrings("", 1) → ArgumentException
GetSubstrings(null, 1) → ArgumentException
GetSubstrings("   ", 1) → ArgumentException
```

## Testing Framework
The project uses NUnit for unit testing. Key features used:
- `[TestFixture]` - Marks a class that contains tests
- `[Test]` - Marks a method as a test method
- `[TestCase]` - Specifies multiple test cases for a single test method
- Various Assert methods for verification

## Evaluation Criteria
1. Correct implementation of the `GetSubstrings` method
2. Proper handling of all edge cases and error conditions
3. Complete test coverage (minimum 90%)
4. Code quality and readability
5. Adherence to C# best practices
6. Proper organization of test cases
7. Comprehensive coverage of all possible scenarios

## Getting Started
1. Clone the repository
2. Open the solution in your preferred IDE
3. Implement the `GetSubstrings` method in `Sequences.cs`
4. Add tests in `SequencesTests.cs`
5. Run the tests to verify your implementation

## Notes
- Follow C# best practices and coding standards
- Ensure all tests pass before submitting the solution
- Use code coverage tools to verify test coverage
- Document any assumptions or design decisions in the code