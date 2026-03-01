using NUnit.Framework;
using ValidationAttributes;

namespace ValidationServiceTask.Tests
{
    public class NumericRangeAttributeTests
    {
        [TestCase("invalid", ExpectedResult = false)]
        [TestCase("text", ExpectedResult = false)]
        [TestCase(true, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 3 }, ExpectedResult = false)]
        [TestCase(99.98, ExpectedResult = false)]
        [TestCase('c', ExpectedResult = false)]
        public bool IsValid_InvalidType_ReturnFalse(object value)
        {
            var attribute = new NumericRangeAttribute(1, 10);
            return attribute.IsValid(value);
        }

        [TestCase("invalid", ExpectedResult = false)]
        [TestCase("text", ExpectedResult = false)]
        [TestCase(true, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 3 }, ExpectedResult = false)]
        [TestCase('c', ExpectedResult = false)]
        public bool IsValid_InvalidType1_ReturnFalse(object value)
        {
            var attribute = new NumericRangeAttribute(1.9, 10.99);
            return attribute.IsValid(value);
        }

        [TestCase(10, 1)]
        [TestCase((byte)10, (byte)1)]
        [TestCase((short)10, (short)10)]
        [TestCase(-10, -10)]
        [TestCase((byte)10, (byte)10)]
        [TestCase((short)10, (short)10)]
        [TestCase((ushort)10, (ushort)10)]
        public void IsValid_MinGreaterThanMax_ThrowsArgumentException(int minimum, int maximum)
        {
            var attribute = new NumericRangeAttribute(minimum, maximum);
#pragma warning disable IDE0058
            Assert.Throws<ArgumentException>(() => attribute.IsValid(5));
#pragma warning restore IDE0058
        }

        [TestCase(12, 10, 20)]
        [TestCase((byte)15, (byte)10, (byte)20)]
        [TestCase((short)15, (short)10, (short)20)]
        [TestCase(-12, -20, -10)]
        [TestCase((byte)15, (byte)10, (byte)20)]
        [TestCase((short)15, (short)10, (short)20)]
        public void IsValid_MinEqualToMax_ValidatesCorrectly(int value, int minimum, int maximum)
        {
            var attribute = new NumericRangeAttribute(minimum, maximum);
            Assert.That(attribute.IsValid(value), Is.True);
        }

        [TestCase(5.07, 1.89, 10.987, ExpectedResult = true)]
        [TestCase(-5.07, 1.89, 10.987, ExpectedResult = false)]
        [TestCase(-5.07, -10.987, -1.89, ExpectedResult = true)]
        [TestCase(5.07, -10.987, -1.89, ExpectedResult = false)]
        public bool IsValid_DoubleRange_ValidatesCorrectly(double value, double minimum, double maximum)
        {
            var attribute = new NumericRangeAttribute(minimum, maximum);
            return attribute.IsValid(value);
        }

        [TestCase(5.07, 1.89, 10.987, typeof(decimal), ExpectedResult = true)]
        [TestCase(-5.07, 1.89, 10.987, typeof(decimal), ExpectedResult = false)]
        [TestCase(-5.07, -10.987, -1.89, typeof(decimal), ExpectedResult = true)]
        [TestCase(5.07, -10.987, -1.89, typeof(decimal), ExpectedResult = false)]
        public bool IsValid_ObjectRange_ValidatesCorrectly(object value, object minimum, object maximum, Type type)
        {
            var attribute = new NumericRangeAttribute(minimum, maximum, type);
            return attribute.IsValid(value);
        }

        [TestCase("string", ExpectedResult = false)]
        public bool IsNumericPrimitive_NonNumericType_ReturnsFalse(object value)
        {
            var attribute = new NumericRangeAttribute(12, 67);
            return attribute.IsValid(value);
        }
    }
}
