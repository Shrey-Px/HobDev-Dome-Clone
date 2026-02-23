namespace Player.CustomControls;

public class AccountTopControl : Grid
{
    public AccountTopControl()
    {
        ColumnDefinitions = Columns.Define((BodyColumn.first, Star), (BodyColumn.second, Star));

        this.Children.Add(
            new Image
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 84,
                WidthRequest = 82,
            }
                .AppThemeBinding(Image.SourceProperty, "dome_logo.png", "dome_logo_white.png")
                .Margins(20, 15, 0, 0)
        );
        this.Children.Add(
            new Image
            {
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Source = "ellipse.png",
                HeightRequest = 164,
            }.Column(BodyColumn.second)
        );
    }
}
