using System.Text;

namespace BankSystem.Tests.Services;

public static class ReadOnlyListHelper
{
    public static string ConvertToString(this IReadOnlyList<object> records)
    {
        var builder = new StringBuilder();
        foreach (var line in records)
        {
            _ = builder.AppendLine(line.ToString());
        }

        return builder.ToString();
    }
}
