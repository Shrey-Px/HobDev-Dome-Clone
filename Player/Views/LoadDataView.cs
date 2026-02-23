namespace Player.Views;

public class LoadDataView : ContentPage
{
    int count = 0;

    public LoadDataView(LoadDataViewModel viewModel)
    {
        this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);

        Content = new VerticalStackLayout
        {
            WidthRequest = 300,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 30,
            Children =
            {
                new ActivityIndicator
                {
                    WidthRequest = 100,
                    HeightRequest = 100,
                    Color = Color.FromArgb("#EF2F50"),
                    IsRunning = true,
                    IsVisible = true,
                },
                new Regular18Label
                {
                    Text = "Loading Data...",
                    HorizontalOptions = LayoutOptions.Center,
                },
            },
        };

        BindingContext = viewModel;

        Loaded += async (sender, args) =>
        {
            // the loaded event is fired twice on iOS. Thus the count variable is used to limit the execution of the method to just once
            count++;
            if (count == 1)
            {
                await viewModel.LoadData();
            }
        };
    }
}
