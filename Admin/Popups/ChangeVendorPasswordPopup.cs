using System.Threading.Tasks;

namespace Admin.Popups
{
    public class ChangeVendorPasswordPopup : Popup
    {
        public ChangeVendorPasswordPopup(string email)
        {
            Background = Color.FromArgb("#F3F4F6");
            // initiating view model inside constructor because we can't pass view model as parameter from calling page
            ChangeVendorPasswordViewModel? viewModel =
                Shell.Current.Handler?.MauiContext?.Services.GetService<ChangeVendorPasswordViewModel>();

            if (viewModel != null)
            {
                viewModel.Email = email;
            }
            Content = new Grid
            {
                Padding = 30,
                Background = Colors.White,
                RowDefinitions = Rows.Define(
                    (BodyRow.first, 30),
                    (BodyRow.second, 25),
                    (BodyRow.third, 70),
                    (BodyRow.fourth, 25),
                    (BodyRow.fifth, 65)
                ),

                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, Stars(1)),
                    (BodyColumn.second, Stars(1))
                ),
                RowSpacing = 10,
                ColumnSpacing = 20,
                Children =
                {
                    new Bold20Label { Text = "Change Password", TextColor = Colors.Black }
                        .Row(BodyRow.first)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                    new Regular18Label { TextColor = Colors.Black }
                        .Bind(
                            Regular18Label.TextProperty,
                            nameof(viewModel.Email),
                            stringFormat: "Vendor Username: {0}"
                        )
                        .Row(BodyRow.second)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                    new DualImageBorderedEntry
                    {
                        PlaceholderText = "New Password",
                        WidthRequest = 350,
                    }
                        .Bind(DualImageBorderedEntry.EntryTextProperty, nameof(viewModel.Password))
                        .Margins(0, 10, 0, 0)
                        .Row(BodyRow.third)
                        .Column(BodyColumn.first),
                    new Label { TextColor = Colors.Red, LineBreakMode = LineBreakMode.WordWrap }
                        .Bind(
                            Label.TextProperty,
                            nameof(viewModel.PasswordError),
                            source: viewModel
                        )
                        .Bind(
                            Label.IsVisibleProperty,
                            nameof(viewModel.PasswordError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.fourth)
                        .Column(BodyColumn.first),
                    new DualImageBorderedEntry
                    {
                        PlaceholderText = "Confirm new Password",
                        WidthRequest = 350,
                    }
                        .Bind(
                            DualImageBorderedEntry.EntryTextProperty,
                            nameof(viewModel.ConfirmPassword)
                        )
                        .Margins(0, 10, 0, 0)
                        .Row(BodyRow.third)
                        .Column(BodyColumn.second),
                    new Label { TextColor = Colors.Red, LineBreakMode = LineBreakMode.WordWrap }
                        .Bind(
                            Label.TextProperty,
                            nameof(viewModel.ConfirmPasswordError),
                            source: viewModel
                        )
                        .Bind(
                            Label.IsVisibleProperty,
                            nameof(viewModel.ConfirmPasswordError),
                            converter: new IsStringNotNullOrEmptyConverter(),
                            source: viewModel
                        )
                        .Row(BodyRow.fourth)
                        .Column(BodyColumn.second),
                    new RegularButton
                    {
                        Text = "CANCEL",
                        WidthRequest = 200,
                        Background = Colors.Black,
                        HorizontalOptions = LayoutOptions.End,
                    }
                        .Invoke(button => button.Clicked += Button_Clicked)
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.fifth)
                        .Column(BodyColumn.first),
                    new RegularButton
                    {
                        Text = "CONFIRM",
                        WidthRequest = 200,
                        HorizontalOptions = LayoutOptions.Start,
                    }
                        .BindCommand(nameof(viewModel.ResetPasswordCommand))
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.fifth)
                        .Column(BodyColumn.second),
                },
            };

            BindingContext = viewModel;
        }

        private async void Button_Clicked(object? sender, EventArgs e)
        {
            await CloseAsync();
        }
    }
}
