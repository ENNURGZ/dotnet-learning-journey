using System;
using System.IO;
using System.Linq;

namespace CorrelatedSubqueries
{
    public enum FileType
    {
        Sql = 0,
        Gpt = 1,
    }

    public static class SqlTask
    {
        private static readonly string[] foldersName = { "sql_queries", "gpt" };

        public static string GetFilesFullPath(FileType type, string fileName)
        {
            return $"{foldersName[(int)type]}/{fileName}";
        }
        public static string GetFilePath(FileType type, string fileName)
        {
            return Path.Combine(foldersName[(int)type], fileName);
        }

        public static string[] GetFilePaths(FileType type, string[] fileNames)
        {
            return fileNames.Select(x => GetFilePath(type, x)).ToArray();
        }
    }
}
