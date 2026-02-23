namespace Player.CustomControls;

public class CompanyNameControl : VerticalStackLayout
{
    public CompanyNameControl()
    {
        this.Children.Add(new ExtraBold28Label { Text = "Dome Sports" });
        this.Children.Add(
            new Image { HorizontalOptions = LayoutOptions.Start }.AppThemeBinding(
                Image.SourceProperty,
                "arrow_light_theme.png",
                "arrow_dark_theme.png"
            )
        );
        this.Children.Add(
            new Bold12Label
            {
                Text = "PLAY YOUR SPORT EVERYDAY",
                TextColor = Color.FromArgb("#EF2F50"),
            }
        );
    }
}
