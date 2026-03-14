using System.Collections.Concurrent;

namespace PalindromeNumberFiltering;

/// <summary>
/// A static class containing methods for filtering palindrome numbers from a collection of integers.
/// </summary>
public static class Selector
{
    /// <summary>
    /// Retrieves a collection of palindrome numbers from the given list of integers using sequential filtering.
    /// </summary>
    /// <param name="numbers">The list of integers to filter.</param>
    /// <returns>A collection of palindrome numbers.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
    public static IList<int> GetPalindromeInSequence(IList<int>? numbers)
    {
        if (numbers is null)
        {
            throw new ArgumentNullException(nameof(numbers), "Array can not be null.");
        }

        List<int> result = new List<int>();
        foreach (var number in numbers)
        {
            if (IsPalindrome(number))
            {
                result.Add(number);
            }
        }

        return result;
    }

    /// <summary>
    /// Retrieves a collection of palindrome numbers from the given list of integers using parallel filtering.
    /// </summary>
    /// <param name="numbers">The list of integers to filter.</param>
    /// <returns>A collection of palindrome numbers.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input list 'numbers' is null.</exception>
    public static IList<int> GetPalindromeInParallel(IList<int> numbers)
    {
        if (numbers is null)
        {
            throw new ArgumentNullException(nameof(numbers), "Array can not be null.");
        }

        ConcurrentBag<int> result = new ConcurrentBag<int>();
        Parallel.ForEach(numbers, number =>
        {
            if (IsPalindrome(number))
            {
                result.Add(number);
            }
        });

        return result.ToList();
    }

    /// <summary>
    /// Checks whether the given integer is a palindrome number.
    /// </summary>
    /// <param name="number">The integer to check.</param>
    /// <returns>True if the number is a palindrome, otherwise false.</returns>
    private static bool IsPalindrome(int number)
    {
        if (number < 0)
        {
            return false;
        }

        if (number < 10)
        {
            return true;
        }

        byte length = GetLength(number);
        int divider = 1;
        for (int i = 1; i < length; i++)
        {
            divider *= 10;
        }

        return IsPositiveNumberPalindrome(number, divider);
    }

    /// <summary>
    /// Recursively checks whether a positive number is a palindrome.
    /// </summary>
    /// <param name="number">The positive number to check.</param>
    /// <param name="divider">The divider used in the recursive check.</param>
    /// <returns>True if the positive number is a palindrome, otherwise false.</returns>
    private static bool IsPositiveNumberPalindrome(int number, int divider)
    {
        if (number < 10)
        {
            return true;
        }

        int firstDigit = number / divider;
        int lastDigit = number % 10;

        if (firstDigit != lastDigit)
        {
            return false;
        }

        int remainingNumber = (number % divider) / 10;
        int nextDivider = divider / 100;

        // If nextDivider becomes 0, it means we don't have middle digits to check
        if (nextDivider <= 0)
        {
            return true;
        }

        return IsPositiveNumberPalindrome(remainingNumber, nextDivider);
    }

    /// <summary>
    /// Gets the number of digits in the given integer.
    /// </summary>
    /// <param name="number">The integer to count digits for.</param>
    /// <returns>The number of digits in the integer.</returns>
    private static byte GetLength(int number) => number switch
    {
        < 10 => 1,
        < 100 => 2,
        < 1000 => 3,
        < 10000 => 4,
        < 100000 => 5,
        < 1000000 => 6,
        < 10000000 => 7,
        < 100000000 => 8,
        < 1000000000 => 9,
        _ => 10
    };
}
