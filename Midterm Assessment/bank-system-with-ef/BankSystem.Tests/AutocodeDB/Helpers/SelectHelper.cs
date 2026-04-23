using BankSystem.Tests.AutocodeDB.Models;
using Microsoft.Data.Sqlite;

namespace BankSystem.Tests.AutocodeDB.Helpers;

public static class SelectHelper
{
    public static SelectResult[] GetResults(IEnumerable<string> queries)
    {
        var results = new List<SelectResult>();
        foreach (var query in queries)
        {
            var result = GetResult(query);
            results.Add(result);
        }

        return results.ToArray();
    }

    public static SelectResult GetResult(string query)
    {
        var command = new SqliteCommand(query, SqliteHelper.Connection);
        var result = new SelectResult();
        try
        {
            var reader = command.ExecuteReader();
            result = Read(reader);
        }
        catch (Exception e)
        {
            result.ErrorMessage = e.Message;
        }

        command.Dispose();
        return result;
    }

    public static SelectResult DumpTable(string name)
    {
        string query = $"SELECT * FROM {name}";
        return GetResult(query);
    }

    public static SelectResult[] DumpTables(IEnumerable<string> names)
    {
        var queries = new List<string>();
        foreach (var name in names)
        {
            queries.Add($"SELECT * FROM {name}");
        }

        return GetResults(queries);
    }

    private static SelectResult Read(SqliteDataReader reader)
    {
        var data = new List<string[]>();
        var result = new SelectResult(reader.FieldCount);
        while (reader.Read())
        {
            var rowData = new string[reader.FieldCount];
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (data.Count == 0)
                {
                    result.Schema[i] = reader.GetName(i);
                    result.Types[i] = reader.GetDataTypeName(i);
                }

                if (!reader.IsDBNull(i))
                {
                    rowData[i] = reader.GetString(i);
                }
            }

            data.Add(rowData);
        }

        result.Data = data.ToArray();
        return result;
    }
}
