using Microsoft.Maui.Platform;
using TravelBuddy.Controls;

namespace TravelBuddy.Handlers;

public partial class CustomEntryHandler
{
    protected override void ConnectHandler(MauiTextField platformView)
    {
        if (VirtualView is BorderlessEntry)
        {
            RemoveBorder(platformView);
        }
        base.ConnectHandler(platformView);
    }

    private void RemoveBorder(MauiTextField platformView)
    {
        platformView.BorderStyle = UIKit.UITextBorderStyle.None;
    }
}