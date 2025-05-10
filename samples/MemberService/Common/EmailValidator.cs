using System.Text.RegularExpressions;

namespace MemberService.Common;

public static class EmailValidator
{
    private static readonly Regex _emailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool IsValidFormat(string text) => _emailRegex.IsMatch(text);
}
