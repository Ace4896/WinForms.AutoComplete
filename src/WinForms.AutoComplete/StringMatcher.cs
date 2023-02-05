using System;
using System.Text.RegularExpressions;

namespace WinForms.AutoComplete;

/// <summary>
/// Matches a string using the provided <see cref="StringMatchingMethod"/>.
/// </summary>
public class StringMatcher
{
    private readonly string _pattern;
    private readonly Regex? _regex;

    public Func<string, bool> IsMatch;

    public StringMatcher(string pattern, StringMatchingMethod matchingMethod)
    {
        _pattern = pattern;

        switch (matchingMethod)
        {
            case StringMatchingMethod.StartsWith:
                IsMatch = StartsWith;
                break;

            case StringMatchingMethod.Contains:
                IsMatch = Contains;
                break;

            case StringMatchingMethod.Regex:
                try
                {
                    _regex = new Regex(pattern, RegexOptions.IgnoreCase);
                }
                catch
                {
                    _regex = null;
                }

                IsMatch = MatchesRegex;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(matchingMethod), "Unknown string matching method");
        }
    }

    /// <summary>
    /// Predicate for whether the source string starts with the specified pattern.
    /// </summary>
    /// <param name="source">The source string to check.</param>
    /// <returns>Whether the source string starts with the specified pattern.</returns>
    private bool StartsWith(string source) => source.StartsWith(_pattern, StringComparison.InvariantCultureIgnoreCase);

    /// <summary>
    /// Predicate for whether the source string contains the specified pattern.
    /// </summary>
    /// <param name="source">The source string to check.</param>
    /// <returns>Whether the source string contains the specified pattern.</returns>
    private bool Contains(string source) => source.Contains(_pattern, StringComparison.InvariantCultureIgnoreCase);

    /// <summary>
    /// Predicate for whether the source string matches the specified regex pattern.
    /// </summary>
    /// <param name="source">The source string to check.</param>
    /// <returns>Whether the source string matches the specified regex pattern.</returns>
    private bool MatchesRegex(string source)
    {
        if (_regex == null)
        {
            // Don't match an invalid regex
            return false;
        }

        return _regex.IsMatch(source);
    }
}
