namespace ContiguousSubstrings
{
    /// <summary>
    /// Provides methods for working with sequences of digits and their substrings.
    /// </summary>
    public static class Sequences
    {
        public static IEnumerable<string> GetSubstrings(string numbers, int length)
        {
            if (string.IsNullOrWhiteSpace(numbers))
            {
                throw new ArgumentException(
                    "Input string cannot be null, empty, or whitespace.",
                    nameof(numbers));
            }

            if (length <= 0)
            {
                throw new ArgumentException(
                    "Length must be greater than zero.",
                    nameof(length));
            }

            if (length > numbers.Length)
            {
                throw new ArgumentException(
                    "Length cannot be greater than input string length.",
                    nameof(length));
            }

            for (int i = 0; i < numbers.Length; i++)
            {
                if (!char.IsDigit(numbers[i]))
                {
                    throw new ArgumentException(
                        "Input string must contain only digits.",
                        nameof(numbers));
                }
            }

            var result = new List<string>();

            for (int i = 0; i <= numbers.Length - length; i++)
            {
                result.Add(numbers.Substring(i, length));
            }

            return result;
        }
    }
}
