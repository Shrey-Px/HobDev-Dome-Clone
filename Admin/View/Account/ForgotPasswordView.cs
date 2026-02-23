namespace Admin.View.Account;

public class ForgotPasswordView : ContentPage
{
    public ForgotPasswordView(ForgotPasswordViewModel viewModel)
    {
        try
        {
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            this.AppThemeBinding(
                ContentPage.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            );

            Content = new ScrollView
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                Orientation = ScrollOrientation.Both,
                Content = new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(20, 10, 20, 10),
                    Children =
                    {
                        new Regular20Label { Text = "Email Address" }.Margins(10, 20, 0, 0),
                        new SingleImageBorderedEntry { WidthRequest = 600 }
                            .AppThemeBinding(
                                SingleImageBorderedEntry.ImgSourceProperty,
                                "email_black_24dp.png",
                                "email_white_24dp.png"
                            )
                            .Margins(0, 10, 0, 0)
                            .Bind(
                                SingleImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.Email),
                                BindingMode.TwoWay,
                                source: viewModel
                            ),
                        new Regular16Label
                        {
                            TextColor = Colors.Red,
                            HorizontalOptions = LayoutOptions.Start,
                        }
                            .Bind(
                                Label.TextProperty,
                                static (ForgotPasswordViewModel vm) => vm.EmailError,
                                source: viewModel
                            )
                            .Bind(
                                IsVisibleProperty,
                                nameof(viewModel.EmailError),
                                converter: new IsStringNotNullOrEmptyConverter(),
                                source: viewModel
                            )
                            .Margins(10, 0, 0, 0),
                        new Regular20Label { Text = "New Password" }.Margins(10, 20, 0, 0),
                        new DualImageBorderedEntry { WidthRequest = 600 }
                            .Bind(
                                DualImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.NewPassword),
                                BindingMode.TwoWay,
                                source: viewModel
                            )
                            .Margins(0, 10, 0, 0),
                        new Regular16Label
                        {
                            TextColor = Colors.Red,
                            HorizontalOptions = LayoutOptions.Start,
                        }
                            .Bind(
                                Label.TextProperty,
                                static (ForgotPasswordViewModel vm) => vm.NewPasswordError,
                                source: viewModel
                            )
                            .Bind(
                                IsVisibleProperty,
                                nameof(viewModel.NewPasswordError),
                                converter: new IsStringNotNullOrEmptyConverter(),
                                source: viewModel
                            )
                            .Margins(10, 0, 0, 0),
                        new Regular20Label { Text = "Confirm New Password" }.Margins(10, 20, 0, 0),
                        new DualImageBorderedEntry { WidthRequest = 600 }
                            .Bind(
                                DualImageBorderedEntry.EntryTextProperty,
                                nameof(viewModel.ConfirmNewPassword),
                                BindingMode.TwoWay,
                                source: viewModel
                            )
                            .Margins(0, 10, 0, 0),
                        new Regular16Label
                        {
                            TextColor = Colors.Red,
                            HorizontalOptions = LayoutOptions.Start,
                        }
                            .Bind(
                                Label.TextProperty,
                                static (ForgotPasswordViewModel vm) => vm.ConfirmNewPasswordError,
                                source: viewModel
                            )
                            .Bind(
                                IsVisibleProperty,
                                nameof(viewModel.ConfirmNewPasswordError),
                                converter: new IsStringNotNullOrEmptyConverter(),
                                source: viewModel
                            )
                            .Margins(10, 0, 0, 0),
                        new RegularButton
                        {
                            Text = "SUBMIT",
                            WidthRequest = 500,
                            FontSize = 32,
                            FontFamily = "InterBold",
                            HeightRequest = 70,
                        }
                            .BindCommand(nameof(viewModel.ResetPasswordCommand), source: viewModel)
                            .Margins(0, 20, 0, 0),
                        new RegularButton
                        {
                            Text = "CANCEL",
                            WidthRequest = 500,
                            FontSize = 32,
                            FontFamily = "InterBold",
                            HeightRequest = 70,
                        }
                            .BindCommand(
                                nameof(viewModel.CancelPasswordResetCommand),
                                source: viewModel
                            )
                            .Margins(0, 20, 0, 0),
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
