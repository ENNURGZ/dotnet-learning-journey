namespace BankSystem.Tests.AutocodeDB.Helpers;

public static class FileIOHelper
{
    public static readonly string DBDirectory = "DB";

    public static readonly string DataDirectory = "Data";

    public static readonly string DBFileName = "marketplace.db";

    public static readonly string EmptyDBFileName = "empty_tables.db";

    public static readonly string InsertFileName = "insert.sql";

    public static int FilesCount { get; set; }

    public static string ProjectDirectory { get; private set; } = string.Empty;

    public static string GenerateProjectDirectory(string currentDir)
    {
        ProjectDirectory = Directory.GetParent(currentDir)!.Parent!.Parent!.FullName;
        return ProjectDirectory;
    }

    public static string GetDBFullPath(string file)
    {
        return Path.Combine(ProjectDirectory, DBDirectory, file);
    }

    public static string GetEmptyDBFullPath(string file)
    {
        return Path.Combine(ProjectDirectory, DBDirectory, "empty_tables.db");
    }

    public static string GetInsertFileFullPath(string file)
    {
        return Path.Combine(ProjectDirectory, DataDirectory, "insert.sql");
    }

    public static string[] GetFilesNames()
    {
        var result = new string[FilesCount];
        for (int i = 0; i < FilesCount; ++i)
        {
            result[i] = $"task{i + 1}.sql";
        }

        return result;
    }

    public static void CreateEmptyCSVFiles(string[] names)
    {
        for (var i = 0; i < FilesCount; i++)
        {
            var file = Path.Combine(ProjectDirectory, "Data", names[i]);
            File.WriteAllText(file, "   ");
        }
    }

    public static string[] ConvertFileExtToCSV(string[] names)
    {
        var result = new string[FilesCount];
        for (var i = 0; i < FilesCount; i++)
        {
            result[i] = Path.ChangeExtension(names[i], ".csv");
        }

        return result;
    }
}
