namespace WinForms.AutoComplete;

/// <summary>
/// Represents the different methods for matching a string in <see cref="Controls.AutoCompleteComboBox"/>.
/// </summary>
public enum StringMatchingMethod
{
    StartsWith,
    Contains,
    Regex,
}
