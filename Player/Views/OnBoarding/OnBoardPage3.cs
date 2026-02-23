namespace Player.Views.OnBoarding;

public class OnBoardPage3 : ContentPage
{
    INavigationService navigationService;

    public OnBoardPage3(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);
        Content = new ScrollView
        {
            Content = new Grid
            {
                RowDefinitions = Rows.Define(
                    (BodyRow.first, Auto),
                    (BodyRow.second, Auto),
                    (BodyRow.third, Auto),
                    (BodyRow.fourth, Auto),
                    (BodyRow.fifth, Auto),
                    (BodyRow.sixth, Auto)
                ),

                Children =
                {
                    new Image { HorizontalOptions = LayoutOptions.Start }
                        .AppThemeBinding(
                            Image.SourceProperty,
                            "dome_logo.png",
                            "dome_logo_white.png"
                        )
                        .Row(BodyRow.first)
                        .Margins(23, 24, 0, 16),
                    new Image { Source = "onboardthirdimage.png" }
                        .Row(BodyRow.second)
                        .Margins(0, 0, 0, 0),
                    new Bold30Label { Text = "Meet up with \nplaypals" }
                        .Row(BodyRow.third)
                        .Margins(36, 33, 0, 0),
                    new Medium16Label
                    {
                        Text =
                            "Host a game for your friends to join, or join games hosted by others!",
                    }
                        .Row(BodyRow.fourth)
                        .Margins(36, 7, 39, 0),
                    new MediumButton
                    {
                        Text = "Get Started",
                        HeightRequest = 40,
                        WidthRequest = 308,
                        HorizontalOptions = LayoutOptions.Center,
                        CornerRadius = 20,
                    }
                        .Invoke(button => button.Clicked += Button_Clicked)
                        .Margins(0, 80, 0, 15)
                        .Row(BodyRow.fifth),
                    new Regular16Label
                    {
                        Text = "BACK",
                        HorizontalOptions = LayoutOptions.Center,
                        TextDecorations = TextDecorations.Underline,
                    }
                        .Invoke(label =>
                        {
                            label.GestureRecognizers.Add(
                                new TapGestureRecognizer
                                {
                                    Command = new Command(async () =>
                                        await navigationService.PopAsync()
                                    ),
                                }
                            );
                        })
                        .Row(BodyRow.sixth),
                },
            },
        };
    }

    private async void Button_Clicked(object? sender, EventArgs e)
    {
        await navigationService.NavigateToAsync(nameof(RegisterView));
    }
}
