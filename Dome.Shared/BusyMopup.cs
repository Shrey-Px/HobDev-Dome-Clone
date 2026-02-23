namespace Dome.Shared
{
    public class BusyMopup : PopupPage
    {
        public BusyMopup()
        {
            this.Background = Color.FromArgb("#80000000");
            this.CloseWhenBackgroundIsClicked = false;

            this.Animation = new Mopups.Animations.ScaleAnimation
            {
                DurationIn = 700,
                EasingIn = Easing.SinIn,
                DurationOut = 700,
                EasingOut = Easing.SinIn,
                ScaleIn = 1.2,
                ScaleOut = 0.8,
                PositionIn = Mopups.Enums.MoveAnimationOptions.Bottom,
                PositionOut = Mopups.Enums.MoveAnimationOptions.Bottom,
            };

            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            {
                Content = new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 400,
                    Spacing = 15,
                    Children =
                    {
                        new ActivityIndicator
                        {
                            HeightRequest = 100,
                            WidthRequest = 100,
                            Color = Colors.White,
                            IsRunning = true,
                        },
                        new Label
                        {
                            FontFamily = "InterMedium",
                            Text = "Please wait...",
                            FontSize = 40,
                            TextColor = Colors.White,
                            HorizontalOptions = LayoutOptions.Center,
                        },
                    },
                }.Margins(0, 100, 0, 0);
            }
            else if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                Content = new VerticalStackLayout
                {
                    WidthRequest = 300,
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 10,
                    Children =
                    {
                        new ActivityIndicator
                        {
                            HeightRequest = 50,
                            WidthRequest = 50,
                            Color = Colors.White,
                            IsRunning = true,
                        },
                        new Label
                        {
                            FontFamily = "InterMedium",
                            Text = "Please wait...",
                            FontSize = 20,
                            TextColor = Colors.White,
                            HorizontalOptions = LayoutOptions.Center,
                        },
                    },
                }.Margins(0, 100, 0, 0);
            }
        }
    }
}
