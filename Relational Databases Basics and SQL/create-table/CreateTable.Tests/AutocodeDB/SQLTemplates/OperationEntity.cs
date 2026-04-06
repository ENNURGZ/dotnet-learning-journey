namespace AutocodeDB.SQLTemplates
{
    public static class OperationEntity
    {
        public static readonly string RelationalOperator = @"((IN)|(NOT\s+IN)|(<)|(<=)|(>)|(>=)|(=)|(NOT\s+EXISTS)|(EXISTS)|(BETWEEN)|(LIKE)|(NOT\s+LIKE))";
    }
}
