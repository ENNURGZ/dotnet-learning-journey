using NUnit.Framework;
using ValidationAttributes;

namespace ValidationServiceTask.Tests
{
    [TestFixture]
    public class ValidationAttributeTests
    {
        [Test]
        public void Constructor_WithNoParameters_SetsDefaultErrorMessage()
        {
            // Arrange & Act
            var attribute = new TestValidationAttribute();

            // Assert
            Assert.That(attribute.ErrorMessage, Is.EqualTo("The field is invalid."));
        }

        [Test]
        public void Constructor_WithCustomErrorMessage_SetsCustomMessage()
        {
            // Arrange
            const string customMessage = "Custom error message";

            // Act
            var attribute = new TestValidationAttribute(customMessage);

            // Assert
            Assert.That(attribute.ErrorMessage, Is.EqualTo(customMessage));
        }

        [Test]
        public void Constructor_WithNullErrorMessage_SetsDefaultMessage()
        {
            // Arrange & Act
            var attribute = new TestValidationAttribute(null!);

            // Assert
            Assert.That(attribute.ErrorMessage, Is.EqualTo("The field is invalid."));
        }

        [Test]
        public void ErrorMessage_CanBeModified()
        {
            // Arrange
            var attribute = new TestValidationAttribute();
            const string newMessage = "New error message";

            // Act
            attribute.ErrorMessage = newMessage;

            // Assert
            Assert.That(attribute.ErrorMessage, Is.EqualTo(newMessage));
        }

        [Test]
        public void IsValid_WithValidValue_ReturnsTrue()
        {
            // Arrange
            var attribute = new TestValidationAttribute();
            var validValue = "test";

            // Act
            bool result = attribute.IsValid(validValue);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_WithNullValue_ReturnsFalse()
        {
            // Arrange
            var attribute = new TestValidationAttribute();

            // Act
            bool result = attribute.IsValid(null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void AttributeUsage_AllowsMultipleAttributes()
        {
            // Arrange
            var attributeUsage = typeof(ValidationAttribute)
                .GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                .FirstOrDefault() as AttributeUsageAttribute;

            // Assert
            Assert.That(attributeUsage, Is.Not.Null);
            Assert.That(attributeUsage!.AllowMultiple, Is.True);
        }

        [Test]
        public void AttributeUsage_SupportsInheritance()
        {
            // Arrange
            var attributeUsage = typeof(ValidationAttribute)
                .GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                .FirstOrDefault() as AttributeUsageAttribute;

            // Assert
            Assert.That(attributeUsage, Is.Not.Null);
            Assert.That(attributeUsage!.Inherited, Is.True);
        }

        [Test]
        public void AttributeUsage_ValidTargets()
        {
            // Arrange
            var attributeUsage = typeof(ValidationAttribute)
                .GetCustomAttributes(typeof(AttributeUsageAttribute), true)
                .FirstOrDefault() as AttributeUsageAttribute;

            // Assert
            Assert.That(attributeUsage, Is.Not.Null);
            Assert.That(attributeUsage!.ValidOn, Is.EqualTo(
                AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter));
        }

        [AttributeUsage(AttributeTargets.All)]
        private sealed class TestValidationAttribute : ValidationAttribute
        {
            public TestValidationAttribute()
            {
            }

            public TestValidationAttribute(string errorMessage)
                : base(errorMessage)
            {
            }

            public override bool IsValid(object? value)
            {
                // Simple implementation for testing - validates if value is not null
                return value != null;
            }
        }
    }
}
