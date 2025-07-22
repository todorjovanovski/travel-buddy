using Android.Content.Res;
using AndroidX.AppCompat.Widget;
using TravelBuddy.Controls;
using Color = Android.Graphics.Color;

namespace TravelBuddy.Handlers;

public partial class CustomEditorHandler
{
    protected override void ConnectHandler(AppCompatEditText platformView)
    {
        if (VirtualView is BorderlessEditor)
        {
            RemoveUnderline(platformView);
            RemovePadding(platformView);
        }
        base.ConnectHandler(platformView);
    }

    void RemoveUnderline(AppCompatEditText editText)
    {
        editText.BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
    }

    void RemovePadding(AppCompatEditText editText)
    {
        editText.SetPadding(0, 0, 0, 0);
        editText.SetIncludeFontPadding(false);
    }
}