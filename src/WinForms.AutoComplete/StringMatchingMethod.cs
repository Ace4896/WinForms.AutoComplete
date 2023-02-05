namespace WinForms.AutoComplete;

/// <summary>
/// Represents the different ways in which a string can be matched using <see cref="StringMatcher"/>.
/// </summary>
public enum StringMatchingMethod
{
    NoWildcards,
    UseWildcards,
    UseRegexs,
}
