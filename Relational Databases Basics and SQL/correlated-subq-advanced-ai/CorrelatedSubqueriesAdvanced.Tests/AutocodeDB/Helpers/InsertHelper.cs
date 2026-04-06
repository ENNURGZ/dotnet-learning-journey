using System.Text.RegularExpressions;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public static class InsertHelper
    {
        private const RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        private static readonly Regex InsertRegExp = new Regex(InsertEntity.Insert, Options);

        public static bool ContainsCorrectInsertInstruction(string query) => InsertRegExp.IsMatch(query);
    }
}
