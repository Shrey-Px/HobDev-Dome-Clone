namespace Admin.View.Account;

public class LoginView : ContentPage
{
    public LoginView(LoginViewModel viewModel)
    {
        try
        {
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
            this.AppThemeBinding(
                ContentPage.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            );
            Title = "Login";
            Content = new ScrollView
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                Orientation = ScrollOrientation.Both,
                Content = new Grid
                {
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Star)),

                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, 350),
                        (BodyColumn.second, Star)
                    ),
                    Children =
                    {
                        new Image
                        {
                            Source = "dx_component.png",
                            HorizontalOptions = LayoutOptions.Fill,
                            Aspect = Aspect.AspectFill,
                        }
                            .Row(BodyRow.first)
                            .Column(BodyColumn.first)
                            .RowSpan(2),
                        new VerticalStackLayout
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = new Thickness(20, 10, 20, 10),
                            Children =
                            {
                                new Regular20Label { Text = "Email Address" }.Margins(10, 20, 0, 0),
                                new SingleImageBorderedEntry { WidthRequest = 600 }
                                    .Bind(
                                        SingleImageBorderedEntry.EntryTextProperty,
                                        nameof(viewModel.Email),
                                        BindingMode.TwoWay,
                                        source: viewModel
                                    )
                                    .AppThemeBinding(
                                        SingleImageBorderedEntry.ImgSourceProperty,
                                        "email_black_24dp.png",
                                        "email_white_24dp.png"
                                    ),
                                new Regular16Label { TextColor = Colors.Red }
                                    .Bind(
                                        Regular16Label.TextProperty,
                                        static (LoginViewModel vm) => vm.EmailError,
                                        source: viewModel
                                    )
                                    .Bind(
                                        IsVisibleProperty,
                                        nameof(viewModel.EmailError),
                                        converter: new IsStringNotNullOrEmptyConverter(),
                                        source: viewModel
                                    )
                                    .Margins(10, 0, 0, 0),
                                new Regular20Label { Text = "Password" }.Margins(10, 20, 0, 0),
                                new DualImageBorderedEntry { WidthRequest = 600 }.Bind(
                                    DualImageBorderedEntry.EntryTextProperty,
                                    nameof(viewModel.Password),
                                    BindingMode.TwoWay,
                                    source: viewModel
                                ),
                                new Regular16Label { TextColor = Colors.Red }
                                    .Bind(
                                        Regular16Label.TextProperty,
                                        static (LoginViewModel vm) => vm.PasswordError,
                                        source: viewModel
                                    )
                                    .Bind(
                                        IsVisibleProperty,
                                        nameof(viewModel.PasswordError),
                                        converter: new IsStringNotNullOrEmptyConverter(),
                                        source: viewModel
                                    )
                                    .Margins(10, 0, 0, 0),
                                new Button
                                {
                                    Text = "Forgot Password?",
                                    FontSize = 20,
                                    FontFamily = "InterRegular",
                                    Background = Colors.Transparent,
                                    BorderColor = Colors.Transparent,
                                    TextColor = Color.FromArgb("#EF2F50"),
                                    HorizontalOptions = LayoutOptions.End,
                                    VerticalOptions = LayoutOptions.Start,
                                }.BindCommand(
                                    nameof(viewModel.ForgotPasswordCommand),
                                    source: viewModel
                                ),
                                new RegularButton
                                {
                                    Text = "LOG IN",
                                    WidthRequest = 500,
                                    FontSize = 32,
                                    FontFamily = "InterBold",
                                    HeightRequest = 70,
                                }
                                    .BindCommand(nameof(viewModel.LoginCommand), source: viewModel)
                                    .Margins(0, 20, 0, 0),
                            },
                        }
                            .Row(BodyRow.first)
                            .Column(BodyColumn.second),
                        new Label
                        {
                            Text = "Daflo Innovations @ 2024",
                            VerticalOptions = LayoutOptions.End,
                            HorizontalOptions = LayoutOptions.End,
                        }
                            .Row(BodyRow.second)
                            .Column(BodyColumn.second)
                            .Margins(0, 0, 10, 10),
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
