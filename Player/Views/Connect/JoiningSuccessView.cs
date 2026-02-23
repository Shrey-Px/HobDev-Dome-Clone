namespace Player.Views.Connect;

public class JoiningSuccessView : ContentPage
{
    public JoiningSuccessView()
    {
        try
        {
            this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
                Children =
                {
                    new Image
                    {
                        Source = "booking_success",
                        HorizontalOptions = LayoutOptions.Center,
                    },
                    new SemiBold24Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Successful",
                    },
                    new Regular14Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "A confirmation email will be sent soon",
                    },
                    new MediumButton
                    {
                        HorizontalOptions = LayoutOptions.Center,
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
                        .Margins(0, 20, 0, 0)
                        .Invoke(button => button.Clicked += Button_Clicked),
                },
            };
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private async void Button_Clicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(MyBookingsView)}");
    }
}
