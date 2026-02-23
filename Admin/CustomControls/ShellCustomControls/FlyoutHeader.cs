namespace Admin.CustomControls.ShellCustomControls
{
    public class FlyoutHeader : ContentView
    {
        public FlyoutHeader()
        {
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 0,
                Margin = new Thickness(0, 20, 0, 40),
                Children =
                {
                    new Image
                    {
                        Source = "dx_dome_text.png",
                        HorizontalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFit,
                        HeightRequest = 109,
                        WidthRequest = 132,
                    },
                },
            };
        }
    }
}
