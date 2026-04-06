using System.Text.RegularExpressions;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public static class DeleteHelper
    {
        private const RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnoreCase;

        private static readonly Regex DeleteFromRegex = new Regex(DeleteEntity.DeleteFrom, Options);

        private static readonly Regex DeleteFromWhereRegex = new Regex(DeleteEntity.DeleteFromWhere, Options);

        private static readonly Regex DeleteFromSublelectRegex = new Regex(DeleteEntity.DeleteFromSubselect, Options);

        public static bool ContainsDeleteFrom(string query) => DeleteFromRegex.IsMatch(query);

        public static bool ContainsDeleteFromWhere(string query) => DeleteFromWhereRegex.IsMatch(query);

        public static bool ContainsDeleteFromSubselect(string query) => DeleteFromSublelectRegex.IsMatch(query);
    }
}
