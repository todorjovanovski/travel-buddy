using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenAI.Chat;
using TravelBuddy.Constants;
using TravelBuddy.Models;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TravelBuddy.ViewModels.Base;
using ChatMessage = TravelBuddy.Models.DTOs.ChatMessage;

namespace TravelBuddy.ViewModels;

public partial class TourGuideViewModel : ViewModelBase, IQueryAttributable
{
    private readonly INavigationService _navigationService;
    private readonly IFirebaseDbService _firebaseDbService;
    private readonly IAlertService _alertService;
    
    [ObservableProperty]
    private TourGuideChat _chat = null!;

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

    public TourGuideViewModel(INavigationService navigationService, IFirebaseDbService firebaseDbService, IAlertService alertService)
    {
        _navigationService = navigationService;
        _firebaseDbService = firebaseDbService;
        _alertService = alertService;
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
            var question = Message;
            Message = string.Empty;
            // await _tourGuideService.AskQuestion(message, Chat);
            
            var sb = new StringBuilder();
            var systemMessage = new SystemChatMessage(
                "You are a helpful tour guide assistant. " +
                "Always answer strictly in plain text only. " +
                "Do not use Markdown, headings, bullet points, or any formatting characters like `#`, `**`, or `_`. " +
                "Respond with plain sentences and line breaks only."
            );
            
            var userMessage = new ChatMessage
            {
                MessageText = question,
                CurrentUserId = LoggedInUser.Id,
                SenderId = _firebaseDbService.LoggedInUserId,
                DateSent = DateTime.UtcNow
            };
            Messages.Add(userMessage);

            var chatMessages = Messages
                .Select(message => (OpenAI.Chat.ChatMessage)(message.SenderId == TourGuideConstants.BotId
                    ? new AssistantChatMessage(message.MessageText)
                    : new UserChatMessage(message.MessageText))).ToList();
            chatMessages.Insert(0, systemMessage);

            var client = new ChatClient(TourGuideConstants.ModelName, TourGuideConstants.OpenAiApiKey);
            var botMessage = new ChatMessage
            {
                MessageText = "...",
                SenderId = TourGuideConstants.BotId,
                CurrentUserId = _firebaseDbService.LoggedInUserId,
                DateSent = DateTime.UtcNow
            };
            Messages.Add(botMessage);
            var stream = client.CompleteChatStreamingAsync(chatMessages);
            await foreach (var update in stream)
            {
                foreach (var content in update.ContentUpdate)
                {
                    if (botMessage.MessageText == "...") botMessage.MessageText = string.Empty;
                    botMessage.MessageText += content.Text;
                    MessageUpdated.Raise(botMessage, EventArgs.Empty);
                }
            }
            
            Chat.Messages.Add(userMessage.ToTourGuideMessage(ChatId));
            Chat.Messages.Add(botMessage.ToTourGuideMessage(ChatId));
            await _firebaseDbService.InsertTourGuideInteraction(Chat);
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
    private async Task ForwardToGroupChat(ChatMessage message)
    {
        var confirmed = await _alertService.AlertAsync(
            "Forward to Group Chat",
            "This action will forward the assistant's message to your group chat. Proceed?",
            "Yes", "No");
        if (!confirmed) return;
        var groupChat = await _firebaseDbService.FetchChat(ChatId);
        await _firebaseDbService.SendMessage(message.MessageText, groupChat, true);
        await _navigationService.GoBackAsync();
    }

    protected override async Task OnAppearingAsync()
    {
        await base.OnAppearingAsync();
        LoggedInUser = (await _firebaseDbService.FetchLoggedInUser())!;
        Chat = await _firebaseDbService.FetchTourGuideChat(ChatId);
        Title = Chat.Title;
        Messages = Chat.Messages.Select(m => new ChatMessage
        {
            MessageText = m.ChatMessage,
            SenderId = m.SenderId,
            CurrentUserId = _firebaseDbService.LoggedInUserId,
            DateSent = m.Time
        }).ToObservableCollection();
        IsChatEmpty = Messages.Count == 0;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValueAs(nameof(ChatId), out string chatId))
        {
            ChatId = chatId;
        }
    }
}