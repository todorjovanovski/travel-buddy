using TravelBuddy.Pages.Base;
using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class ChatPage : ContentPageBase
{
    public ChatPage(ChatViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        var viewModel = BindingContext as ChatViewModel;
        if (viewModel!.Messages.Count == 0) return;
        Messages.ScrollTo(viewModel.Messages.LastOrDefault(), animate: false);
    }
}