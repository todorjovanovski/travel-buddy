using TravelBuddy.ViewModels;

namespace TravelBuddy.Pages;

public partial class ProfilePage
{
    private readonly ProfileViewModel _viewModel;
    public ProfilePage(ProfileViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
    }

    private void PhotosGridOnSizeChanged(object? sender, EventArgs e)
    {
        _viewModel.CalculateIndicatorLength(PhotosGrid.Width);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        PhotosGrid.HeightRequest = height * 0.7;
        base.OnSizeAllocated(width, height);
    }
}