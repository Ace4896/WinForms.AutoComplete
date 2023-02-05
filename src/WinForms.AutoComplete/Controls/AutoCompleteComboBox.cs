using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms.AutoComplete.Controls;

/// <summary>
/// A custom <see cref="ComboBox"/> with more auto-complete methods.
/// </summary>
/// <remarks>This is a refactored version of <see href="https://www.codeproject.com/Tips/755707/ComboBox-with-Suggestions-Based-on-Loose-Character"><c>EasyCompleteComboBox</c></see>.</remarks>
public class AutoCompleteComboBox : ComboBox
{
    #region Private Members

    private StringMatchingMethod _matchingMethod;       // How strings are matched against the input

    private readonly AutoCompleteDropdown _dropDown;    // Custom dropdown control
    private readonly ListBox _suggestionList;           // Suggestion list inside the dropdown control
    private Font _boldFont;                             // Bold font used for displaying matches
    private bool _fromKeyboard;                         // Whether the last change was from the keyboard

    #endregion

    #region New Properties

    [
        DefaultValue(StringMatchingMethod.NoWildcards),
        Description("How strings are matched against the user input"),
        Browsable(true),
        EditorBrowsable(EditorBrowsableState.Always),
        Category("Behavior")
    ]
    public StringMatchingMethod MatchingMethod
    {
        get { return _matchingMethod; }
        set
        {
            if (_matchingMethod != value)
            {
                _matchingMethod = value;
                if (_dropDown.Visible)
                {
                    // recalculate the matches
                    ShowDropdown();
                }
            }
        }
    }

    #endregion

    #region Overridden Inherited Properties

    [Category("Behavior"), DefaultValue(false), Description("Specifies whether items in the list portion of the ComboBox are sorted.")]
    public new bool Sorted
    {
        get { return base.Sorted; }
        set
        {
            _suggestionList.Sorted = value;
            base.Sorted = value;
        }
    }

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool DroppedDown
    {
        get { return base.DroppedDown || _dropDown.Visible; }
        set
        {
            _dropDown.Visible = false;
            base.DroppedDown = value;
        }
    }

    /// <summary>
    /// This property is not relevant for this class.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
    public new AutoCompleteSource AutoCompleteSource
    {
        get { return base.AutoCompleteSource; }
        set { base.AutoCompleteSource = value; }
    }

    /// <summary>
    /// This property is not relevant for this class.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
    public new AutoCompleteStringCollection AutoCompleteCustomSource
    {
        get { return base.AutoCompleteCustomSource; }
        set { base.AutoCompleteCustomSource = value; }
    }

    /// <summary>
    /// This property is not relevant for this class.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
    public new AutoCompleteMode AutoCompleteMode
    {
        get { return base.AutoCompleteMode; }
        set { base.AutoCompleteMode = value; }
    }

    /// <summary>
    /// This property is not relevant for this class.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
    public new ComboBoxStyle DropDownStyle
    {
        get { return base.DropDownStyle; }
        set { base.DropDownStyle = value; }
    }

    #endregion

    public AutoCompleteComboBox()
    {
        _matchingMethod = StringMatchingMethod.NoWildcards;

        // we're overriding these
        DropDownStyle = ComboBoxStyle.DropDown;
        AutoCompleteMode = AutoCompleteMode.None;

        // let's build our suggestion list
        _suggestionList = new ListBox
        {
            DisplayMember = "Text",
            TabStop = false,
            Dock = DockStyle.Fill,
            DrawMode = DrawMode.OwnerDrawFixed,
            IntegralHeight = true,
            Sorted = Sorted,
        };

        _suggestionList.Click += OnSuggestionListClick;
        _suggestionList.DrawItem += OnSuggestionListDrawItem;
        _suggestionList.MouseMove += OnSuggestionListMouseMove;
        _dropDown = new AutoCompleteDropdown(_suggestionList);

        _boldFont = new Font(Font, FontStyle.Bold);
        FontChanged += OnFontChanged;
        OnFontChanged(null, EventArgs.Empty);
    }

    /// <summary>
    /// <see cref="ComboBox.Dispose(bool)"/>
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _boldFont.Dispose();
            _dropDown.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Size and Position of Suggestion ListBox

    /// <summary>
    /// <see cref="ComboBox.OnLocationChanged(EventArgs)"/>
    /// </summary>
    protected override void OnLocationChanged(EventArgs e)
    {
        base.OnLocationChanged(e);
        HideDropdown();
    }

    /// <summary>
    /// <see cref="ComboBox.OnSizeChanged(EventArgs)"/>
    /// </summary>
    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        _dropDown.Width = Width;
    }

    #endregion

    #region Visibility of Suggestion ListBox

    /// <summary>
    /// Shows the drop down.
    /// </summary>
    public void ShowDropdown()
    {
        if (DesignMode)
        {
            return;
        }

        // Hide the "standard" drop down if any
        if (base.DroppedDown)
        {
            BeginUpdate();
            // setting DroppedDown to false may select an item
            // so we save the editbox state
            string oText = Text;
            int selStart = SelectionStart;
            int selLen = SelectionLength;

            // close the "standard" dropdown
            base.DroppedDown = false;

            // and restore the contents of the editbox
            Text = oText;
            Select(selStart, selLen);
            EndUpdate();
        }

        // pop it up and resize it
        int h = Math.Min(MaxDropDownItems, _suggestionList.Items.Count) * _suggestionList.ItemHeight;
        _dropDown.Show(this, new Size(DropDownWidth, h));
    }

    /// <summary>
    /// Hides the drop down.
    /// </summary>
    public void HideDropdown()
    {
        if (_dropDown.Visible)
        {
            _dropDown.Close();
        }
    }

    /// <summary>
    /// <see cref="ComboBox.OnLostFocus(EventArgs)"/>
    /// </summary>
    protected override void OnLostFocus(EventArgs e)
    {
        if (!_dropDown.Focused && !_suggestionList.Focused)
        {
            HideDropdown();
        }
        base.OnLostFocus(e);
    }

    /// <summary>
    /// <see cref="ComboBox.OnDropDown(EventArgs)"/>
    /// </summary>
    protected override void OnDropDown(EventArgs e)
    {
        HideDropdown();
        base.OnDropDown(e);
    }

    #endregion

    #region Keystroke and Mouse Events

    /// <summary>
    /// Called when the user clicks on an item in the suggestion listbox.
    /// </summary>
    private void OnSuggestionListClick(object? sender, EventArgs e)
    {
        _fromKeyboard = false;
        StringMatch sel = (StringMatch)_suggestionList.SelectedItem;
        Text = sel.Text;
        Select(0, Text.Length);
        Focus();
    }

    /// <summary>
    /// Override for processing command keys.
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (_dropDown.Visible && keyData == Keys.Tab)
        {
            // we change the selection but will also allow the navigation to the next control
            if (_suggestionList.Text.Length != 0)
            {
                Text = _suggestionList.Text;
            }
            Select(0, Text.Length);
            HideDropdown();
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>
    /// Override for processing key down events when the dropdown is visible.
    /// </summary>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        _fromKeyboard = true;

        if (!_dropDown.Visible)
        {
            base.OnKeyDown(e);
            return;
        }

        switch (e.KeyCode)
        {
            case Keys.Down:
                if (_suggestionList.SelectedIndex < 0)
                {
                    _suggestionList.SelectedIndex = 0;
                }
                else if (_suggestionList.SelectedIndex < _suggestionList.Items.Count - 1)
                {
                    _suggestionList.SelectedIndex++;
                }

                break;

            case Keys.Up:
                if (_suggestionList.SelectedIndex > 0)
                {
                    _suggestionList.SelectedIndex--;
                }
                else if (_suggestionList.SelectedIndex < 0)
                {
                    _suggestionList.SelectedIndex = _suggestionList.Items.Count - 1;
                }

                break;

            case Keys.Enter:
                if (_suggestionList.Text.Length != 0)
                {
                    Text = _suggestionList.Text;
                }

                Select(0, Text.Length);
                HideDropdown();
                break;

            case Keys.Escape:
                HideDropdown();
                break;

            default:
                base.OnKeyDown(e);
                return;
        }

        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    /// <summary>
    /// Override for determining whether the last text changed event was from the keyboard or the dropdown itself.
    /// </summary>
    protected override void OnDropDownClosed(EventArgs e)
    {
        _fromKeyboard = false;
        base.OnDropDownClosed(e);
    }

    /// <summary>
    /// Override for making suggestions based on the current text input.
    /// </summary>
    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);

        if (!_fromKeyboard || !Focused)
        {
            return;
        }

        _suggestionList.BeginUpdate();
        _suggestionList.Items.Clear();
        StringMatcher matcher = new StringMatcher(MatchingMethod, Text);
        foreach (object item in Items)
        {
            StringMatch sm = matcher.Match(GetItemText(item));
            if (sm != null)
            {
                _suggestionList.Items.Add(sm);
            }
        }
        _suggestionList.EndUpdate();

        bool visible = _suggestionList.Items.Count != 0;

        if (_suggestionList.Items.Count == 1 && ((StringMatch)_suggestionList.Items[0]).Text.Length == Text.Trim().Length)
        {
            StringMatch sel = (StringMatch)_suggestionList.Items[0];
            Text = sel.Text;
            Select(0, Text.Length);
            visible = false;
        }

        if (visible)
        {
            ShowDropdown();
        }
        else
        {
            HideDropdown();
        }

        _fromKeyboard = false;
    }

    /// <summary>
    /// Helper for highlighting the selection under the mouse in the suggestion listbox.
    /// </summary>
    private void OnSuggestionListMouseMove(object? sender, MouseEventArgs e)
    {
        int idx = _suggestionList.IndexFromPoint(e.Location);
        if ((idx >= 0) && (idx != _suggestionList.SelectedIndex))
        {
            _suggestionList.SelectedIndex = idx;
        }
    }

    #endregion

    #region Owner Drawn

    /// <summary>
    /// Keep track of any changes to this control's font.
    /// </summary>
    private void OnFontChanged(object? sender, EventArgs e)
    {
        _boldFont.Dispose();
        _boldFont = new Font(Font, FontStyle.Bold);

        _suggestionList.Font = Font;
        _suggestionList.ItemHeight = _boldFont.Height + 2;
    }

    /// <summary>
    /// Draws a segment of a string and updates the bound rectangle for use with the next segment drawing.
    /// </summary>
    private static void DrawString(Graphics g, Color color, ref Rectangle rect, string text, Font font)
    {
        // TODO: See if this can be converted to use spans

        Size proposedSize = new Size(int.MaxValue, int.MaxValue);
        Size sz = TextRenderer.MeasureText(g, text, font, proposedSize, TextFormatFlags.NoPadding);
        TextRenderer.DrawText(g, text, font, rect, color, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
        rect.X += sz.Width;
        rect.Width -= sz.Width;
    }

    /// <summary>
    /// Draws an item in the suggestion listbox.
    /// </summary>
    private void OnSuggestionListDrawItem(object? sender, DrawItemEventArgs e)
    {
        StringMatch sm = (StringMatch)_suggestionList.Items[e.Index];

        e.DrawBackground();

        bool isBold = sm.StartsOnMatch;
        Rectangle rBounds = e.Bounds;

        foreach (string s in sm.Segments)
        {
            Font f = isBold ? _boldFont : Font;
            DrawString(e.Graphics, e.ForeColor, ref rBounds, s, f);
            isBold = !isBold;
        }

        e.DrawFocusRectangle();
    }

    #endregion
}