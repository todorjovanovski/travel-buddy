using Android.Content.Res;
using Microsoft.Maui.Platform;
using TravelBuddy.Controls;
using Color = Android.Graphics.Color;

namespace TravelBuddy.Handlers;

public partial class CustomDatePickerHandler
{
    protected override void ConnectHandler(MauiDatePicker platformView)
    {
        if (VirtualView is BorderlessDatePicker)
        {
            RemoveUnderline(platformView);
            RemovePadding(platformView);
        }
        base.ConnectHandler(platformView);
    }

    private void RemoveUnderline(MauiDatePicker datePicker)
    {
        datePicker.BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
    }

    private void RemovePadding(MauiDatePicker datePicker)
    {
        datePicker.SetPadding(0, 0, 0, 0);
        datePicker.SetIncludeFontPadding(false);
    }
}