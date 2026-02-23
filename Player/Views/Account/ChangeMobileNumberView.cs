namespace Player.Views.Account;

public class ChangeMobileNumberView : AccountBaseView
{
    public ChangeMobileNumberView(ChangeMobileNumberViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            Content = new Grid
            {
                RowDefinitions = Rows.Define(
                    (BodyRow.first, 165),
                    (BodyRow.second, Auto),
                    (BodyRow.third, Auto),
                    (BodyRow.fourth, Auto),
                    (BodyRow.fifth, Auto),
                    (BodyRow.sixth, Auto),
                    (BodyRow.seventh, Auto)
                ),

                Children =
                {
                    new AccountTopControl().Row(BodyRow.first),
                    new ExtraBold28Label { Text = "Confirm \n Mobile Number" }
                        .Row(BodyRow.second)
                        .Margins(20, 10, 0, 0),
                    new Border
                    {
                        StrokeThickness = .5,
                        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                        Padding = new Thickness(0),
                        Stroke = Brush.Black,
                        Content = new HorizontalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Label
                                {
                                    Margin = new Thickness(5, 7, 0, 7),
                                    VerticalOptions = LayoutOptions.Center,
                                    FontSize = 20,
                                }
                                    .Bind(
                                        Label.TextProperty,
                                        $"{nameof(viewModel.SelectedCountry)}.{nameof(viewModel.SelectedCountry.FlagImage)}",
                                        source: viewModel
                                    )
                                    .AppThemeBinding(
                                        Label.TextColorProperty,
                                        Colors.Black,
                                        Colors.White
                                    ),
                                new Medium15Label { VerticalOptions = LayoutOptions.Center }.Bind(
                                    Medium15Label.TextProperty,
                                    $"{nameof(viewModel.SelectedCountry)}.{nameof(viewModel.SelectedCountry.Code)}",
                                    source: viewModel
                                ),
                                new CustomEntry
                                {
                                    Placeholder = "New Mobile Number",
                                    Keyboard = Keyboard.Telephone,
                                }.Bind(Entry.TextProperty, nameof(viewModel.MobileNumber)),
                            },
                        },
                    }
                        .AppThemeBinding(
                            Border.BackgroundProperty,
                            Colors.White,
                            Color.FromArgb("#23262A")
                        )
                        .Row(BodyRow.third)
                        .ColumnSpan(2)
                        .Margins(20, 20, 20, 0),
                    new Regular16Label { TextColor = Colors.Red }
                        .Bind(
                            Regular16Label.TextProperty,
                            nameof(viewModel.MobileNumberError),
                            source: viewModel
                        )
                        .Bind(
                            IsVisibleProperty,
                            nameof(viewModel.MobileNumberError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.fourth)
                        .ColumnSpan(2),
                    new MediumButton { Text = "CONFIRM", WidthRequest = 257 }
                        .Row(BodyRow.fifth)
                        .BindCommand(nameof(viewModel.ChangeMobileNumberCommand), source: viewModel)
                        .Margins(0, 35, 0, 0),
                    new MediumButton
                    {
                        Text = "CANCEL",
                        WidthRequest = 257,
                        Background = Color.FromArgb("#272727"),
                    }
                        .BindCommand(nameof(viewModel.CancelCommand))
                        .Row(BodyRow.sixth)
                        .Margins(0, 10, 0, 0),
                    new CompanyNameControl().Row(BodyRow.seventh).Margins(20, 100, 0, 0),
                },
            };
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
