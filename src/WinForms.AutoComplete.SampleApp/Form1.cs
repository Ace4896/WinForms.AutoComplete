using System.Windows.Forms;

namespace WinForms.AutoComplete.SampleApp
{
    public partial class Form1 : Form
    {
        private static readonly string[] _items = new[]
        {
            "Diluc",
            "Jean",
            "Mona",
            "Keqing",
            "Qiqi",
            "Tighnari",
        };

        public Form1()
        {
            InitializeComponent();

            Load += Form1_Load;
        }

        private void Form1_Load(object? sender, System.EventArgs e)
        {
            comboBoxStartsWith.Items.AddRange(_items);
            comboBoxStartsWith.MatchingMethod = StringMatchingMethod.StartsWith;

            comboBoxContains.Items.AddRange(_items);
            comboBoxContains.MatchingMethod = StringMatchingMethod.Contains;

            comboBoxRegex.Items.AddRange(_items);
            comboBoxRegex.MatchingMethod = StringMatchingMethod.Regex;

            listBoxAvailableItems.Items.AddRange(_items);
        }
    }
}