using TravelBuddy.Models.DTOs;
using TravelBuddy.Pages.Base;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class TourGuidePage : ContentPageBase
{
    public TourGuidePage(TourGuideViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
        MessageUpdated.MessageUpdatedEvent += MessageUpdatedOnMessageUpdatedEvent;
    }

    private void MessageUpdatedOnMessageUpdatedEvent(object? sender, EventArgs e)
    {
        var message = sender as ChatMessage;
        Messages.ScrollTo(message, position: ScrollToPosition.End);
    }
    
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var viewModel = BindingContext as TourGuideViewModel;
        if (viewModel!.Messages.Count == 0) return;
        Messages.ScrollTo(viewModel.Messages.LastOrDefault(), animate: false, position: ScrollToPosition.End);
    }
}