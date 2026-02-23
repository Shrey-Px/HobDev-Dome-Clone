namespace Player.Views;

public class NoInternetView : ContentPage
{
    NoInternetViewModel? viewModel;

    public NoInternetView()
    {
        try
        {
            this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);
            NoInternetViewModel? viewModel =
                Shell.Current.Handler?.MauiContext?.Services.GetRequiredService<NoInternetViewModel>();
            Shell.SetTabBarIsVisible(this, false);
            this.viewModel = viewModel;
            Content = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Padding = 20,
                Children =
                {
                    new Image { }.AppThemeBinding(Image.SourceProperty, "cat_white", "cat_black"),
                    new ExtraBold32Label
                    {
                        TextColor = Color.FromArgb("#5F5F5F"),
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Oooops!",
                    }.Margins(0, 40, 0, 20),
                    new Medium14Label
                    {
                        TextColor = Color.FromArgb("#5F5F5F"),
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "No Internet Connection Found",
                    },
                    new Medium12Label
                    {
                        TextColor = Color.FromArgb("#5F5F5F"),
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Please check your connection",
                    },
                    new MediumButton
                    {
                        Text = "Try Again",
                        FontFamily = "InterMedium",
                        FontSize = 15,
                        HeightRequest = 50,
                    }
                        .BindCommand(static (NoInternetViewModel vm) => vm.CheckConnectivityCommand, source: viewModel)
                        .Margins(45, 40, 45, 0),
                },
            };

            BindingContext = viewModel;

            Loaded += NoInternetView_Loaded;

            Unloaded += NoInternetView_Unloaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void NoInternetView_Loaded(object? sender, EventArgs e)
    {
        if (viewModel != null)
            await viewModel.StartTimer();
    }

    private async void NoInternetView_Unloaded(object? sender, EventArgs e)
    {
        if (viewModel != null)
            await viewModel.StopTimer();
    }
}
