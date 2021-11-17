using System.Timers;
using System.Windows;
using Microsoft.Win32;

namespace VariabledComparer;

public sealed partial class MainWindow : Window
{
    private readonly HashSet<Tab> _tabs = new();
    private readonly System.Timers.Timer _recompareDebouncer = new(200);
    private readonly object _recompareDebouncerLock = new();

    public MainWindow()
    {
        InitializeComponent();
        Title += " v" + App.InformationalVersion;
        _recompareDebouncer.Elapsed += Recompare;
    }

    private void Recompare(object? sender, ElapsedEventArgs e)
    {
        lock (_recompareDebouncerLock)
        {
            _recompareDebouncer.Stop();
        }

        Dispatcher.Invoke(() =>
        {
            foreach (Tab tab in _tabs)
            {
                tab.Compare(TestInput.Text);
            }
        });
    }

    private void AddButtonClicked(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new();

        if (ofd.ShowDialog(this) != true)
        {
            return;
        }

        GridSplitter splitter = new();
        Grid.SetColumn(splitter, ContentGrid.ColumnDefinitions.Count - 1);
        ContentGrid.Children.Add(splitter);

        ContentGrid.ColumnDefinitions.Add(new ColumnDefinition());
        Tab tab = new(ofd.FileName);
        Grid.SetColumn(tab, ContentGrid.ColumnDefinitions.Count - 1);
        Grid.SetColumnSpan(BottomBarGrid, ContentGrid.ColumnDefinitions.Count);
        ContentGrid.Children.Add(tab);

        _tabs.Add(tab);
        tab.Compare(TestInput.Text);
    }

    private void ResetRecompareDebouncer(object sender, TextChangedEventArgs e)
    {
        lock (_recompareDebouncerLock)
        {
            _recompareDebouncer.Stop();
            _recompareDebouncer.Start();
        }
    }
}
