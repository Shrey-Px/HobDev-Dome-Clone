namespace Admin.View.Account;

public class ChangePasswordView : ContentPage
{
    public ChangePasswordView(ChangePasswordViewModel viewModel)
    {
        try
        {
            Title = "Change Password";
            this.AppThemeBinding(
                ContentPage.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            );

            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Padding = 20,

                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new Regular20Label { Text = "New password" }.Margins(10, 20, 0, 0),
                    new DualImageBorderedEntry { WidthRequest = 500, HeightRequest = 70 }
                        .Bind(DualImageBorderedEntry.EntryTextProperty, nameof(viewModel.Password), BindingMode.TwoWay, source: viewModel)
                        .Margins(0, 0, 0, 0)
                        .Row(BodyRow.second),
                    new Regular16Label
                    {
                        TextColor = Colors.Red,
                        LineBreakMode = LineBreakMode.WordWrap,
                    }
                        .Bind(
                            Regular16Label.TextProperty,
                            static (ChangePasswordViewModel vm) => vm.PasswordError,
                            source: viewModel
                        )
                        .Bind(
                            Label.IsVisibleProperty,
                            nameof(viewModel.PasswordError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.third)
                        .Margins(10, 0, 0, 0),
                    new Regular20Label { Text = "Confirm new password" }.Margins(10, 20, 0, 0),
                    new DualImageBorderedEntry { WidthRequest = 500, HeightRequest = 70 }
                        .Bind(
                            DualImageBorderedEntry.EntryTextProperty,
                            nameof(viewModel.ConfirmPassword),
                            BindingMode.TwoWay,
                            source: viewModel
                        )
                        .Margins(0, 0, 0, 0)
                        .Row(BodyRow.fourth),
                    new Regular16Label
                    {
                        TextColor = Colors.Red,
                        LineBreakMode = LineBreakMode.WordWrap,
                    }
                        .Bind(
                            Regular16Label.TextProperty,
                            static (ChangePasswordViewModel vm) => vm.ConfirmPasswordError,
                            source: viewModel
                        )
                        .Bind(
                            Label.IsVisibleProperty,
                            nameof(viewModel.ConfirmPasswordError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.fifth)
                        .Margins(10, 0, 0, 0),
                    new RegularButton
                    {
                        Text = "CHANGE PASSWORD",
                        WidthRequest = 300,
                        HeightRequest = 50,
                        FontSize = 25,
                    }
                        .BindCommand(static (ChangePasswordViewModel vm) => vm.ResetPasswordCommand, source: viewModel)
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.sixth),
                    new RegularButton
                    {
                        Text = "CANCEL",
                        WidthRequest = 300,
                        HeightRequest = 50,
                        FontSize = 25,
                        Background = Colors.Black,
                    }
                        .BindCommand(static (ChangePasswordViewModel vm) => vm.CancelCommand, source: viewModel)
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.seventh),
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
