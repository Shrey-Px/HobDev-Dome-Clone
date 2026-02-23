namespace Player.Views;

public class FeedbackEmailView : BaseView
{
    public FeedbackEmailView(FeedbackEmailViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.AppThemeBinding(ContentPage.BackgroundProperty, Colors.White, Colors.Black);
            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }
                    .AppThemeBinding(
                        ImageButton.SourceProperty,
                        "back_button_light_theme",
                        "back_button_dark_theme"
                    )
                    .BindCommand(nameof(viewModel.NavigateBackCommand))
                    .Column(BodyColumn.first)
            );

            base.BaseContentGrid.Children.Add(
                new VerticalStackLayout
                {
                    Margin = new Thickness(29, 10, 29, 0),
                    Children =
                    {
                        new Bold28Label { Text = "Contact Us" },
                        new Bold10Label
                        {
                            Text = "usual response time is 24-48 hours",
                            TextColor = Color.FromArgb("#A2A2A2"),
                        }.Margins(0, 10, 0, 0),
                        new Border
                        {
                            StrokeThickness = .5,
                            StrokeShape = new RoundRectangle
                            {
                                CornerRadius = new CornerRadius(22),
                            },
                            Padding = new Thickness(0),
                            Background = Colors.Transparent,
                            Content = new Editor
                            {
                                AutoSize = EditorAutoSizeOption.TextChanges,
                                HeightRequest = 260,
                                MaxLength = 1000,
                                FontSize = 14,
                                FontFamily = "InterRegular",
                            }
                                .AppThemeBinding(
                                    Editor.TextColorProperty,
                                    Colors.Black,
                                    Colors.White
                                )
                                .Bind(Editor.TextProperty, nameof(viewModel.Message))
                                .Margin(5),
                        }
                            .AppThemeBinding(Border.StrokeProperty, Brush.Black, Brush.White)
                            .Margins(0, 40, 0, 0),
                        new MediumButton { Text = "SUBMIT" }
                            .BindCommand(nameof(viewModel.SendEmailCommand))
                            .Margins(0, 38, 0, 0),
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
