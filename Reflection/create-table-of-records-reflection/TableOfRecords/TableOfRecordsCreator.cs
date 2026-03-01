using System.Reflection;

namespace TableOfRecords;

public static class TableOfRecordsCreator
{
    public static void WriteTable<T>(ICollection<T>? collection, TextWriter? writer)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(writer);

        if (collection.Count == 0)
        {
            throw new ArgumentException("Collection is empty");
        }

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        int columnCount = properties.Length;
        int[] columnWidths = new int[columnCount];

        for (int i = 0; i < columnCount; i++)
        {
            columnWidths[i] = properties[i].Name.Length;
        }

        foreach (var item in collection)
        {
            for (int i = 0; i < columnCount; i++)
            {
                var value = properties[i].GetValue(item)?.ToString() ?? string.Empty;
                columnWidths[i] = Math.Max(columnWidths[i], value.Length);
            }
        }

        void WriteBorder()
        {
            writer.Write("+");
            foreach (var width in columnWidths)
            {
                writer.Write(new string('-', width + 2));
                writer.Write("+");
            }

            writer.WriteLine();
        }

        WriteBorder();

        writer.Write("|");
        for (int i = 0; i < columnCount; i++)
        {
            var name = properties[i].Name;
            writer.Write(" " + name.PadRight(columnWidths[i]) + " ");
            writer.Write("|");
        }

        writer.WriteLine();

        WriteBorder();

        foreach (var item in collection)
        {
            writer.Write("|");

            for (int i = 0; i < columnCount; i++)
            {
                var prop = properties[i];
                var value = prop.GetValue(item)?.ToString() ?? " ";

                bool isNumber =
                    prop.PropertyType == typeof(int) ||
                    prop.PropertyType == typeof(byte) ||
                    prop.PropertyType == typeof(decimal) ||
                    prop.PropertyType == typeof(double) ||
                    prop.PropertyType == typeof(float);

                string formatted = isNumber
                    ? value.PadLeft(columnWidths[i])
                    : value.PadRight(columnWidths[i]);

                writer.Write(" " + formatted + " ");
                writer.Write("|");
            }

            writer.WriteLine();
            WriteBorder();
        }
    }
}
