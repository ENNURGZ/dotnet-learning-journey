using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace ValidationServiceTask.Tests
{
    [TestFixture]
    public class RequiredAttributeTests
    {
        [TestCase("test", ExpectedResult = true)]
        [TestCase("", ExpectedResult = false)]
        [TestCase(null, ExpectedResult = false)]
        [TestCase(new int[] { 1, 2, 3 }, ExpectedResult = true)]
        [TestCase(99.98, ExpectedResult = true)]
        [TestCase('c', ExpectedResult = true)]
        public bool IsValid_RequiredAttribute_ValidatesCorrectly(object value)
        {
            var attribute = new RequiredAttribute();
            return attribute.IsValid(value);
        }
    }
}
