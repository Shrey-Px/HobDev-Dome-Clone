namespace Player.Views.Account;

public class LoginView : AccountBaseView
{
    public LoginView(LoginViewModel viewModel)
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
                        (BodyRow.tenth, Star)
                    ),
                    Children =
                    {
                        new AccountTopControl().Row(BodyRow.first),
                        new ExtraBold28Label { Text = "Log In" }
                            .Row(BodyRow.second)
                            .Margins(20, 10, 20, 10),
                        new SingleImageBorderedEntry { PlaceholderText = "Email Address" }
                            .AppThemeBinding(
                                SingleImageBorderedEntry.ImgSourceProperty,
                                "email_light_theme.png",
                                "email_dark_theme.png"
                            )
                            .Row(BodyRow.third)
                            .Margins(20, 20, 20, 10)
                            .Bind(
                                SingleImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.Email),
                                BindingMode.TwoWay,
                                source: viewModel
                            ),
                        new Regular16Label { TextColor = Colors.Red }
                            .Bind(
                                Label.TextProperty,
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
                            .Margins(20, 0, 20, 10),
                        new DualImageBorderedEntry { PlaceholderText = "Password" }
                            .Bind(
                                DualImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.Password),
                                BindingMode.TwoWay,
                                source: viewModel
                            )
                            .Row(BodyRow.fifth)
                            .Margins(20, 20, 20, 10),
                        new Regular16Label { TextColor = Colors.Red }
                            .Bind(
                                Label.TextProperty,
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
                            .Margins(20, 0, 20, 10),
                        new Button
                        {
                            Text = "Forgot Password?",
                            Background = Colors.Transparent,
                            FontSize = 10,
                            FontFamily = "InterSemiBold",
                            TextColor = Color.FromArgb("#EF2F50"),
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Start,
                        }
                            .BindCommand(nameof(viewModel.ForgotPasswordCommand), source: viewModel)
                            .Row(BodyRow.seventh)
                            .Margins(20, 0, 20, 10),
                        new MediumButton { Text = "LOG IN", WidthRequest = 257 }
                            .BindCommand(nameof(viewModel.LoginCommand), source: viewModel)
                            .Row(BodyRow.eighth)
                            .Margins(20, 20, 20, 10),
                        new CompanyNameControl().Row(BodyRow.ninth).Margins(40, 80, 0, 0),
                        new Label
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.End,
                            FormattedText = new FormattedString
                            {
                                Spans =
                                {
                                    new Span
                                    {
                                        Text = "Don't have an account? ",
                                        FontFamily = "InterMedium",
                                        FontSize = 14,
                                    }.AppThemeBinding(
                                        Span.TextColorProperty,
                                        Colors.Black,
                                        Color.FromArgb("#BBBBBB")
                                    ),
                                    new Span
                                    {
                                        Text = "Sign Up",
                                        TextColor = Color.FromArgb("#EF2F50"),
                                        FontFamily = "InterBold",
                                        FontSize = 14,
                                    }.BindTapGesture(
                                        nameof(viewModel.SignUpCommand),
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
