# HSL Color Struct Implementation and Testing

The intermediate level task for implementing a structure that represents a color in the HSL (Hue, Saturation, Lightness) format and creating comprehensive unit tests for it.

Estimated time to complete the task - 2.5h.

The task requires .NET 8 SDK installed.

## Important Note

**DO NOT CHANGE THE FOLLOWING NAMES:**
- Project names: `HslColorStruct` and `HslColorStruct.Tests`
- Structure name: `HslColor`
- Test class name: `HslColorTests`
- Namespaces: `HslColorStruct`, `HslColorStruct.Tests`

The correctness of the implementation will be verified using hidden tests (not included in the student's repository), and test coverage will be evaluated based on the tests written by the student. Changing any of these names will result in test failures.

## Project Structure

The solution consists of two projects:
- `HslColorStruct` - Contains the implementation of the `HslColor` structure
- `HslColorStruct.Tests` - Contains unit tests for the `HslColor` structure

## Task Description

Implement a structure `HslColor` that represents a color in the HSL (Hue, Saturation, Lightness) color model and create a comprehensive test suite for it.

### Requirements for Structure Implementation

1. Create a structure `HslColor` with the following characteristics:
   - Immutable and readonly struct
   - Hue range: 0-360 degrees
   - Saturation range: 0-100%
   - Lightness range: 0-100%
   - String format: "H,S,L" with integer values
   - Equality comparison for all components

2. Implement the following functionality:
   - Constructor with parameter validation
   - Property getters (Hue, Saturation, Lightness)
   - Static `Create` method with parameter validation
   - `Parse` method for converting string to HslColor
   - `TryParse` method for safe string conversion
   - Equality comparison methods (`Equals`, `==`, `!=`)
   - `GetHashCode` method
   - `ToString` method

### Requirements for Test Implementation

1. Create unit tests for all implemented functionality:
   - Constructor validation
   - Property getters
   - Static `Create` method
   - `Parse` and `TryParse` methods
   - Equality comparison methods
   - `GetHashCode` method
   - `ToString` method

2. Test cases should cover:
   - Valid input values
   - Invalid input values (out of range)
   - Edge cases
   - Null values where applicable
   - String parsing edge cases
   - Equality comparison edge cases
   - Hash code distribution

3. Test naming should follow the Arrange-Act-Assert pattern and be descriptive.

4. Use appropriate assertion methods to verify:
   - Expected values
   - Exception throwing
   - Object equality
   - String formatting

### Testing Framework

The project uses [NUnit](https://nunit.org/) testing framework version 4.0.1.

### Example Test Cases

```csharp
// Constructor validation
[Test]
public void Constructor_WithValidValues_CreatesColor()
{
    // Arrange
    double hue = 120;
    double saturation = 50;
    double lightness = 75;

    // Act
    var color = new HslColor(hue, saturation, lightness);

    // Assert
    Assert.That(color.Hue, Is.EqualTo(hue));
    Assert.That(color.Saturation, Is.EqualTo(saturation));
    Assert.That(color.Lightness, Is.EqualTo(lightness));
}

// String parsing
[Test]
public void Parse_WithValidString_ReturnsColor()
{
    // Arrange
    string hslString = "120,50,75";

    // Act
    var color = HslColor.Parse(hslString);

    // Assert
    Assert.That(color.Hue, Is.EqualTo(120));
    Assert.That(color.Saturation, Is.EqualTo(50));
    Assert.That(color.Lightness, Is.EqualTo(75));
}
```

### Code Style and Formatting

The project follows these code style guidelines:
- Uses `.editorconfig` for consistent code formatting
- Uses StyleCop for code analysis
- Follows C# coding conventions
- Uses XML documentation for public members
- Uses meaningful variable and method names

### Test Coverage

The test suite provides comprehensive coverage for:
- All public methods and properties
- Edge cases and boundary conditions
- Invalid input handling
- String parsing and formatting
- Equality comparison
- Hash code generation

For a simple explanation of what code coverage is and why it's important, see [this article](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage).

