using NUnit.Framework;
using ValidationAttributes;
using ValidationServiceTask.Tests.AttributeTestClasses;

namespace ValidationServiceTask.Tests
{
    [TestFixture]
    public class ValidationServiceTests
    {
        private static IEnumerable<TestCaseData> TestCasesForRequiredAttribute
        {
            get
            {
                yield return new TestCaseData(
                    "test",
                    new InnerClass(),
                    true,
                    0,
                    null,
                    null);
                yield return new TestCaseData(
                    null,
                    new InnerClass(),
                    false,
                    1,
                    new[] { nameof(RequiredTestClass.StringField) },
                    new[] { "The field is required." });
                yield return new TestCaseData(
                    "test",
                    null,
                    false,
                    1,
                    new[] { nameof(RequiredTestClass.InnerClassProperty) },
                    new[] { "InnerClassProperty is required." });
                yield return new TestCaseData(
                    null,
                    null,
                    false,
                    2,
                    new[] { nameof(RequiredTestClass.StringField), nameof(RequiredTestClass.InnerClassProperty) },
                    new[] { "The field is required.", "InnerClassProperty is required." });
            }
        }

        private static IEnumerable<TestCaseData> TestCasesForStringLengthAttribute
        {
            get
            {
                yield return new TestCaseData("validLengthString", "someLengthString", true, 0, null, null);
                yield return new TestCaseData(
                    "validLengthString",
                    "short",
                    false,
                    1,
                    new[] { nameof(StringLengthTestClass.NullableStringProperty) },
                    new[] { "The string length must be greater than or equal 10 and less than or equal 25." });
                yield return new TestCaseData(
                    "short",
                    "validLengthString",
                    false,
                    1,
                    new[] { nameof(StringLengthTestClass.NotNullableStringProperty) },
                    new[] { "String field length must be between 10 and 25." });
                yield return new TestCaseData(
                    "validLengthString",
                    null,
                    true,
                    0,
                    null,
                    null);
                yield return new TestCaseData(
                    null,
                    "validLengthString",
                    false,
                    1,
                    new[] { nameof(StringLengthTestClass.NotNullableStringProperty) },
                    new[] { "String field is required." });
                yield return new TestCaseData(
                    null,
                    "short",
                    false,
                    2,
                    new[]
                    {
                    nameof(StringLengthTestClass.NotNullableStringProperty),
                    nameof(StringLengthTestClass.NullableStringProperty),
                    },
                    new[]
                    {
                    "String field is required.",
                    "The string length must be greater than or equal 10 and less than or equal 25.",
                    });
                yield return new TestCaseData(
                    null,
                    null,
                    false,
                    1,
                    new[] { nameof(StringLengthTestClass.NotNullableStringProperty) },
                    new[] { "String field is required." });
                yield return new TestCaseData(
                    "validLengthString",
                    "exceedinglyLongStringThatExceedsTheMaximumAllowedLength",
                    false,
                    1,
                    new[] { nameof(StringLengthTestClass.NullableStringProperty) },
                    new[] { "The string length must be greater than or equal 10 and less than or equal 25." });
            }
        }

        private static IEnumerable<TestCaseData> TestCasesForRangeAttribute
        {
            get
            {
                yield return new TestCaseData(
                    new NumericRangeTestClass
                    {
                        ByteField = 5,
                        LongField = 0L,
                        SbyteField = 0,
                        ShortField = 5,
                        UshortProperty = 9,
                        IntegerProperty = 0,
                        UintProperty = 5u,
                        UlongProperty = 50UL,
                        FloatProperty = 5.0f,
                        DoubleProperty = 105.0,
                        DecimalProperty = 50m,
                    },
                    true,
                    0,
                    null,
                    null);
                yield return new TestCaseData(
                    new NumericRangeTestClass
                    {
                        ByteField = 0,
                        SbyteField = 20,
                        ShortField = 20,
                        UshortProperty = 45,
                        IntegerProperty = 200,
                        UintProperty = 230u,
                        UlongProperty = 0UL,
                        LongField = -15L,
                        FloatProperty = 25.6f,
                        DoubleProperty = 112.5,
                        DecimalProperty = 50m,
                    },
                    false,
                    10,
                    new[]
                    {
                    nameof(NumericRangeTestClass.ByteField), nameof(NumericRangeTestClass.SbyteField),
                    nameof(NumericRangeTestClass.ShortField), nameof(NumericRangeTestClass.UshortProperty),
                    nameof(NumericRangeTestClass.IntegerProperty), nameof(NumericRangeTestClass.UintProperty),
                    nameof(NumericRangeTestClass.LongField), nameof(NumericRangeTestClass.UlongProperty),
                    nameof(NumericRangeTestClass.FloatProperty), nameof(NumericRangeTestClass.DoubleProperty),
                    },
                    new string[]
                    {
                    "The field must be between 1 and 10.", "The value of SbyteField must be between -8 and 10.",
                    "The value of ShortField must be between -100 and 10.",
                    "The value of UshortProperty must be between 5 and 10.",
                    "The value of IntegerProperty must be between -123 and 123.",
                    "The value of UintProperty must be between 1 and 100.",
                    "The value of LongField must be between -10 and 10.",
                    "The value of UlongProperty must be between 10 and 1000.",
                    "The value of FloatProperty must be between 1.5 and 10.5.",
                    "The value of DoubleProperty must be between 100.5 and 105.5.",
                    });
                yield return new TestCaseData(
                    new NumericRangeTestClass
                    {
                        ByteField = 0,
                        LongField = 0L,
                        SbyteField = 0,
                        ShortField = 5,
                        UshortProperty = 9,
                        IntegerProperty = 0,
                        UintProperty = 5u,
                        UlongProperty = 50UL,
                        FloatProperty = 5.0f,
                        DoubleProperty = 105.0,
                        DecimalProperty = 50m,
                    },
                    false,
                    1,
                    new[] { nameof(NumericRangeTestClass.ByteField) },
                    new[] { $"The field must be between 1 and 10." });
                yield return new TestCaseData(
                    new NumericRangeTestClass
                    {
                        ByteField = 1,
                        LongField = 0L,
                        SbyteField = 0,
                        ShortField = 5,
                        UshortProperty = 9,
                        IntegerProperty = 0,
                        UintProperty = 5u,
                        UlongProperty = 50UL,
                        FloatProperty = 5.0f,
                        DoubleProperty = 105.0,
                        DecimalProperty = 0m,
                    },
                    false,
                    1,
                    new[] { nameof(NumericRangeTestClass.DecimalProperty) },
                    new[] { "The value of DecimalProperty must be between 10 and 1000." });
                yield return new TestCaseData(
                    new NumericRangeTestClass
                    {
                        ByteField = 5,
                        LongField = 100L,
                        SbyteField = 0,
                        ShortField = 5,
                        UshortProperty = 9,
                        IntegerProperty = 500,
                        UintProperty = 5u,
                        UlongProperty = 50UL,
                        FloatProperty = 5.0f,
                        DoubleProperty = 305.0,
                        DecimalProperty = 50m,
                    },
                    false,
                    3,
                    new[]
                    {
                    nameof(NumericRangeTestClass.IntegerProperty), nameof(NumericRangeTestClass.LongField),
                    nameof(NumericRangeTestClass.DoubleProperty),
                    },
                    new string[]
                    {
                    "The value of IntegerProperty must be between -123 and 123.",
                    "The value of LongField must be between -10 and 10.",
                    "The value of DoubleProperty must be between 100.5 and 105.5.",
                    });
            }
        }

        private static IEnumerable<TestCaseData> TestCasesForInheritanceVerification
        {
            get
            {
                yield return new TestCaseData(
                    typeof(RequiredAttribute),
                    true);
                yield return new TestCaseData(
                    typeof(StringLengthAttribute),
                    true);
                yield return new TestCaseData(
                    typeof(NumericRangeAttribute),
                    true);
            }
        }

        [TestCaseSource(nameof(TestCasesForRequiredAttribute))]
        public void RequiredAttributeTest(
            string stringField,
            InnerClass innerClassProperty,
            bool expectedIsValid,
            int expectedErrorCount,
            string?[]? expectedMemberName = null!,
            string?[]? expectedErrorMessage = null!)
        {
            var service = new ValidationService<RequiredTestClass>();
            var testObject = new RequiredTestClass { StringField = stringField, InnerClassProperty = innerClassProperty, };
            Assert.That(expectedIsValid == service.IsValid(testObject));
            Assert.That(expectedErrorCount == service.ValidationInfo.Count);
            for (int i = 0; i < expectedErrorCount; i++)
            {
                Assert.That(
                    expectedErrorMessage?[i],
                    Is.EquivalentTo(service.ValidationInfo[expectedMemberName?[i]!].FirstOrDefault()?.ValidationMessage!));
            }
        }

        [TestCaseSource(nameof(TestCasesForStringLengthAttribute))]
        public void StringLengthAttributeTest(
            string notNullableStringProperty,
            string? nullableStringProperty,
            bool expectedIsValid,
            int expectedErrorCount,
            string?[]? expectedMemberName = null,
            string?[]? expectedErrorMessage = null)
        {
            var service = new ValidationService<StringLengthTestClass>();
            var testObject = new StringLengthTestClass
            {
                NotNullableStringProperty = notNullableStringProperty,
                NullableStringProperty = nullableStringProperty,
            };
            Assert.That(expectedIsValid == service.IsValid(testObject));
            Assert.That(expectedErrorCount == service.ValidationInfo.Count);
            for (int i = 0; i < expectedErrorCount; i++)
            {
                Assert.That(
                    expectedErrorMessage?[i],
                    Is.EquivalentTo(service.ValidationInfo[expectedMemberName?[i]!].FirstOrDefault()?.ValidationMessage!));
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCasesForRangeAttribute))]
        public void RangeAttributeTests(
            NumericRangeTestClass testObject,
            bool expectedIsValid,
            int expectedErrorCount = 0,
            string?[]? expectedMemberName = null,
            string?[]? expectedErrorMessage = null)
        {
            var service = new ValidationService<NumericRangeTestClass>();
            Assert.That(expectedIsValid == service.IsValid(testObject));
            Assert.That(expectedErrorCount == service.ValidationInfo.Count);
            for (int i = 0; i < expectedErrorCount; i++)
            {
                Assert.That(
                    expectedErrorMessage?[i],
                    Is.EquivalentTo(service.ValidationInfo[expectedMemberName?[i]!].FirstOrDefault()?.ValidationMessage!));
            }
        }

        [TestCaseSource(nameof(TestCasesForInheritanceVerification))]
        public void AttributesClasses_ShouldInheritFrom_ValuesAttributeClass(Type attributeType, bool expected)
        {
            Assert.That(attributeType.BaseType == typeof(ValidationAttribute), Is.EqualTo(expected));
        }

        [Test]
        public void BaseClass_Is_Abstract()
        {
            Assert.That(typeof(ValidationAttribute).IsAbstract, Is.True);
        }
    }
}
