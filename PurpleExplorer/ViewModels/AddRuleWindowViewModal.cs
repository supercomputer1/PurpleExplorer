using System;
using System.Collections.ObjectModel;
using System.Linq;
using PurpleExplorer.Helpers;
using PurpleExplorer.Models;
using PurpleExplorer.Services;
using ReactiveUI;
using Splat;

namespace PurpleExplorer.ViewModels;

public class AddRuleWindowViewModal : DialogViewModelBase
{
    private string _name = string.Empty;
    private string _filter = string.Empty;

    private readonly ILoggingService _loggingService;
    private readonly ITopicHelper _topicHelper;

    public ServiceBusSubscription? Subscription { get; set; }
    public ServiceBusTopic? Topic { get; set; }
    public ServiceBusConnectionString? ConnectionString { get; set; }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string Filter
    {
        get => _filter;
        set => this.RaiseAndSetIfChanged(ref _filter, value);
    }

    public AddRuleWindowViewModal(ILoggingService? loggingService = null, ITopicHelper? topicHelper = null)
    {
        _loggingService = loggingService ?? Locator.Current.GetService<ILoggingService>();
        _topicHelper = topicHelper ?? Locator.Current.GetService<ITopicHelper>();
    }

    public async void AddRule()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Filter))
        {
            _loggingService.Log("Name and filter can not be empty.");
            return;
        }

        if (ConnectionString == null || Topic == null || Subscription == null)
        {
            return;
        }

        try
        {
            await _topicHelper.AddRule(ConnectionString, Topic.Name, Subscription.Name, Name, Filter);
            _loggingService.Log($"Created rule: {Name}.");
        }
        catch (Exception ex)
        {
            _loggingService.Log(ex.Message);
        }
    }
}