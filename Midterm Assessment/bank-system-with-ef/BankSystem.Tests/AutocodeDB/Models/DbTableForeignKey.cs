namespace BankSystem.Tests.AutocodeDB.Models;

public class DbTableForeignKey
{
    public DbTableForeignKey(string localColumn, string refTable, string refColumn)
    {
        this.LocalColumn = localColumn;
        this.RefTable = refTable;
        this.RefColumn = refColumn;
    }

    public string LocalColumn { get; }

    public string RefTable { get; }

    public string RefColumn { get; }

    public override string ToString()
    {
        return $"{this.LocalColumn}=>{this.RefTable}:{this.RefColumn}";
    }
}
