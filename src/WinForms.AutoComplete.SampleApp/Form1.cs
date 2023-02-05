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
            comboBoxNoWildcards.Items.AddRange(_items);
            comboBoxNoWildcards.MatchingMethod = StringMatchingMethod.NoWildcards;

            comboBoxUseWildcards.Items.AddRange(_items);
            comboBoxUseWildcards.MatchingMethod = StringMatchingMethod.UseWildcards;

            comboBoxRegex.Items.AddRange(_items);
            comboBoxRegex.MatchingMethod = StringMatchingMethod.UseRegexs;

            listBoxAvailableItems.Items.AddRange(_items);
        }
    }
}