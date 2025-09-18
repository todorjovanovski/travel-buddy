using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelBuddy.Models;
using TravelBuddy.Models.DTOs;
using TravelBuddy.Pages;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;

namespace TravelBuddy.ViewModels;

public partial class ChatViewModel : ViewModelBase, IQueryAttributable
{
    private readonly INavigationService _navigationService;
    private readonly IFirebaseDbService _firebaseDbService;
    private CompositeDisposable _disposables = new();
    
    [ObservableProperty]
    private Chat _chat = null!;

    [ObservableProperty] 
    private ObservableCollection<ChatMessage> _messages = [];

    [ObservableProperty] 
    [NotifyPropertyChangedFor(nameof(EntryBoxIcon))]
    private string _message = string.Empty;

    [ObservableProperty] 
    private bool _isChatEmpty = true;
    
    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty] 
    private User _loggedInUser = null!;
    
    public string ChatId { get; set; } = string.Empty;
    public ImageSource EntryBoxIcon => Message == string.Empty ? ImageSource.FromFile("mic_icon") : ImageSource.FromFile("send_icon");

    public ChatViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService)
    {
        _navigationService = navigationService;
        _firebaseDbService = firebaseDbService;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await _navigationService.GoBackAsync();
    }

    [RelayCommand]
    private async Task EntryBoxIconTapped()
    {
        if (Message != string.Empty)
        {
            await _firebaseDbService.SendMessage(Message, Chat);
            Message = string.Empty;
            IsChatEmpty = false;
        }
        else
        {
            // await _navigationService.GoToAsync(nameof(AudioMessagePage), new Dictionary<string, object>
            // {
            //     {nameof(AudioMessageViewModel.ChatId), ChatId}
            // });
        }
    }

    [RelayCommand]
    private async Task GoToTourGuideChat()
    {
        await _navigationService.GoToAsync(nameof(TourGuidePage), new Dictionary<string, object>
        {
            { nameof(TourGuideViewModel.ChatId), ChatId }
        });
    }

    protected override async Task OnAppearingAsync()
    {
        LoggedInUser = (await _firebaseDbService.FetchLoggedInUser())!;
        Chat = await _firebaseDbService.FetchChat(ChatId);
        Title = Chat.Title;
        Messages = Chat.Messages.Select(m => new ChatMessage
        {
            MessageText = m.Content,
            SenderId = m.SenderId,
            CurrentUserId = _firebaseDbService.LoggedInUserId,
            DateSent = m.Time
        }).ToObservableCollection();

        _disposables = new CompositeDisposable();
        _disposables.Add(_firebaseDbService.ObserveChatChanges(ChatId)
            .ObserveOn(new MainThreadSynchronizationContext())
            .Subscribe(firebaseEvent =>
            {
                var chat = firebaseEvent.Object;
                if (chat.LastMessage is null || Messages.Count == chat.Messages.Count) return;
                if (chat.LastMessage.SenderId != _firebaseDbService.LoggedInUserId)
                {
                    Chat.Messages.Add(chat.LastMessage);
                }
                Messages.Add(new ChatMessage
                {
                    MessageText = chat.LastMessage.Content,
                    SenderId = chat.LastMessage.SenderId,
                    CurrentUserId = _firebaseDbService.LoggedInUserId,
                    DateSent = chat.LastMessage.Time
                });
                IsChatEmpty = Messages.Count == 0;
            })
        );
        
        IsChatEmpty = Messages.Count == 0;
        await base.OnAppearingAsync();
    }

    protected override Task OnDisappearingAsync()
    {
        _disposables.Dispose();
        return base.OnDisappearingAsync();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValueAs(nameof(ChatId), out string chatId))
        {
            ChatId = chatId;
        }
    }
}