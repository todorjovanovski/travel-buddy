using Microsoft.Maui.Platform;
using TravelBuddy.Controls;
using UIKit;

namespace TravelBuddy.Handlers;

public partial class CustomPickerHandler
{
    protected override void ConnectHandler(MauiPicker platformView)
    {
        if (VirtualView is BorderlessPicker)
        {
            RemoveBorder(platformView);
        }
        base.ConnectHandler(platformView);
    }

    private void RemoveBorder(MauiPicker platformView)
    {
        platformView.BorderStyle = UITextBorderStyle.None;
    }
}