using System.Text;

namespace BankSystem.Tests.AutocodeDB.Models;

public class SelectResult
{
    public SelectResult()
    {
        this.Schema = Array.Empty<string>();
        this.Types = Array.Empty<string>();
        this.Data = Array.Empty<string[]>();
        this.ErrorMessage = string.Empty;
    }

    public SelectResult(int length)
    {
        this.Schema = new string[length];
        this.Types = new string[length];
        this.Data = Array.Empty<string[]>();
        this.ErrorMessage = string.Empty;
    }

    public string[] Schema { get; set; }

    public string[] Types { get; set; }

    public string[][] Data { get; set; }

    public string ErrorMessage { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder();
        var schema = string.Join(",", this.Schema).Trim();
        var types = string.Join(",", this.Types).Trim();
        var data = new StringBuilder();
        foreach (var row in this.Data)
        {
            var dataRow = string.Join(",", row).Trim();
            _ = data.Append(dataRow + Environment.NewLine);
        }

        _ = sb.Append(schema + Environment.NewLine);
        _ = sb.Append(types + Environment.NewLine);
        _ = sb.Append(data.ToString().Trim() + Environment.NewLine);
        return sb.ToString().Trim();
    }
}
