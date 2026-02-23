namespace Player.Views.Connect;

public class HostingSuccessView : ContentPage
{
    public HostingSuccessView(HostingSuccessViewModel viewModel)
    {
        this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);

        Content = new VerticalStackLayout
        {
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                new Image
                {
                    Source = "hosting_success.png",
                    HeightRequest = 100,
                    WidthRequest = 100,
                },
                new SemiBold24Label { Text = "Event Hosted" }
                    .AppThemeBinding(
                        Picker.TextColorProperty,
                        Color.FromArgb("#626262"),
                        Color.FromArgb("#F0F0F0")
                    )
                    .CenterHorizontal()
                    .Margins(0, 20, 0, 0),
                new Regular14Label { Text = "Users can join the game from the chat window" }
                    .AppThemeBinding(
                        Picker.TextColorProperty,
                        Color.FromArgb("#626262"),
                        Color.FromArgb("#A2A2A2")
                    )
                    .Margins(0, 5, 0, 0),
                new MediumButton
                {
                    Text = "Done",
                    Background = Colors.Transparent,
                    BorderWidth = .5,
                    WidthRequest = 100,
                }
                    .AppThemeColorBinding(
                        MediumButton.TextColorProperty,
                        Colors.Black,
                        Colors.White
                    )
                    .AppThemeColorBinding(
                        MediumButton.BorderColorProperty,
                        Colors.Black,
                        Colors.White
                    )
                    .BindCommand(nameof(viewModel.CompleteCommand))
                    .Margins(0, 20, 0, 0),
            },
        }.CenterHorizontal();

        BindingContext = viewModel;
    }
}
