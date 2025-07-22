using Android.Content.Res;
using Microsoft.Maui.Platform;
using TravelBuddy.Controls;
using Color = Android.Graphics.Color;

namespace TravelBuddy.Handlers;

public partial class CustomPickerHandler
{
    protected override void ConnectHandler(MauiPicker platformView)
    {
        if (VirtualView is BorderlessPicker)
        {
            RemoveUnderline(platformView);
            RemovePadding(platformView);
        }
        base.ConnectHandler(platformView);
        base.ConnectHandler(platformView);
    }

    private void RemovePadding(MauiPicker platformView)
    {
        platformView.SetPadding(0, 0, 0, 0);
        platformView.SetIncludeFontPadding(false);
    }

    private void RemoveUnderline(MauiPicker platformView)
    {
        platformView.BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
    }
}