namespace Admin.View;

public class EditVendorView : ContentPage
{
    EditVendorViewModel viewModel = null!;
    int count = 0;

    public EditVendorView(EditVendorViewModel viewModel)
    {
        try
        {
            Title = "Edit Partner";
            this.viewModel = viewModel;
            this.AppThemeBinding(
                ContentPage.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            );
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Both,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                Content = new HorizontalStackLayout
                {
                    Margin = new Thickness(10),

                    Spacing = 20,
                    Children =
                    {
                        new VerticalStackLayout
                        {
                            Children =
                            {
                                new CustomBorder
                                {
                                    Padding = 20,
                                    Content = new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, 20),
                                            (BodyRow.second, 40),
                                            (BodyRow.third, 35),
                                            (BodyRow.fourth, Auto),
                                            (BodyRow.fifth, 35),
                                            (BodyRow.sixth, 35),
                                            (BodyRow.seventh, Auto),
                                            (BodyRow.eighth, 35),
                                            (BodyRow.ninth, 35),
                                            (BodyRow.tenth, Auto),
                                            (BodyRow.eleventh, 35),
                                            (BodyRow.twelfth, 35),
                                            (BodyRow.thirteenth, Auto),
                                            (BodyRow.fourteenth, 35),
                                            (BodyRow.fifteenth, 35),
                                            (BodyRow.sixteenth, Auto),
                                            (BodyRow.seventeenth, 35),
                                            (BodyRow.eighteenth, Auto),
                                            (BodyRow.nineteenth, Auto),
                                            (BodyRow.twentieth, Auto),
                                            (BodyRow.twentyfirst, Auto),
                                            (BodyRow.twentysecond, 35),
                                            (BodyRow.twentythird, Auto)
                                        ),

                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, 300),
                                            (BodyColumn.second, 300)
                                        ),
                                        HorizontalOptions = LayoutOptions.Start,
                                        ColumnSpacing = 10,
                                        RowSpacing = 5,
                                        Children =
                                        {
                                            new Bold20Label { Text = "PARTNER DETAILS" }
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first),
                                            new BorderedEntryRectangle
                                            {
                                                PlaceholderText = "Stripe Account ID",
                                                WidthRequest = 300,
                                                VerticalOptions = LayoutOptions.Center,
                                                HorizontalOptions = LayoutOptions.End,
                                            }
                                                .Bind(
                                                    BorderedEntryRectangle.EntryTextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.StripeUserId)}"
                                                )
                                                .Row(BodyRow.first)
                                                .RowSpan(2)
                                                .Column(BodyColumn.second),
                                            new Bold20Label { Text = "Business Name" }
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first)
                                                .Margins(0, 15, 0, 0),
                                            new BorderedEntryRectangle
                                            {
                                                PlaceholderText = "Type here",
                                                WidthRequest = 610,
                                            }
                                                .Bind(
                                                    BorderedEntryRectangle.EntryTextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.FullName)}"
                                                )
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.BusinessNameError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.BusinessNameError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Bold20Label { Text = "Street" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.first),
                                            new BorderedEntryRectangle
                                            {
                                                PlaceholderText = "Type here",
                                                WidthRequest = 300,
                                            }
                                                .Bind(
                                                    BorderedEntryRectangle.EntryTextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.Street)}"
                                                )
                                                .Row(BodyRow.sixth)
                                                .Column(BodyColumn.first),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.StreetError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.StreetError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.seventh)
                                                .Column(BodyColumn.first),
                                            new Bold20Label { Text = "Postcode" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.second),
                                            new BorderedEntryRectangle
                                            {
                                                PlaceholderText = "Type here",
                                                WidthRequest = 300,
                                            }
                                                .Bind(
                                                    BorderedEntryRectangle.EntryTextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.PostCode)}"
                                                )
                                                .Row(BodyRow.sixth)
                                                .Column(BodyColumn.second),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.PostCodeError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.PostCodeError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.seventh)
                                                .Column(BodyColumn.second),
                                            new Bold20Label { Text = "Province" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.eighth)
                                                .Column(BodyColumn.first),
                                            new Picker
                                            {
                                                WidthRequest = 300,
                                                FontSize = 16,
                                                FontFamily = "InterRegular",
                                                ItemDisplayBinding = new Binding(
                                                    nameof(Province.ProvinceName)
                                                ),
                                            }
                                                .Bind(
                                                    Picker.ItemsSourceProperty,
                                                    nameof(viewModel.Provinces)
                                                )
                                                .Bind(
                                                    Picker.SelectedItemProperty,
                                                    nameof(viewModel.SelectedProvince)
                                                )
                                                .Row(BodyRow.ninth)
                                                .Column(BodyColumn.first),
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
                                                .Row(BodyRow.tenth)
                                                .Column(BodyColumn.first),
                                            new Bold20Label { Text = "City" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.eighth)
                                                .Column(BodyColumn.second),
                                            new Picker
                                            {
                                                WidthRequest = 300,
                                                FontSize = 16,
                                                FontFamily = "InterRegular",
                                                ItemDisplayBinding = new Binding(
                                                    nameof(City.CityName)
                                                ),
                                            }
                                                .Bind(
                                                    Picker.ItemsSourceProperty,
                                                    nameof(viewModel.Cities)
                                                )
                                                .Bind(
                                                    Picker.SelectedItemProperty,
                                                    nameof(viewModel.SelectedCity)
                                                )
                                                .Row(BodyRow.ninth)
                                                .Column(BodyColumn.second),
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
                                                .Row(BodyRow.tenth)
                                                .Column(BodyColumn.second),
                                            new Bold20Label { Text = "Latitude" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.eleventh)
                                                .Column(BodyColumn.first),
                                            new BorderedEntryRectangle
                                            {
                                                PlaceholderText = "Type here",
                                                WidthRequest = 300,
                                            }
                                                .Bind(
                                                    BorderedEntryRectangle.EntryTextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.Latitude)}"
                                                )
                                                .Row(BodyRow.twelfth)
                                                .Column(BodyColumn.first),
                                            new Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Label.TextProperty,
                                                    nameof(viewModel.LatitudeError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.LatitudeError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.thirteenth)
                                                .Column(BodyColumn.first),
                                            new Bold20Label { Text = "Longitude" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.eleventh)
                                                .Column(BodyColumn.second),
                                            new BorderedEntryRectangle
                                            {
                                                PlaceholderText = "Type here",
                                                WidthRequest = 300,
                                            }
                                                .Bind(
                                                    BorderedEntryRectangle.EntryTextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.Longitude)}"
                                                )
                                                .Row(BodyRow.twelfth)
                                                .Column(BodyColumn.second),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.LongitudeError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.LongitudeError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.thirteenth)
                                                .Column(BodyColumn.second),
                                            new Bold20Label { Text = "Contact Number" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.fourteenth)
                                                .Column(BodyColumn.first),
                                            new CustomBorder
                                            {
                                                Content = new Grid
                                                {
                                                    Padding = new Thickness(5, 0, 5, 0),
                                                    WidthRequest = 210,
                                                    ColumnDefinitions = Columns.Define(
                                                        (BodyColumn.first, Auto),
                                                        (BodyColumn.second, Star)
                                                    ),
                                                    HorizontalOptions = LayoutOptions.Start,
                                                    Children =
                                                    {
                                                        //new Regular14Label{ VerticalOptions=LayoutOptions.Center}.Bind(Label.TextProperty,nameof(viewModel.CountryText),source:viewModel).Column(BodyColumn.first),
                                                        new Regular16Label
                                                        {
                                                            VerticalOptions = LayoutOptions.Center,
                                                        }
                                                            .Bind(
                                                                Regular16Label.TextProperty,
                                                                nameof(viewModel.PhoneCode),
                                                                source: viewModel
                                                            )
                                                            .AppThemeBinding(
                                                                Label.TextColorProperty,
                                                                Colors.Black,
                                                                Colors.White
                                                            )
                                                            .Column(BodyColumn.first),
                                                        new CustomEntry
                                                        {
                                                            Placeholder = "Type here",
                                                            Keyboard = Keyboard.Telephone,
                                                        }
                                                            .Bind(
                                                                CustomEntry.TextProperty,
                                                                $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.MobileNumber)}"
                                                            )
                                                            .Column(BodyColumn.second),
                                                    },
                                                },
                                            }
                                                .Row(BodyRow.fifteenth)
                                                .Column(BodyColumn.first),
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
                                                .Row(BodyRow.sixteenth)
                                                .Column(BodyColumn.first),
                                            new Bold20Label { Text = "Email ID" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.fourteenth)
                                                .Column(BodyColumn.second),
                                            new SingleImageBorderedEntryRectangle
                                            {
                                                WidthRequest = 300,
                                            }
                                                .AppThemeBinding(
                                                    SingleImageBorderedEntryRectangle.ImgSourceProperty,
                                                    "email_black_24dp.png",
                                                    "email_white_24dp.png"
                                                )
                                                .Bind(
                                                    SingleImageBorderedEntryRectangle.EntryTextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Email)}"
                                                )
                                                .Row(BodyRow.fifteenth)
                                                .Column(BodyColumn.second),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.EmailError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.EmailError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.sixteenth)
                                                .Column(BodyColumn.second),
                                            new Bold20Label { Text = "About" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.seventeenth)
                                                .Column(BodyColumn.first),
                                            new CustomBorder
                                            {
                                                Padding = 0,
                                                Margin = new Thickness(0, 0, 0, 10),
                                                Content = new Editor
                                                {
                                                    AutoSize = EditorAutoSizeOption.TextChanges,
                                                    HeightRequest = 100,
                                                    MaxLength = 700,
                                                    FontSize = 16,
                                                    FontFamily = "InterRegular",
                                                }.Bind(
                                                    Editor.TextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.About)}"
                                                ),
                                            }
                                                .Row(BodyRow.eighteenth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.AboutError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.AboutError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.nineteenth)
                                                .Column(BodyColumn.first),
                                            new Bold20Label { Text = "Amenities" }
                                                .Row(BodyRow.twentyfirst)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Margins(0, 15, 0, 0),
                                            new AmenitiesControl()
                                                .Row(BodyRow.twentysecond)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Margins(5),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.SelectedAmenitiesError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.SelectedAmenitiesError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.twentythird)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                        },
                                    },
                                },
                            },
                        },
                        new VerticalStackLayout
                        {
                            Children =
                            {
                                new CustomBorder
                                {
                                    Padding = 20,

                                    Content = new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, 30),
                                            (BodyRow.second, 40),
                                            (BodyRow.third, 35),
                                            (BodyRow.fourth, Auto),
                                            (BodyRow.fifth, 35),
                                            (BodyRow.sixth, Auto),
                                            (BodyRow.seventh, Auto),
                                            (BodyRow.eighth, Auto),
                                            (BodyRow.ninth, Auto),
                                            (BodyRow.tenth, 55),
                                            (BodyRow.eleventh, Auto),
                                            (BodyRow.twelfth, Auto)
                                        ),

                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, 270),
                                            (BodyColumn.second, 270)
                                        ),

                                        ColumnSpacing = 10,

                                        Children =
                                        {
                                            new Bold20Label { Text = "PARTNER GAMES" }
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first),
                                            new Bold20Label { Text = "Sports Category" }
                                                .Margins(10, 10, 0, 0)
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first),
                                            new Picker
                                            {
                                                WidthRequest = 550,
                                                FontFamily = "InterRegular",
                                                FontSize = 16,
                                                ItemDisplayBinding = new Binding(
                                                    nameof(Game.GameName)
                                                ),
                                            }
                                                .Bind(
                                                    Picker.ItemsSourceProperty,
                                                    nameof(viewModel.AllGames)
                                                )
                                                .Bind(
                                                    Picker.SelectedItemProperty,
                                                    nameof(viewModel.SelectedGame)
                                                )
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.SelectedGameError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.SelectedGameError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Bold20Label { Text = "Fields Available" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.first),
                                            new Picker
                                            {
                                                WidthRequest = 270,
                                                FontFamily = "InterRegular",
                                                FontSize = 16,
                                            }
                                                .Bind(
                                                    Picker.ItemsSourceProperty,
                                                    nameof(viewModel.FieldsAndPlayersCount)
                                                )
                                                .Bind(
                                                    Picker.SelectedItemProperty,
                                                    nameof(viewModel.FieldCount)
                                                )
                                                .Row(BodyRow.sixth)
                                                .Column(BodyColumn.first),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.FieldCountError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.FieldCountError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.seventh)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Bold20Label { Text = "Maximum Players" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.second),
                                            new Picker
                                            {
                                                WidthRequest = 270,
                                                FontFamily = "InterRegular",
                                                FontSize = 16,
                                            }
                                                .Bind(
                                                    Picker.ItemsSourceProperty,
                                                    nameof(viewModel.FieldsAndPlayersCount)
                                                )
                                                .Bind(
                                                    Picker.SelectedItemProperty,
                                                    nameof(viewModel.PlayerCount)
                                                )
                                                .Row(BodyRow.sixth)
                                                .Column(BodyColumn.second),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.PlayerCountError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.PlayerCountError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.seventh)
                                                .Column(BodyColumn.second),
                                            new Grid
                                            {
                                                RowDefinitions = Rows.Define(
                                                    (BodyRow.first, 40),
                                                    (BodyRow.second, 40),
                                                    (BodyRow.third, 40),
                                                    (BodyRow.fourth, Auto),
                                                    (BodyRow.fifth, 40),
                                                    (BodyRow.sixth, 40),
                                                    (BodyRow.seventh, 40),
                                                    (BodyRow.eighth, Auto),
                                                    (BodyRow.ninth, 40),
                                                    (BodyRow.tenth, 40),
                                                    (BodyRow.eleventh, 40),
                                                    (BodyRow.twelfth, Auto),
                                                    (BodyRow.thirteenth, 40),
                                                    (BodyRow.fourteenth, 40),
                                                    (BodyRow.fifteenth, 40),
                                                    (BodyRow.sixteenth, Auto)
                                                ),

                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, 150),
                                                    (BodyColumn.second, 150),
                                                    (BodyColumn.third, 120)
                                                ),

                                                ColumnSpacing = 20,

                                                Children =
                                                {
                                                    new Bold20Label { Text = "Weekday" }
                                                        .Row(BodyRow.first)
                                                        .ColumnSpan(3),
                                                    new Bold20Label { Text = "Opening Time" }
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.first),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(viewModel.WeekdayOpenTime),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.third)
                                                        .Column(BodyColumn.first),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(viewModel.WeekdayStartTimeError),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(viewModel.WeekdayStartTimeError),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.fourth)
                                                        .Column(BodyColumn.first),
                                                    new Bold20Label { Text = "Closing Time" }
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.second),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(viewModel.WeekdayCloseTime),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.third)
                                                        .Column(BodyColumn.second),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(viewModel.WeekdayEndTimeError),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(viewModel.WeekdayEndTimeError),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.fourth)
                                                        .Column(BodyColumn.second),
                                                    new Bold20Label { Text = "Price" }
                                                        .Row(BodyRow.second)
                                                        .Column(BodyColumn.third),
                                                    new SingleImageBorderedEntryRectangle { }
                                                        .Bind(
                                                            SingleImageBorderedEntryRectangle.EntryTextProperty,
                                                            nameof(viewModel.WeekdayHourlyRate),
                                                            stringFormat: "{0:F2}"
                                                        )
                                                        .AppThemeBinding(
                                                            SingleImageBorderedEntryRectangle.ImgSourceProperty,
                                                            "attach_money_black_18dp.png",
                                                            "attach_money_white_18dp.png"
                                                        )
                                                        .Row(BodyRow.third)
                                                        .Column(BodyColumn.third),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekdayHourlyRateError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekdayHourlyRateError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.fourth)
                                                        .Column(BodyColumn.third),
                                                    new Bold20Label { Text = "Weekday Peak Hours" }
                                                        .Row(BodyRow.fifth)
                                                        .ColumnSpan(3)
                                                        .Margins(0, 10, 0, 0),
                                                    new Bold20Label { Text = "Start Time" }
                                                        .Row(BodyRow.sixth)
                                                        .Column(BodyColumn.first),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursStartTime
                                                            ),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.seventh)
                                                        .Column(BodyColumn.first),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursStartTimeError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursStartTimeError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.eighth)
                                                        .Column(BodyColumn.first),
                                                    new Bold20Label { Text = "End Time" }
                                                        .Row(BodyRow.sixth)
                                                        .Column(BodyColumn.second),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursEndTime
                                                            ),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.seventh)
                                                        .Column(BodyColumn.second),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursEndTimeError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursEndTimeError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.eighth)
                                                        .Column(BodyColumn.second),
                                                    new Bold20Label { Text = "Price" }
                                                        .Row(BodyRow.sixth)
                                                        .Column(BodyColumn.third),
                                                    new SingleImageBorderedEntryRectangle { }
                                                        .Bind(
                                                            SingleImageBorderedEntryRectangle.EntryTextProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursHourlyRate
                                                            ),
                                                            stringFormat: "{0:F2}"
                                                        )
                                                        .AppThemeBinding(
                                                            SingleImageBorderedEntryRectangle.ImgSourceProperty,
                                                            "attach_money_black_18dp.png",
                                                            "attach_money_white_18dp.png"
                                                        )
                                                        .Row(BodyRow.seventh)
                                                        .Column(BodyColumn.third),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursHourlyRateError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekdayPeakHoursHourlyRateError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.eighth)
                                                        .Column(BodyColumn.third),
                                                    new Bold20Label { Text = "Weekend" }
                                                        .Row(BodyRow.ninth)
                                                        .ColumnSpan(3)
                                                        .Margins(0, 10, 0, 0),
                                                    new Bold20Label { Text = "Opening Time" }
                                                        .Row(BodyRow.tenth)
                                                        .Column(BodyColumn.first),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(viewModel.WeekendOpenTime),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.eleventh)
                                                        .Column(BodyColumn.first),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(viewModel.WeekendStartTimeError),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(viewModel.WeekendStartTimeError),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.twelfth)
                                                        .Column(BodyColumn.first),
                                                    new Bold20Label { Text = "Closing Time" }
                                                        .Row(BodyRow.tenth)
                                                        .Column(BodyColumn.second),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(viewModel.WeekendCloseTime),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.eleventh)
                                                        .Column(BodyColumn.second),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(viewModel.WeekendEndTimeError),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(viewModel.WeekendEndTimeError),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.twelfth)
                                                        .Column(BodyColumn.second),
                                                    new Bold20Label { Text = "Price" }
                                                        .Row(BodyRow.tenth)
                                                        .Column(BodyColumn.third),
                                                    new SingleImageBorderedEntryRectangle { }
                                                        .Bind(
                                                            SingleImageBorderedEntryRectangle.EntryTextProperty,
                                                            nameof(viewModel.WeekendHourlyRate),
                                                            stringFormat: "{0:F2}"
                                                        )
                                                        .AppThemeBinding(
                                                            SingleImageBorderedEntryRectangle.ImgSourceProperty,
                                                            "attach_money_black_18dp.png",
                                                            "attach_money_white_18dp.png"
                                                        )
                                                        .Row(BodyRow.eleventh)
                                                        .Column(BodyColumn.third),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekendHourlyRateError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekendHourlyRateError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.twelfth)
                                                        .Column(BodyColumn.third),
                                                    new Bold20Label { Text = "Weekend Peak Hours" }
                                                        .Row(BodyRow.thirteenth)
                                                        .ColumnSpan(3)
                                                        .Margins(0, 10, 0, 0),
                                                    new Bold20Label { Text = "Start Time" }
                                                        .Row(BodyRow.fourteenth)
                                                        .Column(BodyColumn.first),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursStartTime
                                                            ),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.fifteenth)
                                                        .Column(BodyColumn.first),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursStartTimeError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursStartTimeError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.sixteenth)
                                                        .Column(BodyColumn.first),
                                                    new Bold20Label { Text = "End Time" }
                                                        .Row(BodyRow.fourteenth)
                                                        .Column(BodyColumn.second),
                                                    new VenueTimingControl { }
                                                        .Bind(
                                                            VenueTimingControl.TimeProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursEndTime
                                                            ),
                                                            BindingMode.TwoWay
                                                        )
                                                        .Row(BodyRow.fifteenth)
                                                        .Column(BodyColumn.second),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursEndTimeError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursEndTimeError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.sixteenth)
                                                        .Column(BodyColumn.second),
                                                    new Bold20Label { Text = "Price" }
                                                        .Row(BodyRow.fourteenth)
                                                        .Column(BodyColumn.third),
                                                    new SingleImageBorderedEntryRectangle { }
                                                        .Bind(
                                                            SingleImageBorderedEntryRectangle.EntryTextProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursHourlyRate
                                                            ),
                                                            stringFormat: "{0:F2}"
                                                        )
                                                        .AppThemeBinding(
                                                            SingleImageBorderedEntryRectangle.ImgSourceProperty,
                                                            "attach_money_black_18dp.png",
                                                            "attach_money_white_18dp.png"
                                                        )
                                                        .Row(BodyRow.fifteenth)
                                                        .Column(BodyColumn.third),
                                                    new Regular16Label { TextColor = Colors.Red }
                                                        .Bind(
                                                            Regular16Label.TextProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursHourlyRateError
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Bind(
                                                            IsVisibleProperty,
                                                            nameof(
                                                                viewModel.WeekendPeakHoursHourlyRateError
                                                            ),
                                                            converter: new IsStringNotNullOrEmptyConverter(),
                                                            source: viewModel
                                                        )
                                                        .Row(BodyRow.sixteenth)
                                                        .Column(BodyColumn.third),
                                                },
                                            }
                                                .Row(BodyRow.eighth)
                                                .ColumnSpan(2)
                                                .Margins(0, 30, 30, 0),
                                            new Bold20Label { Text = "First Photo" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.ninth)
                                                .Column(BodyColumn.first),
                                            new Image
                                            {
                                                Aspect = Aspect.AspectFill,
                                                HeightRequest = 65,
                                                WidthRequest = 100,
                                                HorizontalOptions = LayoutOptions.Center,
                                            }
                                                .Bind(
                                                    Image.SourceProperty,
                                                    nameof(viewModel.GameFirstImage),
                                                    converter: new ByteArrayToImageSourceConverter()
                                                )
                                                .Row(BodyRow.tenth)
                                                .Column(BodyColumn.first),
                                            new HorizontalStackLayout
                                            {
                                                Spacing = 20,
                                                HorizontalOptions = LayoutOptions.Center,
                                                Margin = new Thickness(0, 10, 0, 10),
                                                Children =
                                                {
                                                    new RegularButton
                                                    {
                                                        Text = "+",
                                                        FontSize = 26,
                                                    }.BindCommand(
                                                        nameof(viewModel.PickFirstPhotoCommand)
                                                    ),
                                                    new RegularButton
                                                    {
                                                        Text = "-",
                                                        FontSize = 26,
                                                    }.BindCommand(
                                                        nameof(viewModel.RemoveFirstPhotoCommand)
                                                    ),
                                                },
                                            }
                                                .Row(BodyRow.eleventh)
                                                .Column(BodyColumn.first),
                                            new Bold20Label { Text = "Second Photo" }
                                                .Margins(0, 10, 0, 0)
                                                .Row(BodyRow.ninth)
                                                .Column(BodyColumn.second),
                                            new Image
                                            {
                                                Aspect = Aspect.AspectFill,
                                                HeightRequest = 65,
                                                WidthRequest = 100,
                                                HorizontalOptions = LayoutOptions.Center,
                                            }
                                                .Bind(
                                                    Image.SourceProperty,
                                                    nameof(viewModel.GameSecondImage),
                                                    converter: new ByteArrayToImageSourceConverter()
                                                )
                                                .Row(BodyRow.tenth)
                                                .Column(BodyColumn.second),
                                            new HorizontalStackLayout
                                            {
                                                HorizontalOptions = LayoutOptions.Center,
                                                Spacing = 20,
                                                Margin = new Thickness(0, 10, 0, 10),
                                                Children =
                                                {
                                                    new RegularButton
                                                    {
                                                        Text = "+",
                                                        FontSize = 26,
                                                    }.BindCommand(
                                                        nameof(viewModel.PickSecondPhotoCommand)
                                                    ),
                                                    new RegularButton
                                                    {
                                                        Text = "-",
                                                        FontSize = 26,
                                                    }.BindCommand(
                                                        nameof(viewModel.RemoveSecondPhotoCommand)
                                                    ),
                                                },
                                            }
                                                .Row(BodyRow.eleventh)
                                                .Column(BodyColumn.second),
                                            new Regular16Label { TextColor = Colors.Red }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(viewModel.GameImageError),
                                                    source: viewModel
                                                )
                                                .Bind(
                                                    IsVisibleProperty,
                                                    nameof(viewModel.GameImageError),
                                                    converter: new IsStringNotNullOrEmptyConverter(),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.twelfth)
                                                .Column(BodyColumn.first),
                                            new RegularButton
                                            {
                                                Text = "Add",
                                                HeightRequest = 38,
                                                FontSize = 18,
                                                FontFamily = "InterBold",
                                                Margin = new Thickness(20, 0, 20, 0),
                                            }
                                                .BindCommand(
                                                    static (EditVendorViewModel vm) =>
                                                        vm.AddGameCommand,
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.thirteenth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                        },
                                    },
                                },
                            },
                        },
                        new CollectionView
                        {
                            ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                            WidthRequest = 300,
                            EmptyView = "Minimum one game is required",
                            ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical)
                            {
                                HorizontalItemSpacing = 10,
                            },
                            ItemTemplate = new DataTemplate(() =>
                            {
                                return new CustomBorder
                                {
                                    Padding = 8,
                                    Content = new Grid
                                    {
                                        HorizontalOptions = LayoutOptions.Start,
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, Auto),
                                            (BodyRow.second, Auto),
                                            (BodyRow.third, Auto),
                                            (BodyRow.fourth, Auto),
                                            (BodyRow.fifth, Auto)
                                        ),

                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, Auto),
                                            (BodyColumn.second, Auto)
                                        ),
                                        RowSpacing = 5,
                                        Children =
                                        {
                                            new Bold20Label { }
                                                .Bind(
                                                    Bold20Label.TextProperty,
                                                    nameof(AvailableGame.Name)
                                                )
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first),
                                            new Regular16Label { }
                                                .Bind(
                                                    Regular16Label.TextProperty,
                                                    nameof(AvailableGame.FieldInformation)
                                                )
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first),
                                            new Regular16Label { }
                                                .Bind(
                                                    Regular18Label.TextProperty,
                                                    binding1: new Binding(
                                                        $"{nameof(AvailableGame.Timing)}.{nameof(AvailableGame.Timing.WeekdayOpenTime)}",
                                                        stringFormat: "{0:HH:mm}"
                                                    ),
                                                    binding2: new Binding(
                                                        $"{nameof(AvailableGame.Timing)}.{nameof(AvailableGame.Timing.WeekdayCloseTime)}",
                                                        stringFormat: "{0:HH:mm}"
                                                    ),
                                                    convert: (
                                                        (string? openTime, string? closeTime) values
                                                    ) =>
                                                        $" weekday: {values.openTime} to {values.closeTime}"
                                                )
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first),
                                            new Bold16Label { }
                                                .Bind(
                                                    Bold18Label.TextProperty,
                                                    nameof(AvailableGame.WeekdayHourlyRate),
                                                    BindingMode.OneWay,
                                                    stringFormat: "{0:C}"
                                                )
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.second),
                                            new Regular16Label { }
                                                .Bind(
                                                    Regular18Label.TextProperty,
                                                    binding1: new Binding(
                                                        $"{nameof(AvailableGame.Timing)}.{nameof(viewModel.WeekendOpenTime)}",
                                                        stringFormat: "{0:HH:mm}"
                                                    ),
                                                    binding2: new Binding(
                                                        $"{nameof(AvailableGame.Timing)}.{nameof(viewModel.WeekendCloseTime)}",
                                                        stringFormat: "{0:HH:mm}"
                                                    ),
                                                    convert: (
                                                        (string? openTime, string? closeTime) values
                                                    ) =>
                                                        $" weekend: {values.openTime} to {values.closeTime}"
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first),
                                            new Bold16Label { }
                                                .Bind(
                                                    Bold18Label.TextProperty,
                                                    nameof(AvailableGame.WeekendHourlyRate),
                                                    BindingMode.OneWay,
                                                    stringFormat: "{0:C}"
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.second),
                                            new Bold12Label { Text = "Photos Added" }
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.first),
                                            new ImageButton
                                            {
                                                Source = "remove_game.png",
                                                HeightRequest = 24,
                                                WidthRequest = 24,
                                                Background = Colors.Transparent,
                                            }
                                                .BindCommand(
                                                    nameof(viewModel.RemoveGameCommand),
                                                    source: viewModel,
                                                    parameterPath: "."
                                                )
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.second),
                                        },
                                    },
                                };
                            }),
                        }
                            .Bind(
                                ItemsView.ItemsSourceProperty,
                                static (EditVendorViewModel vm) => vm.AddedGames,
                                source: viewModel
                            )
                            .Margins(20, 30, 20),
                        new VerticalStackLayout
                        {
                            Children =
                            {
                                new CustomBorder
                                {
                                    Padding = new Thickness(20, 15, 20, 10),
                                    Content = new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, 30),
                                            (BodyRow.second, 40),
                                            (BodyRow.third, 55),
                                            (BodyRow.fourth, 25),
                                            (BodyRow.fifth, 25),
                                            (BodyRow.sixth, 30),
                                            (BodyRow.seventh, Auto),
                                            (BodyRow.eighth, 30),
                                            (BodyRow.ninth, 30),
                                            (BodyRow.tenth, 40),
                                            (BodyRow.eleventh, 40),
                                            (BodyRow.twelfth, 60),
                                            (BodyRow.thirteenth, 60)
                                        ),

                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, 250),
                                            (BodyColumn.second, 175)
                                        ),
                                        RowSpacing = 2,
                                        Children =
                                        {
                                            new Bold20Label { Text = "Facility Review" }
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Bold20Label { }
                                                .Bind(
                                                    Bold20Label.TextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.FullName)}",
                                                    BindingMode.OneWay
                                                )
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Margins(0, 10, 0, 0),
                                            new Regular18Label { }
                                                .FormattedText(
                                                    new[]
                                                    {
                                                        new Span { }.Bind(
                                                            Span.TextProperty,
                                                            $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.Street)}",
                                                            BindingMode.OneWay,
                                                            stringFormat: "{0}, "
                                                        ),
                                                        new Span { }.Bind(
                                                            Span.TextProperty,
                                                            $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.Province)}",
                                                            BindingMode.OneWay,
                                                            stringFormat: "{0}, "
                                                        ),
                                                        new Span { }.Bind(
                                                            Span.TextProperty,
                                                            $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.City)}",
                                                            BindingMode.OneWay,
                                                            stringFormat: "{0}, "
                                                        ),
                                                        new Span { }.Bind(
                                                            Span.TextProperty,
                                                            $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.PostCode)}",
                                                            BindingMode.OneWay,
                                                            stringFormat: "{0}, "
                                                        ),
                                                        new Span { }.Bind(
                                                            Span.TextProperty,
                                                            $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Address)}.{nameof(viewModel.Vendor.Address.Country)}",
                                                            BindingMode.OneWay
                                                        ),
                                                    }
                                                )
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Margins(0, 0, 0, 0),
                                            new Regular18Label { }
                                                .Bind(
                                                    Regular18Label.TextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.MobileNumber)}",
                                                    BindingMode.OneWay
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Regular18Label { }
                                                .Bind(
                                                    Regular18Label.TextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Email)}",
                                                    BindingMode.OneWay
                                                )
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Regular18Label { }
                                                .Bind(
                                                    Regular18Label.TextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.Email)}",
                                                    BindingMode.OneWay,
                                                    stringFormat: "Vendor Username: {0}"
                                                )
                                                .Row(BodyRow.sixth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Regular18Label { }
                                                .Bind(
                                                    Regular18Label.TextProperty,
                                                    $"{nameof(viewModel.Vendor)}.{nameof(viewModel.Vendor.About)}",
                                                    BindingMode.OneWay
                                                )
                                                .Row(BodyRow.seventh)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new CollectionView
                                            {
                                                EmptyView = "No Amenity Selected",
                                                ItemSizingStrategy =
                                                    ItemSizingStrategy.MeasureAllItems,
                                                ItemsLayout = new LinearItemsLayout(
                                                    ItemsLayoutOrientation.Horizontal
                                                )
                                                {
                                                    ItemSpacing = 5,
                                                },
                                                ItemTemplate = new DataTemplate(() =>
                                                {
                                                    return new Regular18Label
                                                    {
                                                        LineBreakMode = LineBreakMode.NoWrap,
                                                    }.Bind(
                                                        Regular14Label.TextProperty,
                                                        nameof(Amenity.AmenityName),
                                                        BindingMode.OneWay
                                                    );
                                                }),
                                            }
                                                .Bind(
                                                    ItemsView.ItemsSourceProperty,
                                                    nameof(viewModel.SelectedAmenities),
                                                    BindingMode.OneWay,
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.tenth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new CollectionView
                                            {
                                                EmptyView = "Games not added",
                                                ItemsLayout = new GridItemsLayout(
                                                    ItemsLayoutOrientation.Horizontal
                                                )
                                                {
                                                    HorizontalItemSpacing = 5,
                                                },
                                                ItemTemplate = new DataTemplate(() =>
                                                {
                                                    return new CustomBorder
                                                    {
                                                        HeightRequest = 35,
                                                        Padding = new Thickness(10, 5),
                                                        Content = new Regular14Label { }.Bind(
                                                            Regular12Label.TextProperty,
                                                            nameof(AvailableGame.Name),
                                                            BindingMode.OneWay
                                                        ),
                                                    };
                                                }),
                                            }
                                                .Bind(
                                                    ItemsView.ItemsSourceProperty,
                                                    nameof(viewModel.AddedGames)
                                                )
                                                .Row(BodyRow.eleventh)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new RegularButton
                                            {
                                                Text = "SAVE CHANGES",
                                                HeightRequest = 40,
                                                FontFamily = "InterBold",
                                                FontSize = 18,
                                            }
                                                .BindCommand(
                                                    nameof(viewModel.ValidateDataCommand),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.twelfth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new RegularButton
                                            {
                                                Text = "CANCEL",
                                                HeightRequest = 40,
                                                Background = Colors.Black,
                                                FontFamily = "InterBold",
                                                FontSize = 18,
                                            }
                                                .BindCommand(
                                                    nameof(viewModel.CancelCommand),
                                                    source: viewModel
                                                )
                                                .Row(BodyRow.thirteenth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };

            BindingContext = viewModel;

            Loaded += EditVendorView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // can't use this method because the Loaded event is fired and reset the data whenever navigating to another page or when a popup is opened
    private async void EditVendorView_Loaded(object? sender, EventArgs e)
    {
        if (count == 0)
        {
            await viewModel.EditExistingVendor();
            count++;
        }
    }
}
