namespace BankSystem.Tests.AutocodeDB.Models;

public class DbTable
{
    public DbTable()
    {
        this.TableName = string.Empty;
        this.ColumnList = new Dictionary<string, string>(10);
        this.ForeignKeys = [];
        this.SequenceNumber = 0;
    }

    public string TableName { get; set; }

    public Dictionary<string, string> ColumnList { get; }

    public List<DbTableForeignKey> ForeignKeys { get; }

    public int SequenceNumber { get; set; }

    public override string ToString()
    {
        return $"{this.TableName}({this.SequenceNumber}):" + string.Join(',', this.ColumnList) + ";" + string.Join(',', this.ForeignKeys);
    }
}
