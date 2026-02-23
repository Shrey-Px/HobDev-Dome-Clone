namespace Player.Views.Account;

public class NewProfileView : AccountBaseView
{
    NewProfileViewModel viewModel;

    public NewProfileView(NewProfileViewModel viewModel)
        : base(viewModel)
    {
        this.viewModel = viewModel;

        CustomPicker cityPicker = new CustomPicker { }
            .Bind(CustomPicker.PickerItemSourceProperty, static (NewProfileViewModel vm) => vm.Cities, source: viewModel)
            .Bind(
                CustomPicker.CustomPickerSelectedItemProperty,
                static (NewProfileViewModel vm) => vm.SelectedCity,
                source: viewModel,
                mode: BindingMode.TwoWay
            )
            .Margins(20, 10, 10, 0);
        cityPicker.picker.ItemDisplayBinding = new Binding(nameof(City.CityName));

        CustomPicker provincePicker = new CustomPicker { }
            .Bind(CustomPicker.PickerItemSourceProperty, static (NewProfileViewModel vm) => vm.AllProvinces, source: viewModel)
            .Bind(
                CustomPicker.CustomPickerSelectedItemProperty,
                static (NewProfileViewModel vm) => vm.SelectedProvince,
                source: viewModel,
                mode: BindingMode.TwoWay
            )
            .Margins(10, 10, 20, 0);
        provincePicker.picker.ItemDisplayBinding = new Binding(nameof(Province.ProvinceName));

        Content = new ScrollView
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
                    (BodyRow.seventh, Auto),
                    (BodyRow.eighth, Auto),
                    (BodyRow.ninth, Auto),
                    (BodyRow.tenth, Auto),
                    (BodyRow.eleventh, Auto),
                    (BodyRow.twelfth, Auto)
                ),

                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, Star),
                    (BodyColumn.second, Star)
                ),

                Children =
                {
                    new AccountTopControl().Row(BodyRow.first).ColumnSpan(2),
                    new ExtraBold28Label { Text = "Create Profile" }
                        .Margins(20, 20, 0, 0)
                        .Row(BodyRow.second)
                        .ColumnSpan(2),
                    new AvatarView
                    {
                        BorderWidth = 1,
                        BorderColor = Colors.Black,
                        Background = Colors.White,
                        WidthRequest = 130,
                        HeightRequest = 130,
                        CornerRadius = 65,
                        Padding = 0,
                        HorizontalOptions = LayoutOptions.Center,
                    }
                        .Bind(
                            AvatarView.ImageSourceProperty,
                            nameof(viewModel.PlayerImage),
                            converter: new ByteArrayToImageSourceConverter(),
                            targetNullValue: "profile_image_fallback"
                        )
                        .BindTapGesture(nameof(viewModel.AddImageCommand))
                        .Row(BodyRow.third)
                        .ColumnSpan(2),
                    new BorderedEntry { PlaceholderText = "First Name" }
                        .Bind(BorderedEntry.EntryTextProperty, nameof(viewModel.FirstName), BindingMode.TwoWay, source: viewModel)
                        .Margins(20, 20, 20, 0)
                        .Row(BodyRow.fourth)
                        .ColumnSpan(2),
                    new Regular16Label { TextColor = Colors.Red }
                        .Bind(
                            Regular16Label.TextProperty,
                            nameof(viewModel.FirstNameError),
                            source: viewModel
                        )
                        .Bind(
                            IsVisibleProperty,
                            nameof(viewModel.FirstNameError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.fifth)
                        .ColumnSpan(2)
                        .Margins(20, 0, 20, 0),
                    new BorderedEntry { PlaceholderText = "Last Name" }
                        .Bind(BorderedEntry.EntryTextProperty, nameof(viewModel.LastName), BindingMode.TwoWay, source: viewModel)
                        .Margins(20, 10, 20, 0)
                        .Row(BodyRow.sixth)
                        .ColumnSpan(2),
                    new Regular16Label { TextColor = Colors.Red }
                        .Bind(
                            Regular16Label.TextProperty,
                            nameof(viewModel.LastNameError),
                            source: viewModel
                        )
                        .Bind(
                            IsVisibleProperty,
                            nameof(viewModel.LastNameError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.seventh)
                        .ColumnSpan(2)
                        .Margins(20, 0, 20, 0),
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
                                    Placeholder = "Mobile Number",
                                    Keyboard = Keyboard.Telephone,
                                    VerticalOptions = LayoutOptions.Center,
                                }.Bind(Entry.TextProperty, static (NewProfileViewModel vm) => vm.MobileNumber, source: viewModel),
                            },
                        }.Margins(10, 0, 0, 0),
                    }
                        .AppThemeBinding(
                            Border.BackgroundProperty,
                            Colors.White,
                            Color.FromArgb("#23262A")
                        )
                        .Margins(20, 10, 20, 0)
                        .Row(BodyRow.eighth)
                        .ColumnSpan(2),
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
                        .Row(BodyRow.ninth)
                        .ColumnSpan(2)
                        .Margins(20, 0, 20, 0),
                    cityPicker.Row(BodyRow.tenth).Column(BodyColumn.first),
                    new Regular16Label { TextColor = Colors.Red }
                        .Bind(
                            Regular16Label.TextProperty,
                            nameof(viewModel.CityError),
                            source: viewModel
                        )
                        .Bind(
                            IsVisibleProperty,
                            nameof(viewModel.CityError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.eleventh)
                        .Column(BodyColumn.first)
                        .Margins(10, 0, 20, 0),
                    provincePicker.Row(BodyRow.tenth).Column(BodyColumn.second),
                    new Regular16Label { TextColor = Colors.Red }
                        .Bind(
                            Regular16Label.TextProperty,
                            nameof(viewModel.ProvinceError),
                            source: viewModel
                        )
                        .Bind(
                            IsVisibleProperty,
                            nameof(viewModel.ProvinceError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.eleventh)
                        .Column(BodyColumn.second)
                        .Margins(10, 0, 20, 0),
                    new MediumButton
                    {
                        Text = "NEXT",
                        WidthRequest = 284,
                        HeightRequest = 47,
                    }
                        .Row(BodyRow.twelfth)
                        .ColumnSpan(2)
                        .BindCommand(nameof(viewModel.SaveCommand), source: viewModel)
                        .Margins(0, 50, 0, 0),
                },
            },
        };

        Loaded += NewProfileView_Loaded;
    }

    private async void NewProfileView_Loaded(object? sender, EventArgs e)
    {
        await viewModel.InitializeAsync();
    }
}
