# WinForms.AutoComplete

A custom WinForms control which provides a `ComboBox` with better auto-complete methods.

This implementation is a slimmed down version of [Serge Weinstock's implementation](https://www.codeproject.com/Tips/755707/ComboBox-with-Suggestions-Based-on-Loose-Character), with the main differences being:

- Updated for .NET 6.0
- `StartsWith`, `Contains` and `Regex` matching methods
- No bold highlighting on matched segments
