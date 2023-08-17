using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.Enums;
using PurpleExplorer.Helpers;
using PurpleExplorer.Models;
using PurpleExplorer.Services;
using Splat;

namespace PurpleExplorer.ViewModels;

public class RuleDetailsWindowViewModel : ViewModelBase
{
    private readonly ITopicHelper _topicHelper;
    private readonly ILoggingService _loggingService;

    public Rule? Rule { get; set; }
    public ServiceBusSubscription? Subscription { get; set; }
    public ServiceBusTopic? Topic { get; set; }
    public ServiceBusConnectionString? ConnectionString { get; set; }

    public RuleDetailsWindowViewModel(ITopicHelper? topicHelper = null, ILoggingService? loggingService = null)
    {
        _topicHelper = topicHelper ?? Locator.Current.GetService<ITopicHelper>();
        _loggingService = loggingService ?? Locator.Current.GetService<ILoggingService>();
    }

    public async Task DeleteRule(Window window)
    {
        if (Rule == null)
        {
            return;
        }

        if (ConnectionString == null || Topic == null || Subscription == null)
        {
            return;
        }

        _loggingService.Log($"WARNING: Trying to delete {Rule.Name}..");

        var buttonResult = await MessageBoxHelper.ShowConfirmation(
            $"Delete rule",
            $"Are you sure you want to delete rule {Rule.Name}? \n" +
            $"This action is irreversible.");

        if (buttonResult != ButtonResult.Yes)
        {
            return;
        }

        await _topicHelper.DeleteRule(ConnectionString, Topic.Name, Subscription.Name, Rule.Name);

        _loggingService.Log($"Deleted rule {Rule.Name}");
        window.Close();
    }

}