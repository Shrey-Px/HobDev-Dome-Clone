namespace Admin.View;

public class CoachView : BaseView
{
    CoachViewModel viewModel;
    int count = 0;

    public CoachView(CoachViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            Title = "Coach";
            this.viewModel = viewModel;
            Padding = new Thickness(20, 40);
            Content = new ScrollView
            {
                WidthRequest = 750,
                HorizontalOptions = LayoutOptions.Start,
                Orientation = ScrollOrientation.Both,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                Content = new CustomBorder
                {
                    Content = new Grid
                    {
                        RowDefinitions = Rows.Define(
                            (BodyRow.first, 20),
                            (BodyRow.second, 45),
                            (BodyRow.third, 50),
                            (BodyRow.fourth, Auto),
                            (BodyRow.fifth, 40),
                            (BodyRow.sixth, 50),
                            (BodyRow.seventh, Auto),
                            (BodyRow.eighth, 35),
                            (BodyRow.ninth, 50),
                            (BodyRow.tenth, Auto),
                            (BodyRow.eleventh, 35),
                            (BodyRow.twelfth, 80),
                            (BodyRow.thirteenth, Auto),
                            (BodyRow.fourteenth, 40),
                            (BodyRow.fifteenth, 50),
                            (BodyRow.sixteenth, Auto),
                            (BodyRow.seventeenth, 70)
                        ),
                        ColumnDefinitions = Columns.Define(
                            (BodyColumn.first, 310),
                            (BodyColumn.second, 310)
                        ),
                        Padding = 20,

                        ColumnSpacing = 10,
                        Children =
                        {
                            new Bold20Label { Text = "COACH DETAILS" }
                                .Row(BodyRow.first)
                                .Column(BodyColumn.first),
                            new Bold20Label { Text = "Coach Name" }
                                .Row(BodyRow.second)
                                .Column(BodyColumn.first)
                                .Margins(0, 20, 0, 0),
                            new BorderedEntryRectangle
                            {
                                PlaceholderText = "Type here",
                                WidthRequest = 610,
                            }
                                .Row(BodyRow.third)
                                .Column(BodyColumn.first)
                                .ColumnSpan(2)
                                .Bind(
                                    BorderedEntryRectangle.EntryTextProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.CoachName)}",
                                    mode: BindingMode.TwoWay
                                )
                                .Margins(0, 10, 0, 0),
                            new Regular16Label
                            {
                                TextColor = Colors.Red,
                                Text = "Coach name is required",
                            }
                                .Row(BodyRow.fourth)
                                .Column(BodyColumn.first)
                                .Bind(
                                    Regular16Label.IsVisibleProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.CoachName)}",
                                    converter: new IsStringNullOrWhiteSpaceConverter()
                                ),
                            new Bold20Label { Text = "Sport Type" }
                                .Row(BodyRow.fifth)
                                .Column(BodyColumn.first)
                                .Margins(0, 10, 0, 0),
                            new Picker { WidthRequest = 305 }
                                .Row(BodyRow.sixth)
                                .Column(BodyColumn.first)
                                .Bind(Picker.ItemsSourceProperty, static (CoachViewModel vm) => vm.AllGames, source: viewModel)
                                .Bind(
                                    Picker.SelectedItemProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.GameCategory)}",
                                    mode: BindingMode.TwoWay
                                )
                                .Margins(0, 10, 0, 0),
                            new Regular16Label
                            {
                                TextColor = Colors.Red,
                                Text = "Sport Type is required",
                            }
                                .Row(BodyRow.seventh)
                                .Column(BodyColumn.first)
                                .Bind(
                                    Regular16Label.IsVisibleProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.GameCategory)}",
                                    converter: new IsStringNullOrWhiteSpaceConverter()
                                ),
                            new Bold20Label { Text = "Competancy" }
                                .Row(BodyRow.fifth)
                                .Column(BodyColumn.second)
                                .Margins(0, 15, 0, 0),
                            new Picker { WidthRequest = 305 }
                                .Row(BodyRow.sixth)
                                .Column(BodyColumn.second)
                                .Bind(Picker.ItemsSourceProperty, static (CoachViewModel vm) => vm.CompetancyTypes, source: viewModel)
                                .Bind(
                                    Picker.SelectedItemProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.Competancy)}",
                                    mode: BindingMode.TwoWay
                                )
                                .Margins(0, 10, 0, 0),
                            new Regular16Label
                            {
                                TextColor = Colors.Red,
                                Text = "Competancy is required",
                            }
                                .Row(BodyRow.seventh)
                                .Column(BodyColumn.second)
                                .Bind(
                                    Regular16Label.IsVisibleProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.Competancy)}",
                                    converter: new IsStringNullOrWhiteSpaceConverter()
                                ),
                            new Bold20Label { Text = "Venue Name" }
                                .Row(BodyRow.eighth)
                                .Column(BodyColumn.first)
                                .Margins(0, 10, 0, 0),
                            new Picker 
                            { 
                                WidthRequest = 610,
                                ItemDisplayBinding = new Binding(nameof(Venue.FullName))
                            }
                                .Row(BodyRow.ninth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(2)
                                .Bind(Picker.ItemsSourceProperty, static (CoachViewModel vm) => vm.AllVenues, source: viewModel)
                                .Bind(
                                    Picker.SelectedItemProperty,
                                    nameof(viewModel.SelectedVenue),
                                    mode: BindingMode.TwoWay
                                )
                                .Margins(0, 10, 0, 0),
                            new Regular16Label
                            {
                                TextColor = Colors.Red,
                                Text = "Venue name is required",
                            }
                                .Row(BodyRow.tenth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(2)
                                .Bind(
                                    Regular16Label.IsVisibleProperty,
                                    nameof(viewModel.SelectedVenue),
                                    converter: new IsNullConverter()
                                ),
                            new Bold20Label { Text = "Description" }
                                .Row(BodyRow.eleventh)
                                .Column(BodyColumn.first)
                                .Margins(0, 10, 0, 0),
                            new CustomBorder
                            {
                                Padding = 0,
                                Content = new Editor
                                {
                                    Placeholder = "Type here",
                                    AutoSize = EditorAutoSizeOption.TextChanges,
                                    MaxLength = 300,
                                    FontSize = 16,
                                    FontFamily = "InterRegular",
                                }.Bind(
                                    Editor.TextProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.CoachDescription)}",
                                    mode: BindingMode.TwoWay
                                ),
                            }
                                .Row(BodyRow.twelfth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(2)
                                .Margins(0, 10, 0, 0),
                            new Regular16Label
                            {
                                TextColor = Colors.Red,
                                Text = "Description is required",
                            }
                                .Row(BodyRow.thirteenth)
                                .Column(BodyColumn.first)
                                .ColumnSpan(2)
                                .Bind(
                                    Regular16Label.IsVisibleProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.CoachDescription)}",
                                    converter: new IsStringNullOrWhiteSpaceConverter()
                                ),
                            new Bold20Label { Text = "Contact Number" }
                                .Row(BodyRow.fourteenth)
                                .Column(BodyColumn.first)
                                .Margins(0, 10, 0, 0),
                            new BorderedEntryRectangle
                            {
                                PlaceholderText = "Type here",
                                WidthRequest = 305,
                            }
                                .Row(BodyRow.fifteenth)
                                .Column(BodyColumn.first)
                                .Bind(
                                    BorderedEntryRectangle.EntryTextProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.ContactNumber)}",
                                    mode: BindingMode.TwoWay
                                )
                                .Margins(0, 10, 0, 0),
                            new Regular16Label
                            {
                                TextColor = Colors.Red,
                                Text = "Contact number is required",
                            }
                                .Row(BodyRow.sixteenth)
                                .Column(BodyColumn.first)
                                .Bind(
                                    Regular16Label.IsVisibleProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.ContactNumber)}",
                                    converter: new IsStringNullOrWhiteSpaceConverter()
                                ),
                            new Bold20Label { Text = "Email ID" }
                                .Row(BodyRow.fourteenth)
                                .Column(BodyColumn.second)
                                .Margins(0, 10, 0, 0),
                            new BorderedEntryRectangle
                            {
                                PlaceholderText = "Type here",
                                WidthRequest = 305,
                            }
                                .Row(BodyRow.fifteenth)
                                .Column(BodyColumn.second)
                                .Bind(
                                    BorderedEntryRectangle.EntryTextProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.Email)}",
                                    mode: BindingMode.TwoWay
                                )
                                .Margins(0, 10, 0, 0),
                            new Regular16Label
                            {
                                TextColor = Colors.Red,
                                Text = "Email is required",
                            }
                                .Row(BodyRow.sixteenth)
                                .Column(BodyColumn.second)
                                .Bind(
                                    Regular16Label.IsVisibleProperty,
                                    $"{nameof(viewModel.CoachData)}.{nameof(viewModel.CoachData.Email)}",
                                    converter: new IsStringNullOrWhiteSpaceConverter()
                                ),
                            new RegularButton
                            {
                                Text = "CANCEL",
                                FontFamily = "InterBold",
                                Background = Colors.Black,
                                Command = viewModel.CancelCommand,
                            }
                                .Row(BodyRow.seventeenth)
                                .Column(BodyColumn.first)
                                .Margins(20, 20),
                            new RegularButton
                            {
                                Text = "UPLOAD",
                                FontFamily = "InterBold",
                                Command = viewModel.UploadCoachCommand,
                            }
                                .Row(BodyRow.seventeenth)
                                .Column(BodyColumn.second)
                                .Margins(20, 20),
                        },
                    },
                },
            };

            Loaded += LearnView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void LearnView_Loaded(object? sender, EventArgs e)
    {
        if (count == 0)
        {
            await viewModel.OnLoaded();
            count++;
        }
    }
}
