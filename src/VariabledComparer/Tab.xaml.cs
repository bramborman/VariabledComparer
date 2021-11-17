using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace VariabledComparer;

public sealed partial class Tab : UserControl
{
    private static readonly Regex VariablePattern = new(@"\\#[a-zA-Z0-9_]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);

    private readonly string _text;
    private readonly Regex _pattern;

    public Tab(string filePath)
    {
        InitializeComponent();

        FileNameDisplay.Text = Path.GetFileName(filePath);
        _text = File.ReadAllText(filePath);
        FileContentDisplay.Text = _text;

        string regexPattern = VariablePattern.Replace(Regex.Escape(_text), /*lang=regex*/".*");
        _pattern = new Regex(regexPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline);
        RegexPatternDisplay.Text = regexPattern;
    }

    public void Compare(string? comparedText)
    {
        FileNameDisplay.Background =
            string.IsNullOrEmpty(comparedText)
            ? Brushes.Transparent
            : _pattern.IsMatch(comparedText)
                ? Brushes.Green
                : Brushes.Red;
    }
}
