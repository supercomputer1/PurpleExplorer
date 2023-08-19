using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PurpleExplorer.Helpers;
using PurpleExplorer.Models;
using PurpleExplorer.ViewModels;

namespace PurpleExplorer.Views;

public class AddRuleWindow : Window
{
    public AddRuleWindow()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public async void btnClose(object sender, RoutedEventArgs e)
    {
        var dataContext = DataContext as AddRuleWindowViewModal;

        if (dataContext == null)
        {
            return;
        }

        dataContext.Cancel = false;
        Close();
    }
}