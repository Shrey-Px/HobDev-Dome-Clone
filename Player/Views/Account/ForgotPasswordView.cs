namespace Player.Views.Account;

public class ForgotPasswordView : AccountBaseView
{
    public ForgotPasswordView(ForgotPasswordViewModel viewModel)
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
                        (BodyRow.fourth, Star)
                    ),
                    Children =
                    {
                        new AccountTopControl().Row(BodyRow.first),
                        new VerticalStackLayout
                        {
                            Margin = new Thickness(20, 10, 20, 10),
                            Children =
                            {
                                new Bold28Label { Text = "Forgot Password" },
                                new SingleImageBorderedEntry { PlaceholderText = "Email Address" }
                                    .AppThemeBinding(
                                        SingleImageBorderedEntry.ImgSourceProperty,
                                        "email_light_theme.png",
                                        "email_dark_theme.png"
                                    )
                                    .Margins(0, 20, 0, 0)
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
                                    ),
                                new DualImageBorderedEntry { PlaceholderText = "New Password" }
                                    .Bind(
                                        DualImageBorderedEntry.EntryTextProperty,
                                        nameof(viewModel.NewPassword),
                                        BindingMode.TwoWay,
                                        source: viewModel
                                    )
                                    .Margins(0, 15, 0, 0),
                                new Regular16Label { TextColor = Colors.Red }
                                    .Bind(
                                        Label.TextProperty,
                                        nameof(viewModel.NewPasswordError),
                                        source: viewModel
                                    )
                                    .Bind(
                                        IsVisibleProperty,
                                        nameof(viewModel.NewPasswordError),
                                        converter: new IsStringNotNullOrEmptyConverter(),
                                        source: viewModel
                                    ),
                                new DualImageBorderedEntry
                                {
                                    PlaceholderText = "Confirm New Password",
                                }
                                    .Bind(
                                        DualImageBorderedEntry.EntryTextProperty,
                                        nameof(viewModel.ConfirmNewPassword),
                                        BindingMode.TwoWay,
                                        source: viewModel
                                    )
                                    .Margins(0, 15, 0, 0),
                                new Regular16Label { TextColor = Colors.Red }
                                    .Bind(
                                        Label.TextProperty,
                                        nameof(viewModel.ConfirmNewPasswordError),
                                        source: viewModel
                                    )
                                    .Bind(
                                        IsVisibleProperty,
                                        nameof(viewModel.ConfirmNewPasswordError),
                                        converter: new IsStringNotNullOrEmptyConverter(),
                                        source: viewModel
                                    ),
                                new MediumButton
                                {
                                    Text = "SUBMIT",
                                    WidthRequest = 257,
                                    HeightRequest = 47,
                                }
                                    .BindCommand(
                                        nameof(viewModel.ResetPasswordCommand),
                                        source: viewModel
                                    )
                                    .Margins(0, 40, 0, 0),
                                new MediumButton
                                {
                                    Text = "CANCEL",
                                    WidthRequest = 257,
                                    HeightRequest = 47,
                                    Background = Color.FromArgb("#272727"),
                                }
                                    .BindCommand(
                                        nameof(viewModel.CancelPasswordResetCommand),
                                        source: viewModel
                                    )
                                    .Margins(0, 15, 0, 0),
                            },
                        }.Row(BodyRow.second),
                        new CompanyNameControl().Row(BodyRow.third).Margins(20, 40, 0, 0),
                        new HorizontalStackLayout
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.End,
                            Spacing = 5,
                            Children =
                            {
                                new Medium14Label { Text = "Already have an account?" },
                                new Button
                                {
                                    Text = "Sign In",
                                    Background = Colors.Transparent,
                                    TextColor = Color.FromArgb("#EF2F50"),
                                    FontFamily = "InterBold",
                                    FontSize = 14,
                                    Padding = 0,
                                }.BindCommand(nameof(viewModel.SignInCommand), source: viewModel),
                            },
                        }
                            .Row(BodyRow.fourth)
                            .Margins(0, 0, 0, 20),
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
