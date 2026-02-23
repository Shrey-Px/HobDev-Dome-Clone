namespace Admin.View;

public class SyncDataView : ContentPage
{
    public SyncDataView(SyncDataViewModel viewModel)
    {
        Content = new VerticalStackLayout
        {
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
                new Regular28Label { Text = "Loading Data..." },
            },
        };

        BindingContext = viewModel;

        Loaded += async (sender, args) =>
        {
            await viewModel.LoadData();
        };
    }
}
