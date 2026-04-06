namespace AutocodeDB.SQLTemplates
{
    public static class FunctionEntity
    {
        public static readonly string Agregation = @"\s((COUNT)|(AVG)|(SUM)|(MIN)|(MAX))\s*\(\s*(.)+?\s*\)";
        public static readonly string SQLString = string.Empty;
        public static readonly string Math = string.Empty;
        public static readonly string Date = string.Empty;
        public static readonly string Any = $@"[A-Za-z]+[(]\s*{TableEntity.TableAndColumnName}\s*[)]";
    }
}
