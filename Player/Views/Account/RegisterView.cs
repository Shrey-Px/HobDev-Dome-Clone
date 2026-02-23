namespace Player.Views.Account;

public class RegisterView : AccountBaseView
{
    public RegisterView(RegisterViewModel viewModel)
        : base(viewModel)
    {
        try
        {
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
                        (BodyRow.tenth, Auto)
                    ),

                    Children =
                    {
                        new AccountTopControl().Row(BodyRow.first),
                        new ExtraBold28Label { Text = "Create Account" }
                            .Margins(20, 20, 0, 0)
                            .Row(BodyRow.second),
                        new SingleImageBorderedEntry { PlaceholderText = "Email" }
                            .Bind(
                                SingleImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.Email),
                                BindingMode.TwoWay,
                                source: viewModel
                            )
                            .Margins(20, 30, 20, 0)
                            .Row(BodyRow.third)
                            .AppThemeBinding(
                                SingleImageBorderedEntry.ImgSourceProperty,
                                "email_light_theme.png",
                                "email_dark_theme.png"
                            ),
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
                            .Row(BodyRow.fourth)
                            .Margins(20, 0, 20, 0),
                        new DualImageBorderedEntry { PlaceholderText = "Password" }
                            .Bind(
                                DualImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.Password),
                                BindingMode.TwoWay,
                                source: viewModel
                            )
                            .Row(BodyRow.fifth)
                            .Margins(20, 10, 20, 0),
                        new Regular16Label { TextColor = Colors.Red }
                            .Bind(
                                Regular16Label.TextProperty,
                                nameof(viewModel.PasswordError),
                                source: viewModel
                            )
                            .Bind(
                                IsVisibleProperty,
                                nameof(viewModel.PasswordError),
                                converter: new IsStringNotNullOrEmptyConverter(),
                                source: viewModel
                            )
                            .Row(BodyRow.sixth)
                            .Margins(20, 0, 20, 0),
                        new DualImageBorderedEntry { PlaceholderText = "Confirm Password" }
                            .Bind(
                                DualImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.ConfirmPassword),
                                BindingMode.TwoWay,
                                source: viewModel
                            )
                            .Row(BodyRow.seventh)
                            .Margins(20, 10, 20, 0),
                        new Regular16Label { TextColor = Colors.Red }
                            .Bind(
                                Regular16Label.TextProperty,
                                nameof(viewModel.ConfirmPasswordError),
                                source: viewModel
                            )
                            .Bind(
                                IsVisibleProperty,
                                nameof(viewModel.ConfirmPasswordError),
                                converter: new IsStringNotNullOrEmptyConverter(),
                                source: viewModel
                            )
                            .Row(BodyRow.eighth)
                            .Margins(20, 0, 20, 0),
                        new ImageButton { Source = "signup.png" }
                            .BindCommand(nameof(viewModel.RegisterUserCommand), source: viewModel)
                            .Row(BodyRow.ninth)
                            .Margins(0, 40, 0, 0),
                        new Label
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            FormattedText = new FormattedString
                            {
                                Spans =
                                {
                                    new Span
                                    {
                                        Text = "Already have an account? ",
                                        FontFamily = "InterMedium",
                                        FontSize = 14,
                                    }.AppThemeBinding(
                                        Span.TextColorProperty,
                                        Colors.Black,
                                        Color.FromArgb("#BBBBBB")
                                    ),
                                    new Span
                                    {
                                        Text = "Sign In",
                                        TextColor = Color.FromArgb("#EF2F50"),
                                        FontFamily = "InterBold",
                                        FontSize = 14,
                                    }.BindTapGesture(
                                        nameof(viewModel.SignInCommand),
                                        commandSource: viewModel
                                    ),
                                },
                            },
                        }
                            .Row(BodyRow.tenth)
                            .Margins(0, 2, 0, 0),
                    },
                },
            };
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
