using System;
using System.Collections.Generic;
using System.IO;
using AutocodeDB.Models;

namespace AutocodeDB.Helpers
{
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
            ProjectDirectory = Directory.GetParent(currentDir) !.Parent!.Parent!.FullName;
            return ProjectDirectory;
        }

        public static string GetDBFullPath(string file)
        {
            return Path.Combine(ProjectDirectory, DBDirectory, "marketplace.db");
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

        public static void SerializeResult(SelectResult selectResult, string file)
        {
            File.WriteAllText(file, selectResult.ToString() ?? string.Empty);
        }

        public static SelectResult DeserializeResult(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException($"The file '{file}' was not found in the 'Data' folder.");
            }

            var lines = File.ReadAllLines(file);
            var schema = lines[0].Split(",");
            var types = lines[1].Split(",");
            var data = new List<string[]>();
            for (var i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                string[] lineData = line.Split(",");

                for (int j = 0; j < lineData.Length; j++)
                {
                    if (string.IsNullOrEmpty(lineData[j]))
                    {
                        lineData[j] = null;
                    }
                }

                data.Add(lineData);
            }

            return new SelectResult
            {
                Schema = schema,
                Types = types,
                Data = data.ToArray(),
            };
        }

        public static void SerializeResultFiles(SelectResult[] actualResults, string[] fileNames)
        {
            for (var i = 0; i < FilesCount; i++)
            {
                var file = Path.ChangeExtension(fileNames[i], ".csv");
                file = Path.Combine(ProjectDirectory, "Data", file);
                FileIOHelper.SerializeResult(actualResults[i], file);
            }
        }

        public static SelectResult[] DeserializeResultFiles(string[] fileNames)
        {
            SelectResult[] expectedResults = new SelectResult[FilesCount];
            for (var i = 0; i < FilesCount; i++)
            {
                var file = Path.ChangeExtension(fileNames[i], ".csv");
                file = Path.Combine(ProjectDirectory, "Data", file);
                expectedResults[i] = FileIOHelper.DeserializeResult(file);
            }

            return expectedResults;
        }
    }
}
