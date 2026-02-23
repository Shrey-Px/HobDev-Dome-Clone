namespace Player.Views.Account;

public class AccountView : AccountBaseView
{
    public AccountView(AccountViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            Resources = new ResourceDictionary
            {
                new Style<Button>((Button.BackgroundProperty, Color.FromArgb("EF2F50"))),
            };

            Content = new ScrollView
            {
                Content = new Grid
                {
                    RowDefinitions = Rows.Define(
                        (BodyRow.first, Auto),
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
                        (BodyRow.twelfth, Auto),
                        (BodyRow.thirteenth, Auto),
                        (BodyRow.fourteenth, Auto)
                    ),

                    Children =
                    {
                        new AccountTopControl(),
                        new ExtraBold28Label { Text = "Settings" }
                            .Row(BodyRow.first)
                            .Margins(30, 120, 0, 0),
                        new Grid
                        {
                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, Auto),
                                (BodyColumn.second, Star)
                            ),

                            ColumnSpacing = 10,
                            Children =
                            {
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
                                    .Column(BodyColumn.first),
                                new VerticalStackLayout
                                {
                                    VerticalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                        new Label { }.FormattedText(
                                            new[]
                                            {
                                                new Span { FontFamily = "InterBold", FontSize = 28 }
                                                    .AppThemeBinding(
                                                        Span.TextColorProperty,
                                                        Colors.Black,
                                                        Colors.White
                                                    )
                                                    .Bind(
                                                        Span.TextProperty,
                                                        $"{nameof(viewModel.VenueUser)}.{nameof(viewModel.VenueUser.FirstName)}",
                                                        stringFormat: "{0}\n"
                                                    ),
                                                new Span
                                                {
                                                    FontFamily = "InterRegular",
                                                    FontSize = 28,
                                                }
                                                    .AppThemeBinding(
                                                        Span.TextColorProperty,
                                                        Colors.Black,
                                                        Colors.White
                                                    )
                                                    .Bind(
                                                        Span.TextProperty,
                                                        $"{nameof(viewModel.VenueUser)}.{nameof(viewModel.VenueUser.LastName)}"
                                                    ),
                                            }
                                        ),
                                        new Regular14Label
                                        {
                                            VerticalOptions = LayoutOptions.Start,
                                        }.Bind(
                                            Regular14Label.TextProperty,
                                            $"{nameof(viewModel.VenueUser)}.{nameof(viewModel.VenueUser.Email)}"
                                        ),
                                    },
                                }.Column(BodyColumn.second),
                            },
                        }
                            .Row(BodyRow.second)
                            .Margins(30, 10, 30, 0),
                        new HorizontalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Image { }.AppThemeBinding(
                                    Image.SourceProperty,
                                    "account_icon_light_theme.png",
                                    "account_icon_dark_theme.png"
                                ),
                                new ExtraBold18Label
                                {
                                    Text = "Account",
                                    VerticalOptions = LayoutOptions.Center,
                                },
                            },
                        }
                            .Row(BodyRow.third)
                            .Margins(30, 20, 0, 10),
                        new Rectangle { HeightRequest = 2 }
                            .AppThemeBinding(
                                Rectangle.BackgroundProperty,
                                Colors.Black,
                                Color.FromArgb("#A3A3A3")
                            )
                            .Row(BodyRow.fourth),
                        new Grid
                        {
                            Children =
                            {
                                new Medium16Label
                                {
                                    HorizontalOptions = LayoutOptions.Fill,
                                    Text = "Dark Theme",
                                    VerticalOptions = LayoutOptions.Center,
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                                new Switch
                                {
                                    OnColor = Colors.LightGrey,
                                    ThumbColor = Color.FromArgb("#EF2F50"),
                                    VerticalOptions = LayoutOptions.Start,
                                    HorizontalOptions = LayoutOptions.End,
                                }.Bind(
                                    Switch.IsToggledProperty,
                                    nameof(viewModel.IsDarkThemWantedByUser)
                                ),
                            },
                        }
                            .Row(BodyRow.fifth)
                            .BindTapGesture(nameof(viewModel.EditProfileCommand))
                            .Margins(30, 10, 20, 0),
                        new Grid
                        {
                            Children =
                            {
                                new Medium16Label
                                {
                                    HorizontalOptions = LayoutOptions.Fill,
                                    Text = "Profile",
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                                new Medium16Label
                                {
                                    Text = ">",
                                    HorizontalOptions = LayoutOptions.End,
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                            },
                        }
                            .Row(BodyRow.sixth)
                            .BindTapGesture(nameof(viewModel.EditProfileCommand))
                            .Margins(30, 10, 30, 0),
                        new Grid
                        {
                            Children =
                            {
                                new Medium16Label
                                {
                                    HorizontalOptions = LayoutOptions.Fill,
                                    Text = "Age & Interests",
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                                new Medium16Label
                                {
                                    Text = ">",
                                    HorizontalOptions = LayoutOptions.End,
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                            },
                        }
                            .Row(BodyRow.seventh)
                            .BindTapGesture(nameof(viewModel.EditAgeAndInterestsCommand))
                            .Margins(30, 20, 30, 0),
                        new Grid
                        {
                            Children =
                            {
                                new Medium16Label
                                {
                                    HorizontalOptions = LayoutOptions.Fill,
                                    Text = "Change Password",
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                                new Medium16Label
                                {
                                    Text = ">",
                                    HorizontalOptions = LayoutOptions.End,
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                            },
                        }
                            .Row(BodyRow.eighth)
                            .BindTapGesture(nameof(viewModel.ChangePasswordCommand))
                            .Margins(30, 20, 30, 0),
                        new Grid
                        {
                            Children =
                            {
                                new Medium16Label
                                {
                                    HorizontalOptions = LayoutOptions.Fill,
                                    Text = "Delete Account",
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                                new Medium16Label
                                {
                                    Text = ">",
                                    HorizontalOptions = LayoutOptions.End,
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                            },
                        }
                            .Row(BodyRow.ninth)
                            .BindTapGesture(nameof(viewModel.DeleteAccountCommand))
                            .Margins(30, 20, 30, 0),
                        new HorizontalStackLayout
                        {
                            Spacing = 5,
                            Children =
                            {
                                new Image { }.AppThemeBinding(
                                    Image.SourceProperty,
                                    "notification_light_theme.png",
                                    "notification_dark_theme.png"
                                ),
                                new ExtraBold18Label { Text = "Notifications" },
                            },
                        }
                            .Row(BodyRow.tenth)
                            .Margins(30, 40, 30, 10),
                        new Rectangle { HeightRequest = 2 }
                            .AppThemeBinding(
                                Rectangle.BackgroundProperty,
                                Colors.Black,
                                Color.FromArgb("#A3A3A3")
                            )
                            .Row(BodyRow.eleventh),
                        new Grid
                        {
                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, Star),
                                (BodyColumn.second, Auto),
                                (BodyColumn.third, Auto)
                            ),
                            ColumnSpacing = 10,
                            Children =
                            {
                                new Medium16Label
                                {
                                    HorizontalOptions = LayoutOptions.Start,
                                    Text = "Mobile Number",
                                }.AppThemeBinding(
                                    Medium16Label.TextColorProperty,
                                    Colors.Black,
                                    Color.FromArgb("#BBBBBB")
                                ),
                                new MediumButton
                                {
                                    Text = "Change",
                                    CornerRadius = 7,
                                    FontSize = 8,
                                    WidthRequest = 65,
                                    Padding = new Thickness(0, 2),
                                }
                                    .Column(BodyColumn.second)
                                    .BindCommand(nameof(viewModel.ChangeMobileNumberCommand)),
                                new MediumButton
                                {
                                    Text = "Confirm",
                                    CornerRadius = 7,
                                    FontSize = 8,
                                    Background = Colors.Black,
                                    WidthRequest = 65,
                                    Padding = new Thickness(0, 2),
                                }
                                    .Bind(
                                        MediumButton.IsVisibleProperty,
                                        $"{nameof(viewModel.VenueUser)}.{nameof(viewModel.VenueUser.IsMobileNumberVerified)}",
                                        convert: (bool verified) => verified == true ? false : true
                                    )
                                    .Column(BodyColumn.third)
                                    .BindCommand(nameof(viewModel.ConfirmMobileNumberCommand)),
                            },
                        }
                            .Row(BodyRow.twelfth)
                            .Margins(30, 20, 30, 0),
                        new MediumButton
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            Text = "Logout",
                            WidthRequest = 133,
                            HeightRequest = 47,
                            CornerRadius = 20,
                        }
                            .Row(BodyRow.thirteenth)
                            .BindCommand(nameof(viewModel.LogoutCommand))
                            .Margins(30, 80, 30, 0),
                        new MediumButton
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            Text = "Contact Us",
                            Padding = 0,
                            TextColor = Color.FromArgb("#EF2F50"),
                            Background = Colors.Transparent,
                        }
                            .Row(BodyRow.fourteenth)
                            .BindCommand(nameof(viewModel.EmailUsCommand))
                            .Margins(30, 15, 30, 15),
                    },
                },
            };

            BindingContext = viewModel;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
