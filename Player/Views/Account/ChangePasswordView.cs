namespace Player.Views.Account;

public class ChangePasswordView : AccountBaseView
{
    public ChangePasswordView(ChangePasswordViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            Content = new ScrollView
            {
                Content = new Grid
                {
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Auto)),
                    Children =
                    {
                        new AccountTopControl().Row(BodyRow.first),
                        new VerticalStackLayout
                        {
                            Margin = new Thickness(20, 10, 20, 10),
                            Children =
                            {
                                new ExtraBold28Label { Text = "Change Password" },
                                new DualImageBorderedEntry { PlaceholderText = "New Password" }
                                    .Bind(
                                        DualImageBorderedEntry.EntryTextProperty,
                                        nameof(viewModel.NewPassword),
                                        BindingMode.TwoWay,
                                        source: viewModel
                                    )
                                    .Margins(0, 20, 0, 0),
                                new Label { TextColor = Colors.Red }
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
                                    .Margins(0, 20, 0, 0),
                                new Label { TextColor = Colors.Red }
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
                                new MediumButton { Text = "SUBMIT", WidthRequest = 200 }
                                    .BindCommand(
                                        nameof(viewModel.ResetPasswordCommand),
                                        source: viewModel
                                    )
                                    .Margins(0, 20, 0, 0),
                                new MediumButton
                                {
                                    Text = "CANCEL",
                                    WidthRequest = 200,
                                    Background = Color.FromArgb("#272727"),
                                }
                                    .BindCommand(
                                        nameof(viewModel.CancelPasswordResetCommand),
                                        source: viewModel
                                    )
                                    .Margins(0, 20, 0, 0),
                            },
                        }.Row(BodyRow.second),
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
