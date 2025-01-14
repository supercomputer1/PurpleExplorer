using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PurpleExplorer.Helpers;
using PurpleExplorer.Models;
using PurpleExplorer.ViewModels;

namespace PurpleExplorer.Views;

public class MainWindow : Window
{
    public MainWindow()
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

    private async void MessagesGrid_DoubleTapped(object sender, RoutedEventArgs e)
    {
        var grid = sender as DataGrid;
        var mainWindowViewModel = DataContext as MainWindowViewModel;

        if (grid?.SelectedItem == null)
        {
            return;
        }

        var viewModal = new MessageDetailsWindowViewModel
        {
            Message = grid.SelectedItem as Message,
            ConnectionString = mainWindowViewModel.ConnectionString,
            Subscription = mainWindowViewModel.CurrentSubscription,
            Queue = mainWindowViewModel.CurrentQueue
        };

        await ModalWindowHelper.ShowModalWindow<MessageDetailsWindow, MessageDetailsWindowViewModel>(viewModal);
    }

    private void MessagesGrid_Tapped(object sender, RoutedEventArgs e)
    {
        var grid = sender as DataGrid;
        var mainWindowViewModel = DataContext as MainWindowViewModel;

        if (grid.SelectedItem is Message message)
        {
            mainWindowViewModel.SetSelectedMessage(message);
        }
    }

    private void RulesGrid_Tapped(object sender, RoutedEventArgs e)
    {
        var grid = sender as DataGrid;
        var mainWindowViewModel = DataContext as MainWindowViewModel;

        if (grid == null)
        {
            return;
        }

        if (mainWindowViewModel == null)
        {
            return;
        }

        if (grid.SelectedItem is Rule rule)
        {
            mainWindowViewModel.SetSelectedRule(rule);
        }
    }

    private async void RulesGrid_DoubleTapped(object sender, RoutedEventArgs e)
    {
        var grid = sender as DataGrid;
        var mainWindowViewModel = DataContext as MainWindowViewModel;

        if (grid?.SelectedItem == null)
        {
            return;
        }

        if (mainWindowViewModel == null)
        {
            return;
        }

        var viewModal = new RuleDetailsWindowViewModel
        {
            Rule = grid.SelectedItem as Rule,
            ConnectionString = mainWindowViewModel.ConnectionString,
            Subscription = mainWindowViewModel.CurrentSubscription,
            Topic = mainWindowViewModel.CurrentTopic
        };

        await ModalWindowHelper.ShowModalWindow<RuleDetailsWindow, RuleDetailsWindowViewModel>(viewModal);

        await mainWindowViewModel.FetchRules();
        mainWindowViewModel.RefreshTabHeaders();
    }

    private async void TreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var mainWindowViewModel = DataContext as MainWindowViewModel;
        var treeView = sender as TreeView;

        ClearOtherSelections(treeView);
        mainWindowViewModel.ClearAllSelections();

        var selectedItem = treeView.SelectedItems.Count > 0 ? treeView.SelectedItems[0] : null;
        if (selectedItem is ServiceBusSubscription selectedSubscription)
        {
            mainWindowViewModel.SetSelectedSubscription(selectedSubscription);
            await mainWindowViewModel.FetchMessages();
            await mainWindowViewModel.FetchRules();
            mainWindowViewModel.RefreshTabHeaders();
        }

        if (selectedItem is ServiceBusTopic selectedTopic)
        {
            mainWindowViewModel.SetSelectedTopic(selectedTopic);
        }

        if (selectedItem is ServiceBusQueue selectedQueue)
        {
            mainWindowViewModel.SetSelectedQueue(selectedQueue);
            await mainWindowViewModel.FetchMessages();
            mainWindowViewModel.RefreshTabHeaders();
        }
    }

    private void ClearOtherSelections(TreeView currentTreeView)
    {
        var tvQueues = this.FindControl<TreeView>("tvQueues");
        var tvTopics = this.FindControl<TreeView>("tvTopics");
        if (currentTreeView == tvQueues)
        {
            tvTopics.UnselectAll();
        }

        if (currentTreeView == tvTopics)
        {
            tvQueues.UnselectAll();
        }
    }
}