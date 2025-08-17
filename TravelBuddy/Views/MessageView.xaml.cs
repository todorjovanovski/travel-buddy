using Microsoft.Maui.Controls.Shapes;
using TravelBuddy.Models.DTOs;

namespace TravelBuddy.Views;

public partial class MessageView : ContentView
{
    public MessageView()
    {
        InitializeComponent();
    }
    
    public static readonly BindableProperty MessageProperty =
        BindableProperty.Create(nameof(Message), typeof(ChatMessage), typeof(MessageView), propertyChanged: OnMessageChanged);

    public ChatMessage Message
    {
        get => (ChatMessage)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly BindableProperty IsDateSentVisibleProperty =
        BindableProperty.Create(nameof(IsDateSentVisible), typeof(bool), typeof(MessageView));

    public bool IsDateSentVisible
    {
        get => (bool)GetValue(IsDateSentVisibleProperty);
        set => SetValue(IsDateSentVisibleProperty, value);
    }

    private static void OnMessageChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var messageView = (MessageView)bindable;
        if (newValue is not ChatMessage message) return;
        if (message.SenderId == message.CurrentUserId)
        {
            messageView.MessageBorder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10, 10, 10, 0) };
            messageView.MessageTextLabel.HorizontalTextAlignment = TextAlignment.End;
            messageView.MessageStackLayout.HorizontalOptions = LayoutOptions.End;
            messageView.MessageStackLayout.Padding = new Thickness(30, 0, 0, 0);
            messageView.DateSentLabel.HorizontalTextAlignment = TextAlignment.End;
        }
        else
        {
            messageView.MessageBorder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10, 10, 0, 10) };
            messageView.MessageTextLabel.HorizontalTextAlignment = TextAlignment.Start;
            messageView.MessageStackLayout.HorizontalOptions = LayoutOptions.Start;
            messageView.MessageStackLayout.Padding = new Thickness(0, 0, 30, 0);
            messageView.DateSentLabel.HorizontalTextAlignment = TextAlignment.Start;
        }
    }

    private void OnMessageTapped(object? sender, TappedEventArgs e)
    {
        DateSentLabel.Text = Message.DateSent.Hour + ":" + Message.DateSent.Minute;
        DateSentLabel.FontSize = 12;
        DateSentLabel.TextColor = Colors.DimGray;
        DateSentLabel.Padding = new Thickness(3);
        IsDateSentVisible = !IsDateSentVisible;
    }
}