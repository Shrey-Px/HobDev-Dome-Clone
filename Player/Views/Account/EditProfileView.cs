namespace Player.Views.Account;

public class EditProfileView : AccountBaseView
{
    public EditProfileView(EditProfileViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            CustomPicker cityPicker = new CustomPicker { }
                .Bind(CustomPicker.PickerItemSourceProperty, static (EditProfileViewModel vm) => vm.Cities, source: viewModel)
                .Bind(
                    CustomPicker.CustomPickerSelectedItemProperty,
                    static (EditProfileViewModel vm) => vm.SelectedCity,
                    source: viewModel,
                    mode: BindingMode.TwoWay
                )
                .Margins(20, 10, 0, 0);
            cityPicker.picker.ItemDisplayBinding = new Binding(nameof(City.CityName));

            CustomPicker provincePicker = new CustomPicker { }
                .Bind(CustomPicker.PickerItemSourceProperty, static (EditProfileViewModel vm) => vm.AllProvinces, source: viewModel)
                .Bind(
                    CustomPicker.CustomPickerSelectedItemProperty,
                    static (EditProfileViewModel vm) => vm.SelectedProvince,
                    source: viewModel,
                    mode: BindingMode.TwoWay
                )
                .Margins(0, 10, 20, 0);
            provincePicker.picker.ItemDisplayBinding = new Binding(nameof(Province.ProvinceName));

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

                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Star),
                        (BodyColumn.second, Star)
                    ),
                    ColumnSpacing = 20,
                    Children =
                    {
                        new AccountTopControl().Row(BodyRow.first).ColumnSpan(2),
                        new ExtraBold28Label { Text = "Edit Profile" }
                            .Margins(20, 20, 0, 0)
                            .Row(BodyRow.second)
                            .ColumnSpan(2),
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
                            .Row(BodyRow.third)
                            .ColumnSpan(2),
                        new BorderedEntry { PlaceholderText = "First Name" }
                            .Bind(BorderedEntry.EntryTextProperty, nameof(viewModel.FirstName), BindingMode.TwoWay, source: viewModel)
                            .Margins(20, 20, 20, 0)
                            .Row(BodyRow.fourth)
                            .ColumnSpan(2),
                        new Regular16Label { TextColor = Colors.Red }
                            .Bind(
                                Regular16Label.TextProperty,
                                nameof(viewModel.FirstNameError),
                                source: viewModel
                            )
                            .Bind(
                                IsVisibleProperty,
                                nameof(viewModel.FirstNameError),
                                converter: new IsStringNotNullOrEmptyConverter(),
                                source: viewModel
                            )
                            .Row(BodyRow.fifth)
                            .ColumnSpan(2)
                            .Margins(20, 0, 20, 0),
                        new BorderedEntry { PlaceholderText = "Last Name" }
                            .Bind(BorderedEntry.EntryTextProperty, nameof(viewModel.LastName), BindingMode.TwoWay, source: viewModel)
                            .Margins(20, 10, 20, 0)
                            .Row(BodyRow.sixth)
                            .ColumnSpan(2),
                        new Regular16Label { TextColor = Colors.Red }
                            .Bind(
                                Regular16Label.TextProperty,
                                nameof(viewModel.LastNameError),
                                source: viewModel
                            )
                            .Bind(
                                IsVisibleProperty,
                                nameof(viewModel.LastNameError),
                                converter: new IsStringNotNullOrEmptyConverter(),
                                source: viewModel
                            )
                            .Row(BodyRow.seventh)
                            .ColumnSpan(2)
                            .Margins(20, 0, 20, 0),
                        cityPicker.Row(BodyRow.eighth).Column(BodyColumn.first),
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
                            .Row(BodyRow.ninth)
                            .Column(BodyColumn.first)
                            .Margins(20, 0, 0, 0),
                        provincePicker.Row(BodyRow.eighth).Column(BodyColumn.second),
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
                            .Row(BodyRow.ninth)
                            .Column(BodyColumn.second)
                            .Margins(0, 0, 20, 0),
                        new Grid
                        {
                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, Star),
                                (BodyColumn.second, Star)
                            ),
                            ColumnSpacing = 20,

                            Children =
                            {
                                new MediumButton { Text = "SAVE" }.BindCommand(
                                    nameof(viewModel.SaveCommand),
                                    source: viewModel
                                ),
                                new MediumButton
                                {
                                    Text = "CANCEL",
                                    Background = Color.FromArgb("#272727"),
                                }
                                    .BindCommand(nameof(viewModel.CancelCommand), source: viewModel)
                                    .Column(BodyColumn.second),
                            },
                        }
                            .Margins(20, 20, 20, 10)
                            .Row(BodyRow.tenth)
                            .ColumnSpan(2),
                    },
                },
            };

            Loaded += EditProfileView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void EditProfileView_Loaded(object? sender, EventArgs e)
    {
        await ((EditProfileViewModel)BindingContext).InitializeAsync();
    }
}
