using Microsoft.Maui.Platform;
using TravelBuddy.Controls;

namespace TravelBuddy.Handlers;

public partial class CustomEditorHandler
{
    protected override void ConnectHandler(MauiTextView platformView)
    {
        if (VirtualView is BorderlessEditor)
        {
            RemoveBorder(platformView);
        }
        base.ConnectHandler(platformView);
    }

    private void RemoveBorder(MauiTextView platformView)
    {
#pragma warning disable CA1416
        platformView.BorderStyle = UIKit.UITextViewBorderStyle.None;
#pragma warning restore CA1416
    }
}