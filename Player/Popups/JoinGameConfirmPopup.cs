using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;

namespace Player.Popups;

public class JoinGameConfirmPopup : Popup<bool>
{
    public JoinGameConfirmPopup()
    {
        Background = Colors.Transparent;

        Content = new Border
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            StrokeThickness = 0,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
            Stroke = new SolidColorBrush(Color.FromArgb("#020202")),
            Padding = new Thickness(26, 16, 38, 21),
            Content = new Grid
            {
                RowDefinitions = Rows.Define(
                    (BodyRow.first, Auto),
                    (BodyRow.second, Auto),
                    (BodyRow.third, Auto)
                ),
                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, Star),
                    (BodyColumn.second, Star)
                ),
                ColumnSpacing = 15,
                Children =
                {
                    new ExtraBold20Label { Text = "Way to Go!" }
                        .Row(BodyRow.first)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                    new ExtraBold12Label { Text = "Are you sure you want to join this game?" }
                        .Row(BodyRow.second)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                    new MediumButton
                    {
                        Text = "Yes",
                        FontSize = 11,
                        CornerRadius = 16,
                    }
                        .Invoke(yesButton => yesButton.Clicked += YesButton_Clicked)
                        .Row(BodyRow.third)
                        .Column(BodyColumn.first)
                        .Margins(0, 25, 0, 0),
                    new MediumButton
                    {
                        Text = "No",
                        FontSize = 11,
                        Background = Colors.Black,
                        BorderColor = Color.FromArgb("#EF2F50"),
                        CornerRadius = 16,
                    }
                        .Invoke(noButton => noButton.Clicked += NoButton_Clicked)
                        .AppThemeBinding(MediumButton.BorderWidthProperty, 0, 1)
                        .Row(BodyRow.third)
                        .Column(BodyColumn.second)
                        .Margins(0, 25, 0, 0),
                },
            },
        }
            .AppThemeBinding(
                Border.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            )
            .AppThemeBinding(Border.StrokeThicknessProperty, 0, 1);
    }

    private async void NoButton_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync(false);
    }

    private async void YesButton_Clicked(object? sender, EventArgs e)
    {
        await CloseAsync(true);
    }
}
