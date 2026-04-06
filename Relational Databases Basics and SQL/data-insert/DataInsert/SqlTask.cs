namespace SqlDataInsert
{
    public static class SqlTask
    {
        private const string FolderName = "sql_queries";
        
        public static string GetQueriesFullPath(string fileName)
        {
            return $"{FolderName}/{fileName}";
        }
    }
}
