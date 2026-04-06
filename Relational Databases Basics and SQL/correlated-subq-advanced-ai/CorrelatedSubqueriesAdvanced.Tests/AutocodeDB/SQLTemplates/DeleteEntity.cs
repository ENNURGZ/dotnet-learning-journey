using System;

namespace AutocodeDB.SQLTemplates
{
    public static class DeleteEntity
    {
        public static readonly string DeleteFrom = $@"^\s*DELETE\s+FROM\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}\s*";

        public static readonly string DeleteFromWhere =
            $@"{DeleteFrom}WHERE\s*{TableEntity.TableAndColumnName}\s*{OperationEntity.RelationalOperator}";

        public static readonly string DeleteFromSubselect =
            $@"{DeleteFromWhere}\s*[(]\s*SELECT\s*(({FunctionEntity.Any})|({TableEntity.TableAndColumnName}))\s*FROM\s*{TableEntity.TblNameIn}{TableEntity.TableName}{TableEntity.TblNameOut}";
    }
}
