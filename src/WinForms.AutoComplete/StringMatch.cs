using System.Collections.Generic;

namespace WinForms.AutoComplete;

// TODO: Use spans instead of substrings

/// <summary>
/// The result of a match, which are stored in the suggestion listbox.
/// </summary>
public class StringMatch
{
    /// <summary>
    /// The original source
    /// </summary>
    public string Text { get; internal set; } = string.Empty;
    /// <summary>
    /// The source decomposed on match/non matches against the pattern
    /// </summary>
    public List<string> Segments { get; internal set; } = new List<string>();
    /// <summary>
    /// Is the first segment a match?
    /// </summary>
    public bool StartsOnMatch { get; internal set; }
}
