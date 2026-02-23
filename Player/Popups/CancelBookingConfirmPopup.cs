using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;

namespace Player.Popups;

public class CancelBookingConfirmPopup : Popup<bool>
{
    public CancelBookingConfirmPopup()
    {
        Background = Colors.Transparent;

        // Content = new Border
        // {
        //     StrokeThickness = 0,
        //     StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(16) },
        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                (BodyRow.first, Auto),
                (BodyRow.second, Auto),
                (BodyRow.third, Auto)
            ),
            ColumnDefinitions = Columns.Define((BodyColumn.first, Star), (BodyColumn.second, Star)),
            Padding = new Thickness(50),
            ColumnSpacing = 15,
            Children =
            {
                new ExtraBold20Label { Text = "Hey, Wait!!!" }
                    .Row(BodyRow.first)
                    .Column(BodyColumn.first)
                    .ColumnSpan(2),
                new ExtraBold12Label { Text = "Are you sure you want to cancel this \n booking?" }
                    .Row(BodyRow.second)
                    .Column(BodyColumn.first)
                    .ColumnSpan(2),
                new MediumButton
                {
                    Text = "No",
                    FontSize = 11,
                    CornerRadius = 16,
                }
                    .AppThemeBinding(
                        MediumButton.TextColorProperty,
                        Colors.White,
                        Color.FromArgb("#EF2F50")
                    )
                    .AppThemeBinding(
                        MediumButton.BackgroundProperty,
                        Color.FromArgb("#EF2F50"),
                        Color.FromArgb("#FFFFFF")
                    )
                    .Invoke(noButton => noButton.Clicked += NoButton_Clicked)
                    .AppThemeBinding(MediumButton.BorderWidthProperty, 0, 1)
                    .Row(BodyRow.fourth)
                    .Column(BodyColumn.first)
                    .Margins(0, 25, 0, 0),
                new MediumButton
                {
                    Text = "Yes",
                    TextColor = Colors.White,
                    FontSize = 11,
                    CornerRadius = 16,
                    Background = Colors.Black,
                    BorderColor = Color.FromArgb("#EF2F50"),
                }
                    .AppThemeBinding(MediumButton.BorderWidthProperty, 0, 1)
                    .Invoke(yesButton => yesButton.Clicked += YesButton_Clicked)
                    .Row(BodyRow.third)
                    .Column(BodyColumn.second)
                    .Margins(0, 25, 0, 0),
            },
        }.AppThemeBinding(BackgroundProperty, Color.FromArgb("#F1F3F2"), Colors.Black);
        // };
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
