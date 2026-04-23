using System;

namespace AutocodeDB.SQLTemplates
{
    public static class UpdateEntity
    {
        public static readonly string UpdateSet =
            $@"^\s*UPDATE\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*SET\s*((\s)|(\[))";

        public static readonly string UpdateSetWhere = $@"{UpdateSet}[\s\S]*?WHERE\s*{TableEntity.TableAndColumnName}\s*{OperationEntity.RelationalOperator}";

        public static readonly string UpdateSetWhereSubselect =
            $@"{UpdateSetWhere}\s*[(]\s*SELECT\s*(({FunctionEntity.Any})|({TableEntity.TableAndColumnName}))\s*FROM\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}";
    }
}
