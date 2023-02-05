using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinForms.AutoComplete.Controls;

/// <summary>
/// A custom <see cref="ToolStripDropDown"/> used by the <see cref="AutoCompleteComboBox"/> control.
/// </summary>
/// <remarks>This is a refactored version of <see href="https://www.codeproject.com/Tips/755707/ComboBox-with-Suggestions-Based-on-Loose-Character"><c>DropdownControl</c></see>.</remarks>
[ToolboxItem(false)]
public class AutoCompleteDropdown : ToolStripDropDown, IMessageFilter
{
    /// <summary>
    /// The <see cref="ToolStripControlHost"/> hosting this <see cref="AutoCompleteDropdown"/>.
    /// </summary>
    private readonly ToolStripControlHost _controlHost;

    /// <summary>
    /// The <see cref="Control"/> that opened this <see cref="AutoCompleteDropdown"/>.
    /// </summary>
    private Control? _opener;

    /// <summary>
    /// The content displayed in this <see cref="AutoCompleteDropdown"/>.
    /// </summary>
    public Control Content { get; }

    public AutoCompleteDropdown(Control content)
    {
        Content = content;
        Content.Location = Point.Empty;
        _controlHost = new ToolStripControlHost(Content);

        // NB: AutoClose must be set to false, because otherwise the ToolStripManager would steal keyboard events
        AutoClose = false;

        // we do ourselves the sizing
        AutoSize = false;
        DoubleBuffered = true;
        ResizeRedraw = false;
        Padding = Margin = _controlHost.Padding = _controlHost.Margin = Padding.Empty;

        // we adjust the size according to the contents
        MinimumSize = Content.MinimumSize;
        content.MinimumSize = Content.Size;

        MaximumSize = Content.MaximumSize;
        content.MaximumSize = Content.Size;

        Size = Content.Size;
        TabStop = Content.TabStop = true;

        // set up the content
        Items.Add(_controlHost);

        // we must listen to mouse events for "emulating" AutoClose
        Application.AddMessageFilter(this);
    }

    /// <summary>
    /// Display the dropdown and adjust its size and location
    /// </summary>
    public void Show(Control opener, Size preferredSize = new Size())
    {
        _opener = opener;

        int w = preferredSize.Width == 0 ? ClientRectangle.Width : preferredSize.Width;
        int h = preferredSize.Height == 0 ? Content.Height : preferredSize.Height;
        h += Padding.Size.Height + Content.Margin.Size.Height;

        Rectangle screen = Screen.FromControl(_opener).WorkingArea;

        // let's try first to place it below the opener control
        Rectangle loc = _opener.RectangleToScreen(
            new Rectangle(
                _opener.ClientRectangle.Left,
                _opener.ClientRectangle.Bottom,
                _opener.ClientRectangle.Left + w,
                _opener.ClientRectangle.Bottom + h
            )
        );

        Point cloc = new Point(_opener.ClientRectangle.Left, _opener.ClientRectangle.Bottom);
        if (!screen.Contains(loc))
        {
            // let's try above the opener control
            loc = _opener.RectangleToScreen(
                new Rectangle(
                    _opener.ClientRectangle.Left,
                    _opener.ClientRectangle.Top - h,
                    _opener.ClientRectangle.Left + w,
                    _opener.ClientRectangle.Top
                )
            );

            if (screen.Contains(loc))
            {
                cloc = new Point(_opener.ClientRectangle.Left, _opener.ClientRectangle.Top - h);
            }
        }

        Width = w;
        Height = h;
        Show(_opener, cloc, ToolStripDropDownDirection.BelowRight);
    }

    #region Overrides

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Content?.Dispose();
            Application.RemoveMessageFilter(this);
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// On resizes, resize the contents
    /// </summary>
    protected override void OnSizeChanged(EventArgs e)
    {
        if (Content != null)
        {
            Content.MinimumSize = Size;
            Content.MaximumSize = Size;
            Content.Size = Size;
            Content.Location = Point.Empty;
        }

        base.OnSizeChanged(e);
    }

    #endregion

    #region IMessageFilter implementation

    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_RBUTTONDOWN = 0x0204;
    private const int WM_MBUTTONDOWN = 0x0207;
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    private const int WM_NCRBUTTONDOWN = 0x00A4;
    private const int WM_NCMBUTTONDOWN = 0x00A7;

    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    internal static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref Point pt, int cPoints);

#pragma warning disable CA1806
    public bool PreFilterMessage(ref Message m)
    {
        if (Visible && _opener != null)
        {
            switch (m.Msg)
            {
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                case WM_MBUTTONDOWN:
                case WM_NCLBUTTONDOWN:
                case WM_NCRBUTTONDOWN:
                case WM_NCMBUTTONDOWN:
                    int i = unchecked((int)(long)m.LParam);
                    short x = (short)(i & 0xFFFF);
                    short y = (short)((i >> 16) & 0xffff);
                    Point pt = new Point(x, y);
                    IntPtr srcWnd =
                        // client area: x, y are relative to the client area of the windows
                        (m.Msg == WM_LBUTTONDOWN) || (m.Msg == WM_RBUTTONDOWN) || (m.Msg == WM_MBUTTONDOWN) ?
                        m.HWnd :
                        // non-client area: x, y are relative to the desktop
                        IntPtr.Zero;

                    MapWindowPoints(srcWnd, Handle, ref pt, 1);
                    if (!ClientRectangle.Contains(pt))
                    {
                        // the user has clicked outside the dropdown
                        pt = new Point(x, y);
                        MapWindowPoints(srcWnd, _opener.Handle, ref pt, 1);
                        if (!_opener.ClientRectangle.Contains(pt))
                        {
                            // the user has clicked outside the opener control
                            Close();
                        }
                    }

                    break;
            }
        }

        return false;
    }
#pragma warning restore CA1806

    #endregion
}
