using Business.Validation;

namespace Business.Tests;

/// <summary>
/// Unit tests for MarketException class.
/// </summary>
public class MarketExceptionTests
{
    [Test]
    public void Constructor_Default_ShouldCreateException()
    {
        // Act
        var exception = new MarketException();

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Exception of type 'Business.Validation.MarketException' was thrown."));
    }

    [Test]
    public void Constructor_WithMessage_ShouldCreateExceptionWithMessage()
    {
        // Arrange
        var expectedMessage = "Test error message";

        // Act
        var exception = new MarketException(expectedMessage);

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Constructor_WithMessageAndInnerException_ShouldCreateExceptionWithMessageAndInnerException()
    {
        // Arrange
        var expectedMessage = "Test error message";
        var innerException = new ArgumentException("Inner exception");

        // Act
        var exception = new MarketException(expectedMessage, innerException);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        });
    }

    [Test]
    public void Constructor_WithNullMessage_ShouldCreateExceptionWithDefaultMessage()
    {
        // Act
        var exception = new MarketException(null!);

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Exception of type 'Business.Validation.MarketException' was thrown."));
    }

    [Test]
    public void Constructor_WithEmptyMessage_ShouldCreateExceptionWithEmptyMessage()
    {
        // Arrange
        var expectedMessage = string.Empty;

        // Act
        var exception = new MarketException(expectedMessage);

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Constructor_WithWhitespaceMessage_ShouldCreateExceptionWithWhitespaceMessage()
    {
        // Arrange
        var expectedMessage = "   ";

        // Act
        var exception = new MarketException(expectedMessage);

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Constructor_WithLongMessage_ShouldCreateExceptionWithLongMessage()
    {
        // Arrange
        var expectedMessage = new string('A', 1000);

        // Act
        var exception = new MarketException(expectedMessage);

        // Assert
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo(expectedMessage));
    }

    [Test]
    public void Constructor_WithMessageAndNullInnerException_ShouldCreateExceptionWithMessageAndNullInnerException()
    {
        // Arrange
        var expectedMessage = "Test error message";

        // Act
        var exception = new MarketException(expectedMessage, null!);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
            Assert.That(exception.InnerException, Is.Null);
        });
    }

    [Test]
    public void Constructor_WithMessageAndInnerException_ShouldPreserveStackTrace()
    {
        // Arrange
        var expectedMessage = "Test error message";
        Exception innerException;

        try
        {
            throw new InvalidOperationException("Inner exception");
        }
        catch (Exception ex)
        {
            innerException = ex;
        }

        // Act
        var exception = new MarketException(expectedMessage, innerException);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
            Assert.That(exception.InnerException!.Message, Is.EqualTo("Inner exception"));
        });
    }

    [Test]
    public void Constructor_ShouldInheritFromException()
    {
        // Act
        var exception = new MarketException();

        // Assert
        Assert.That(exception, Is.InstanceOf<Exception>());
    }

    [Test]
    public void Constructor_ShouldBeSerializable()
    {
        // Act
        var exception = new MarketException("Test message");

        // Assert
        Assert.That(exception, Is.InstanceOf<System.Runtime.Serialization.ISerializable>());
    }
}
