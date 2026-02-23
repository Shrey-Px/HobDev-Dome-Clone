using CommunityToolkit.Maui.Markup;

namespace Player.Views.OnBoarding;

public class OnBoardPage1 : ContentPage
{
    INavigationService navigationService;

    public OnBoardPage1(INavigationService navigationService)
    {
        this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);
        this.navigationService = navigationService;
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
                    (BodyRow.sixth, Auto),
                    (BodyRow.seventh, Auto),
                    (BodyRow.eighth, Auto),
                    (BodyRow.ninth, Auto),
                    (BodyRow.tenth, Auto)
                ),
                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, 50),
                    (BodyColumn.second, Star)
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
                        .ColumnSpan(2)
                        .Margins(23, 24, 0, 16),
                    new Image { Source = "onboardfirstimage.png" }
                        .Row(BodyRow.second)
                        .ColumnSpan(2),
                    new Bold30Label { Text = "Welcome to \nDome!" }
                        .Row(BodyRow.third)
                        .ColumnSpan(2)
                        .Margins(36, 33, 0, 0),
                    new Medium16Label { Text = "Discover sports, playpals and coaches near you" }
                        .Row(BodyRow.fourth)
                        .ColumnSpan(2)
                        .Margins(36, 7, 39, 0),
                    new Image
                    {
                        HeightRequest = 20,
                        WidthRequest = 20,
                        Source = "findfacilities.png",
                    }
                        .Row(BodyRow.fifth)
                        .Margins(46, 30, 0, 0),
                    new Medium14Label { Text = "Find sport facilities around you" }
                        .Row(BodyRow.fifth)
                        .Column(BodyColumn.second)
                        .Margins(20, 30, 39, 0),
                    new Image
                    {
                        HeightRequest = 20,
                        WidthRequest = 20,
                        Source = "meetplaymates.png",
                    }
                        .Row(BodyRow.sixth)
                        .Margins(46, 16, 0, 0),
                    new Medium14Label { Text = "Meet up with playpals" }
                        .Row(BodyRow.sixth)
                        .Column(BodyColumn.second)
                        .Margins(20, 16, 39, 0),
                    new Image
                    {
                        HeightRequest = 20,
                        WidthRequest = 20,
                        Source = "engagewithcoach.png",
                    }
                        .Row(BodyRow.seventh)
                        .Margins(46, 16, 0, 0),
                    new Medium14Label { Text = "Locate and engage with your coach" }
                        .Row(BodyRow.seventh)
                        .Column(BodyColumn.second)
                        .Margins(20, 16, 39, 0),
                    new Image
                    {
                        HeightRequest = 20,
                        WidthRequest = 20,
                        Source = "onboard_learn.png",
                    }
                        .Row(BodyRow.eighth)
                        .Margins(46, 16, 0, 0),
                    new Medium14Label { Text = "Discover helpful tips and techniques" }
                        .Row(BodyRow.eighth)
                        .Column(BodyColumn.second)
                        .Margins(20, 16, 39, 0),
                    new MediumButton
                    {
                        Text = "Get Started",
                        HeightRequest = 40,
                        WidthRequest = 308,
                        HorizontalOptions = LayoutOptions.Center,
                        CornerRadius = 20,
                    }
                        .Invoke(button => button.Clicked += Button_Clicked)
                        .Margins(23, 30, 23, 15)
                        .Row(BodyRow.ninth)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                    new Regular16Label
                    {
                        Text = "LOGIN",
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
                        .Row(BodyRow.tenth)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                },
            },
        };
    }

    private async void Button_Clicked(object? sender, EventArgs e)
    {
        await navigationService.NavigateToAsync(nameof(OnBoardPage2));
    }
}
