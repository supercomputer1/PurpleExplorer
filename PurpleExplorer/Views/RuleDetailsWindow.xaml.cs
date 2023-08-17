using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace PurpleExplorer.Views;

public class RuleDetailsWindow : Window
{
    public RuleDetailsWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}