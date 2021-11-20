using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;

namespace VariabledComparer;

public sealed partial class MainWindow : Window
{
    private readonly HashSet<Tab> _tabs = new();
    private readonly DispatcherTimer _recompareDebouncer = new() { Interval = TimeSpan.FromMilliseconds(200) };

    public MainWindow()
    {
        InitializeComponent();
        Title += " v" + App.InformationalVersion;
        _recompareDebouncer.Tick += Recompare;
    }

    private void Recompare(object? sender, EventArgs e)
    {
        _recompareDebouncer.Stop();

        string text = TestInput.Text;
        foreach (Tab tab in _tabs)
        {
            tab.Compare(text);
        }
    }

    private void AddButtonClicked(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new()
        {
            Multiselect = true,
            ShowReadOnly = true,
        };

        if (ofd.ShowDialog(this) != true)
        {
            return;
        }

        foreach (string path in ofd.FileNames)
        {
            GridSplitter splitter = new();
            Grid.SetColumn(splitter, ContentGrid.ColumnDefinitions.Count - 1);
            ContentGrid.Children.Add(splitter);

            ContentGrid.ColumnDefinitions.Add(new ColumnDefinition());
            Tab tab = new(path);
            Grid.SetColumn(tab, ContentGrid.ColumnDefinitions.Count - 1);
            ContentGrid.Children.Add(tab);

            _tabs.Add(tab);
            tab.Compare(TestInput.Text);
        }

        Grid.SetColumnSpan(BottomBarGrid, ContentGrid.ColumnDefinitions.Count);
    }

    private void ResetRecompareDebouncer(object sender, TextChangedEventArgs e)
    {
        _recompareDebouncer.Stop();
        _recompareDebouncer.Start();
    }
}
