using Microsoft.Maui.Platform;
using TravelBuddy.Controls;
using UIKit;

namespace TravelBuddy.Handlers;

public partial class CustomDatePickerHandler
{
    protected override void ConnectHandler(MauiDatePicker platformView)
    {
        if (VirtualView is BorderlessDatePicker)
        {
            RemoveBorder(platformView);
        }

        base.ConnectHandler(platformView);
    }
    
    private void RemoveBorder(MauiDatePicker platformView)
    {
        platformView.BorderStyle = UITextBorderStyle.None;
    }
}