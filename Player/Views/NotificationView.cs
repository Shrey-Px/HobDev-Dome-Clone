namespace Player.Views;

public class NotificationView : ContentPage
{
    enum BodyRow
    {
        navigation,
        firstNotification,
        firstNotificationSecondLine,
        line,
        remaining,
    }

    enum BodyColumn
    {
        first,
        second,
    }

    public NotificationView(NotificationViewModel viewModel)
    {
        this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);
        Content = new ScrollView
        {
            Padding = new Thickness(0, 0, 0, 20),
            Content = new Grid
            {
                RowDefinitions = Rows.Define(
                    (BodyRow.navigation, Auto),
                    (BodyRow.firstNotification, Auto),
                    (BodyRow.firstNotificationSecondLine, Auto),
                    (BodyRow.line, Auto),
                    (BodyRow.remaining, Star)
                ),
                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, Auto),
                    (BodyColumn.second, Star)
                ),
                ColumnSpacing = 10,
                Children =
                {
                    new ImageButton { }
                        .AppThemeBinding(
                            ImageButton.SourceProperty,
                            "back_button_light_theme",
                            "back_button_dark_theme"
                        )
                        .BindCommand(nameof(viewModel.NavigateBackCommand))
                        .Column(BodyColumn.first)
                        .Row(BodyRow.navigation)
                        .Margins(30, 25, 0, 0),
                    new Regular18Label
                    {
                        Text = "Notifications",
                        HorizontalOptions = LayoutOptions.Center,
                    }
                        .Row(BodyRow.navigation)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2)
                        .Margins(0, 30, 0, 0),
                    new Image { Source = "notification_initial" }
                        .Row(BodyRow.firstNotification)
                        .Column(BodyColumn.first)
                        .RowSpan(2)
                        .Margins(30, 10, 0, 0),
                    new Bold15Label { Text = "Welcome to Dome!" }
                        .Row(BodyRow.firstNotification)
                        .Column(BodyColumn.second)
                        .Margins(0, 30, 20, 0),
                    new Regular10Label
                    {
                        Text =
                            "We're thrilled to have you on board. Book, connect, and make the most of your sporting experience with us. ",
                    }
                        .AppThemeBinding(
                            Regular10Label.TextColorProperty,
                            Color.FromArgb("626262"),
                            Color.FromArgb("A2A2A2")
                        )
                        .Row(BodyRow.firstNotificationSecondLine)
                        .Column(BodyColumn.second)
                        .Margins(0, 0, 20, 20),
                    new Rectangle { Background = Color.FromArgb("D3D1D1"), HeightRequest = 2 }
                        .Row(BodyRow.line)
                        .ColumnSpan(2),
                },
            },
        };

        BindingContext = viewModel;
    }
}
