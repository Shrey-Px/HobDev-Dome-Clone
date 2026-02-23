using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace Admin.View;

public class VendorYardView : BaseView
{
    VendorYardViewModel viewModel;

    int count = 0;

    public VendorYardView(VendorYardViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;
            Title = "Partner Yard";

            Padding = new Thickness(20);
            Content = new ScrollView
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                Orientation = ScrollOrientation.Both,
                Content = new Grid
                {
                    RowDefinitions = Rows.Define(
                        (BodyRow.first, 50),
                        (BodyRow.second, 110),
                        (BodyRow.third, 30),
                        (BodyRow.fourth, Auto),
                        (BodyRow.fifth, 30),
                        (BodyRow.sixth, Auto)
                    ),

                    WidthRequest = 1250,
                    RowSpacing = 20,
                    Children =
                    {
                        new RegularButton
                        {
                            HorizontalOptions = LayoutOptions.End,
                            FontFamily = "InterBold",
                            WidthRequest = 186,
                            FontSize = 20,
                            Text = "+ ADD PARTNER",
                        }
                            .BindCommand(
                                static (VendorYardViewModel vm) => vm.AddVendorCommand,
                                source: viewModel
                            )
                            .Row(BodyRow.first)
                            .Margins(0, 0, 155, 0),
                        new CustomBorder
                        {
                            WidthRequest = 1100,
                            HeightRequest = 109,
                            HorizontalOptions = LayoutOptions.Start,
                            Padding = new Thickness(20, 10, 20, 10),
                            Content = new Grid
                            {
                                RowDefinitions = Rows.Define(
                                    (BodyRow.first, 35),
                                    (BodyRow.second, 35)
                                ),
                                ColumnDefinitions = Columns.Define(
                                    (BodyColumn.first, 257),
                                    (BodyColumn.second, 230),
                                    (BodyColumn.third, 450),
                                    (BodyColumn.fourth, 40)
                                ),
                                ColumnSpacing = 20,
                                Children =
                                {
                                    new Regular16Label
                                    {
                                        Text = "Select Sports",
                                        VerticalOptions = LayoutOptions.Center,
                                        HorizontalOptions = LayoutOptions.Start,
                                    }
                                        .Row(BodyRow.first)
                                        .Column(BodyColumn.first),
                                    new Picker { FontSize = 20, FontFamily = "InterRegular" }
                                        .Bind(
                                            Picker.ItemsSourceProperty,
                                            static (VendorYardViewModel vm) => vm.GameNames,
                                            source: viewModel
                                        )
                                        .Bind(
                                            Picker.SelectedItemProperty,
                                            static (VendorYardViewModel vm) => vm.SelectedGame,
                                            source: viewModel
                                        )
                                        .Row(BodyRow.second)
                                        .Column(BodyColumn.first),
                                    new Regular16Label
                                    {
                                        Text = "Select Region",
                                        VerticalOptions = LayoutOptions.Center,
                                        HorizontalOptions = LayoutOptions.Start,
                                    }
                                        .Row(BodyRow.first)
                                        .Column(BodyColumn.second),
                                    new Picker { FontSize = 20, FontFamily = "InterRegular" }
                                        .Bind(
                                            Picker.ItemsSourceProperty,
                                            static (VendorYardViewModel vm) => vm.CityNames,
                                            source: viewModel
                                        )
                                        .Bind(
                                            Picker.SelectedItemProperty,
                                            static (VendorYardViewModel vm) => vm.SelectedCity,
                                            source: viewModel
                                        )
                                        .Row(BodyRow.second)
                                        .Column(BodyColumn.second),
                                    new Regular16Label
                                    {
                                        Text = "Search Partner",
                                        VerticalOptions = LayoutOptions.Center,
                                        HorizontalOptions = LayoutOptions.Start,
                                    }
                                        .Row(BodyRow.first)
                                        .Column(BodyColumn.third),
                                    new Picker
                                    {
                                        FontSize = 20,
                                        FontFamily = "InterRegular",
                                        ItemDisplayBinding = new Binding(nameof(Venue.FullName)),
                                    }
                                        .Bind(
                                            Picker.ItemsSourceProperty,
                                            static (VendorYardViewModel vm) => vm.ActiveVendors,
                                            source: viewModel
                                        )
                                        .Bind(
                                            Picker.SelectedItemProperty,
                                            static (VendorYardViewModel vm) => vm.SelectedVendor,
                                            source: viewModel
                                        )
                                        .Row(BodyRow.second)
                                        .Column(BodyColumn.third),
                                    new ImageButton
                                    {
                                        Source = "cancel.png",
                                        HeightRequest = 40,
                                        WidthRequest = 40,
                                        VerticalOptions = LayoutOptions.Center,
                                    }
                                        .BindCommand(
                                            static (VendorYardViewModel vm) =>
                                                vm.ClearSearchCommand,
                                            source: viewModel
                                        )
                                        .Margins(20, 0, 0, 0)
                                        .Row(BodyRow.first)
                                        .RowSpan(2)
                                        .Column(BodyColumn.fourth),
                                },
                            },
                        }.Row(BodyRow.second),
                        new Bold24Label { Text = "Active Partners" }.Row(BodyRow.third),
                        new CollectionView
                        {
                            // ItemSizingStrategy= ItemSizingStrategy.MeasureFirstItem,
                            ItemTemplate = new DataTemplate(() =>
                            {
                                return new CustomBorder
                                {
                                    Padding = new Thickness(20, 10, 20, 10),
                                    Content = new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, 30),
                                            (BodyRow.second, 30),
                                            (BodyRow.third, 30),
                                            (BodyRow.fourth, 30),
                                            (BodyRow.fifth, 60)
                                        ),
                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, 80),
                                            (BodyColumn.second, 150)
                                        ),
                                        ColumnSpacing = 15,
                                        RowSpacing = 10,

                                        Children =
                                        {
                                            new Bold24Label { }
                                                .Bind(
                                                    Bold24Label.TextProperty,
                                                    static (Venue venue) => venue.FullName
                                                )
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new Regular21Label { }
                                                .Bind(
                                                    Regular21Label.TextProperty,
                                                    static (Venue venue) => venue.Address.City
                                                )
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new HorizontalStackLayout
                                            {
                                                Children =
                                                {
                                                    new CheckBox
                                                    {
                                                        Color = Color.FromArgb("#EF2F50"),
                                                    }
                                                        .Bind(
                                                            CheckBox.IsCheckedProperty,
                                                            static (Venue venue) => venue.IsActive,
                                                            mode: BindingMode.OneWay
                                                        )
                                                        .Invoke(activeCheckBox =>
                                                            activeCheckBox.CheckedChanged +=
                                                                ActiveCheckBox_CheckedChanged
                                                        ),
                                                    new Regular16Label
                                                    {
                                                        Text = "Active",
                                                        VerticalOptions = LayoutOptions.Center,
                                                    }.Margins(5, 0, 0, 0),
                                                },
                                            }
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new HorizontalStackLayout
                                            {
                                                Children =
                                                {
                                                    new CheckBox
                                                    {
                                                        Color = Color.FromArgb("#EF2F50"),
                                                    }
                                                        .Bind(
                                                            CheckBox.IsCheckedProperty,
                                                            static (Venue venue) =>
                                                                venue.IsPromoted,
                                                            mode: BindingMode.OneWay
                                                        )
                                                        .Invoke(promotedCheckBox =>
                                                            promotedCheckBox.CheckedChanged +=
                                                                PromotedCheckBox_CheckedChanged
                                                        ),
                                                    new Regular16Label
                                                    {
                                                        Text = "Promoted",
                                                        VerticalOptions = LayoutOptions.Center,
                                                    }.Margins(5, 0, 0, 0),
                                                },
                                            }
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new RegularButton
                                            {
                                                Text = "EDIT",
                                                Background = Colors.Black,
                                                HeightRequest = 50,
                                                FontSize = 18,
                                                FontFamily = "InterBold",
                                                WidthRequest = 80,
                                                Padding = new Thickness(4),
                                                VerticalOptions = LayoutOptions.Center,
                                                HorizontalOptions = LayoutOptions.Start,
                                            }
                                                .BindCommand(
                                                    nameof(viewModel.EditVendorCommand),
                                                    source: viewModel,
                                                    parameterPath: "."
                                                )
                                                .Row(BodyRow.fifth)
                                                .Column(BodyColumn.first),
                                        },
                                    },
                                };
                            }),
                            ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical)
                            {
                                HorizontalItemSpacing = 20,
                                VerticalItemSpacing = 20,
                            },
                        }
                            .Bind(
                                ItemsView.ItemsSourceProperty,
                                static (VendorYardViewModel vm) => vm.FilteredVendors,
                                source: viewModel
                            )
                            .Row(BodyRow.fourth),
                        new Bold24Label { Text = "InActive Partners" }.Row(BodyRow.fifth),
                        new CollectionView
                        {
                            ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
                            ItemTemplate = new DataTemplate(() =>
                            {
                                return new CustomBorder
                                {
                                    Padding = new Thickness(20, 10, 20, 10),
                                    Content = new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, 30),
                                            (BodyRow.second, 30),
                                            (BodyRow.third, 30),
                                            (BodyRow.fourth, 60)
                                        ),
                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, 80),
                                            (BodyColumn.second, 150),
                                            (BodyColumn.third, 80)
                                        ),
                                        ColumnSpacing = 15,
                                        RowSpacing = 10,

                                        Children =
                                        {
                                            new Bold24Label { }
                                                .Bind(
                                                    Bold24Label.TextProperty,
                                                    static (Venue venue) => venue.FullName
                                                )
                                                .Row(BodyRow.first)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(3),
                                            new Regular21Label { }
                                                .Bind(
                                                    Regular21Label.TextProperty,
                                                    static (Venue venue) => venue.Address.City
                                                )
                                                .Row(BodyRow.second)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2),
                                            new HorizontalStackLayout
                                            {
                                                new CheckBox { Color = Color.FromArgb("#EF2F50") }
                                                    .Bind(
                                                        CheckBox.IsCheckedProperty,
                                                        static (Venue venue) => venue.IsActive,
                                                        mode: BindingMode.OneWay
                                                    )
                                                    .Invoke(activeCheckBox =>
                                                        activeCheckBox.CheckedChanged +=
                                                            ActiveCheckBox_CheckedChanged
                                                    ),
                                                new Regular16Label
                                                {
                                                    Text = "Active:",
                                                    VerticalOptions = LayoutOptions.Center,
                                                    HorizontalOptions = LayoutOptions.Start,
                                                }.Margins(5, 0, 0, 0),
                                                new CheckBox { Color = Color.FromArgb("#EF2F50") }
                                                    .Bind(
                                                        CheckBox.IsCheckedProperty,
                                                        static (Venue venue) => venue.IsPromoted,
                                                        mode: BindingMode.OneWay
                                                    )
                                                    .Invoke(promotedCheckBox =>
                                                        promotedCheckBox.CheckedChanged +=
                                                            PromotedCheckBox_CheckedChanged
                                                    )
                                                    .Margins(20, 0, 0, 0),
                                                new Regular16Label
                                                {
                                                    Text = "Promoted:",
                                                    VerticalOptions = LayoutOptions.Center,
                                                    HorizontalOptions = LayoutOptions.Start,
                                                }.Margins(5, 0, 0, 0),
                                            }
                                                .Row(BodyRow.third)
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(3),
                                            new RegularButton
                                            {
                                                Text = "EDIT",
                                                Background = Colors.Black,
                                                HeightRequest = 50,
                                                FontSize = 18,
                                                FontFamily = "InterBold",
                                                WidthRequest = 80,
                                                Padding = new Thickness(4),
                                                VerticalOptions = LayoutOptions.Center,
                                                HorizontalOptions = LayoutOptions.Start,
                                            }
                                                .BindCommand(
                                                    nameof(viewModel.EditVendorCommand),
                                                    source: viewModel,
                                                    parameterPath: "."
                                                )
                                                .Row(BodyRow.fourth)
                                                .Column(BodyColumn.first),
                                        },
                                    },
                                };
                            }),
                            ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical)
                            {
                                HorizontalItemSpacing = 20,
                                VerticalItemSpacing = 20,
                            },
                        }
                            .Bind(
                                ItemsView.ItemsSourceProperty,
                                static (VendorYardViewModel vm) => vm.DormantVendors,
                                source: viewModel
                            )
                            .Row(BodyRow.sixth),
                    },
                },
            };

            Loaded += VendorYardView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void VendorYardView_Loaded(object? sender, EventArgs e)
    {
        if (count == 0)
        {
            await viewModel.Initialize();
            count++;
        }
    }

    private async void ActiveCheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        bool isChecked = e.Value;
        CheckBox? checkBox = (CheckBox)sender;
        Venue? venue = (Venue)checkBox.BindingContext;
        if (venue != null)
        {
            if (venue.IsActive != isChecked)
            {
                if (isChecked)
                {
                    bool isActionIntended = await Shell.Current.DisplayAlert(
                        "Confirmation",
                        "Are you sure you want to activate this vendor?",
                        "Yes",
                        "No"
                    );
                    if (isActionIntended)
                    {
                        await viewModel.ActivateVendorCommand.ExecuteAsync(venue);
                    }
                    else
                    {
                        checkBox.IsChecked = venue.IsActive;
                    }
                }
                else if (!isChecked)
                {
                    bool isActionIntended = await Shell.Current.DisplayAlert(
                        "Confirmation",
                        "Are you sure you want to deactivate this vendor?",
                        "Yes",
                        "No"
                    );
                    if (isActionIntended)
                    {
                        await viewModel.DeactivateVendorCommand.ExecuteAsync(venue);
                    }
                    else
                    {
                        checkBox.IsChecked = venue.IsActive;
                    }
                }
            }
        }
    }

    private async void PromotedCheckBox_CheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        bool isChecked = e.Value;
        CheckBox? checkBox = (CheckBox)sender;
        Venue? venue = (Venue)checkBox.BindingContext;
        if (venue != null)
        {
            if (venue.IsPromoted != isChecked)
            {
                if (isChecked)
                {
                    bool isActionIntended = await Shell.Current.DisplayAlert(
                        "Confirmation",
                        "Are you sure you want to promote this vendor?",
                        "Yes",
                        "No"
                    );
                    if (isActionIntended)
                    {
                        await viewModel.PromoteVendorCommand.ExecuteAsync(venue);
                    }
                    else
                    {
                        checkBox.IsChecked = venue.IsPromoted;
                    }
                }
                else if (!isChecked)
                {
                    bool isActionIntended = await Shell.Current.DisplayAlert(
                        "Confirmation",
                        "Are you sure you don't want to promote this vendor?",
                        "Yes",
                        "No"
                    );
                    if (isActionIntended)
                    {
                        await viewModel.UnPromoteVendorCommand.ExecuteAsync(venue);
                    }
                    else
                    {
                        checkBox.IsChecked = venue.IsPromoted;
                    }
                }
            }
        }
    }
}
