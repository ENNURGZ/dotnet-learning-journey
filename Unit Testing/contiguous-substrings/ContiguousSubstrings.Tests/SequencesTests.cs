using NUnit.Framework;

namespace ContiguousSubstrings.Tests
{
    [TestFixture]
    public class SequencesTests
    {
        private static readonly object[] ValidCases =
{
    new object[] { "1", 1, new[] { "1" } },
    new object[] { "12", 1, new[] { "1", "2" } },
    new object[] { "35", 2, new[] { "35" } },
    new object[] { "9142", 2, new[] { "91", "14", "42" } },
    new object[] { "777777", 3, new[] { "777", "777", "777", "777" } },
    new object[]
    {
        "918493904243",
        5,
        new[]
        {
            "91849",
            "18493",
            "84939",
            "49390",
            "93904",
            "39042",
            "90424",
            "04243",
        },
    },
};

        [TestCaseSource(nameof(ValidCases))]
        public void GetSubstrings_ValidInput_ReturnsExpectedResult(
            string input,
            int length,
            string[] expected)
        {
            var result = Sequences
                .GetSubstrings(input, length)
                .ToArray();

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetSubstrings_LengthEqualsInputLength_ReturnsSingleSubstring()
        {
            var result = Sequences
                .GetSubstrings("12345", 5)
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo("12345"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void GetSubstrings_InvalidString_ThrowsArgumentException(string input)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Sequences.GetSubstrings(input, 1));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void GetSubstrings_InvalidLength_ThrowsArgumentException(int length)
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Sequences.GetSubstrings("12345", length));
        }

        [Test]
        public void GetSubstrings_LengthGreaterThanInput_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Sequences.GetSubstrings("12345", 6));
        }

        [Test]
        public void GetSubstrings_ContainsNonDigit_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() =>
                Sequences.GetSubstrings("123a", 2));
        }
    }
}
