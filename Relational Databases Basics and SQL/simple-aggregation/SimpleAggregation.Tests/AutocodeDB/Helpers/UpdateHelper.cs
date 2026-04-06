using System.Text.RegularExpressions;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public static class UpdateHelper
    {
        private const RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Regex UpdateSetRegExp = new Regex(UpdateEntity.UpdateSet, Options);
        private static readonly Regex UpdateSetWhereRegExp = new Regex(UpdateEntity.UpdateSetWhere, Options);
        private static readonly Regex UpdateSetWhereSubselectRegExp = new Regex(UpdateEntity.UpdateSetWhereSubselect, Options);

        public static bool ContainsCorrectUpdateSetInstruction(string query) => UpdateSetRegExp.IsMatch(query);

        public static bool ContainsCorrectUpdateSetWhereInstruction(string query) => UpdateSetWhereRegExp.IsMatch(query);

        public static bool ContainsCorrectUpdateSetWhereSubselectInstruction(string query) => UpdateSetWhereSubselectRegExp.IsMatch(query);
    }
}
