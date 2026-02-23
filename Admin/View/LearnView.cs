namespace Admin.View;

public class LearnView : BaseView
{
    LearnViewModel viewModel;
    int count = 0;

    public LearnView(LearnViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            Title = "Learn";
            this.viewModel = viewModel;
            Padding = new Thickness(20, 40);
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Both,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Always,
                VerticalScrollBarVisibility = ScrollBarVisibility.Always,
                Content = new HorizontalStackLayout
                {
                    Spacing = 20,
                    Children =
                    {
                        new CustomBorder
                        {
                            Content = new Grid
                            {
                                RowDefinitions = Rows.Define(
                                    (BodyRow.first, 20),
                                    (BodyRow.second, 45),
                                    (BodyRow.third, 50),
                                    (BodyRow.fourth, Auto),
                                    (BodyRow.fifth, 35),
                                    (BodyRow.sixth, 50),
                                    (BodyRow.seventh, Auto),
                                    (BodyRow.eighth, 35),
                                    (BodyRow.ninth, 80),
                                    (BodyRow.tenth, Auto),
                                    (BodyRow.eleventh, 35),
                                    (BodyRow.twelfth, 50),
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
                                    new Bold20Label { Text = "LEARN DETAILS" }
                                        .Row(BodyRow.first)
                                        .Column(BodyColumn.first),
                                    new Bold20Label { Text = "Creator Name" }
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
                                            static (LearnViewModel vm) => vm.Content.PublishedBy,
                                            mode: BindingMode.TwoWay,
                                            source: viewModel
                                        )
                                        .Margins(0, 10, 0, 0),
                                    new Regular16Label
                                    {
                                        TextColor = Colors.Red,
                                        Text = "Name is required",
                                    }
                                        .Row(BodyRow.fourth)
                                        .Column(BodyColumn.first)
                                        .Bind(
                                            Regular16Label.IsVisibleProperty,
                                            $"{nameof(viewModel.Content)}.{nameof(viewModel.Content.PublishedBy)}",
                                            converter: new IsStringNullOrWhiteSpaceConverter(),
                                            source: viewModel
                                        ),
                                    new Bold20Label { Text = "Title" }
                                        .Row(BodyRow.fifth)
                                        .Column(BodyColumn.first)
                                        .Margins(0, 10, 0, 0),
                                    new BorderedEntryRectangle
                                    {
                                        PlaceholderText = "Type here",
                                        WidthRequest = 305,
                                    }
                                        .Row(BodyRow.sixth)
                                        .Column(BodyColumn.first)
                                        .Bind(
                                            BorderedEntryRectangle.EntryTextProperty,
                                            static (LearnViewModel vm) => vm.Content.Title,
                                            mode: BindingMode.TwoWay,
                                            source: viewModel
                                        )
                                        .Margins(0, 10, 0, 0),
                                    new Regular16Label
                                    {
                                        TextColor = Colors.Red,
                                        Text = "Title is required",
                                    }
                                        .Row(BodyRow.seventh)
                                        .Column(BodyColumn.first)
                                        .Bind(
                                            Regular16Label.IsVisibleProperty,
                                            $"{nameof(viewModel.Content)}.{nameof(viewModel.Content.Title)}",
                                            converter: new IsStringNullOrWhiteSpaceConverter(),
                                            source: viewModel
                                        ),
                                    new Bold20Label { Text = "Content Type" }
                                        .Row(BodyRow.fifth)
                                        .Column(BodyColumn.second)
                                        .Margins(0, 10, 0, 0),
                                    new Picker { WidthRequest = 305 }
                                        .Row(BodyRow.sixth)
                                        .Column(BodyColumn.second)
                                        .Bind(
                                            Picker.ItemsSourceProperty,
                                            static (LearnViewModel vm) => vm.ContentTypes,
                                            source: viewModel
                                        )
                                        .Bind(
                                            Picker.SelectedItemProperty,
                                            static (LearnViewModel vm) => vm.Content.Type,
                                            mode: BindingMode.TwoWay,
                                            source: viewModel
                                        )
                                        .Margins(0, 10, 0, 0),
                                    new Regular16Label
                                    {
                                        TextColor = Colors.Red,
                                        Text = "Content Type is required",
                                    }
                                        .Row(BodyRow.seventh)
                                        .Column(BodyColumn.second)
                                        .Bind(
                                            Regular16Label.IsVisibleProperty,
                                            $"{nameof(viewModel.Content)}.{nameof(viewModel.Content.Type)}",
                                            converter: new IsStringNullOrWhiteSpaceConverter(),
                                            source: viewModel
                                        ),
                                    new Bold20Label { Text = "Description" }
                                        .Row(BodyRow.eighth)
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
                                            static (LearnViewModel vm) => vm.Content.Description,
                                            mode: BindingMode.TwoWay,
                                            source: viewModel
                                        ),
                                    }
                                        .Row(BodyRow.ninth)
                                        .Column(BodyColumn.first)
                                        .ColumnSpan(2)
                                        .Margins(0, 10, 0, 0),
                                    new Regular16Label
                                    {
                                        TextColor = Colors.Red,
                                        Text = "Description is required",
                                    }
                                        .Row(BodyRow.tenth)
                                        .Column(BodyColumn.first)
                                        .Bind(
                                            Regular16Label.IsVisibleProperty,
                                            $"{nameof(viewModel.Content)}.{nameof(viewModel.Content.Description)}",
                                            converter: new IsStringNullOrWhiteSpaceConverter(),
                                            source: viewModel
                                        ),
                                    new Bold20Label { Text = "Content Link" }
                                        .Row(BodyRow.eleventh)
                                        .Column(BodyColumn.first)
                                        .Margins(0, 10, 0, 0),
                                    new BorderedEntryRectangle
                                    {
                                        PlaceholderText = "Type here",
                                        WidthRequest = 610,
                                    }
                                        .Row(BodyRow.twelfth)
                                        .Column(BodyColumn.first)
                                        .ColumnSpan(2)
                                        .Bind(
                                            BorderedEntryRectangle.EntryTextProperty,
                                            static (LearnViewModel vm) => vm.Content.Content,
                                            mode: BindingMode.TwoWay,
                                            source: viewModel
                                        )
                                        .Margins(0, 10, 0, 0),
                                    new Regular16Label
                                    {
                                        TextColor = Colors.Red,
                                        Text = "Content link is required",
                                    }
                                        .Row(BodyRow.thirteenth)
                                        .Column(BodyColumn.first)
                                        .Bind(
                                            Regular16Label.IsVisibleProperty,
                                            $"{nameof(viewModel.Content)}.{nameof(viewModel.Content.Content)}",
                                            converter: new IsStringNullOrWhiteSpaceConverter(),
                                            source: viewModel
                                        ),
                                    new Bold20Label { Text = "Category" }
                                        .Row(BodyRow.fourteenth)
                                        .Column(BodyColumn.first)
                                        .ColumnSpan(2)
                                        .Margins(0, 15, 0, 0),
                                    new Picker
                                    {
                                        WidthRequest = 610,
                                        ItemDisplayBinding = new Binding(nameof(Game.GameName)),
                                    }
                                        .Row(BodyRow.fifteenth)
                                        .Column(BodyColumn.first)
                                        .ColumnSpan(2)
                                        .Bind(
                                            Picker.ItemsSourceProperty,
                                            static (LearnViewModel vm) => vm.AllGames,
                                            source: viewModel
                                        )
                                        .Bind(
                                            Picker.SelectedItemProperty,
                                            static (LearnViewModel vm) => vm.SelectedGame,
                                            mode: BindingMode.TwoWay,
                                            source: viewModel
                                        )
                                        .Margins(0, 10, 0, 0),
                                    new Regular16Label
                                    {
                                        TextColor = Colors.Red,
                                        Text = "Category is required",
                                    }
                                        .Row(BodyRow.sixteenth)
                                        .Column(BodyColumn.first)
                                        .ColumnSpan(2)
                                        .Bind(
                                            Regular16Label.IsVisibleProperty,
                                            nameof(viewModel.SelectedGame),
                                            convert: (Game? game) => game == null ? true : false
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
                                        Command = viewModel.UploadLearningContentCommand,
                                    }
                                        .Row(BodyRow.seventeenth)
                                        .Column(BodyColumn.second)
                                        .Margins(20, 20),
                                },
                            },
                        },
                        new CustomBorder
                        {
                            Padding = 20,
                            Content = new VerticalStackLayout
                            {
                                WidthRequest = 500,

                                Spacing = 10,
                                Children =
                                {
                                    new Picker
                                    {
                                        WidthRequest = 355,
                                        FontSize = 32,
                                        FontFamily = "InterRegular",
                                    }
                                        .Bind(
                                            Picker.ItemsSourceProperty,
                                            static (LearnViewModel vm) => vm.ContentTypes,
                                            source: viewModel
                                        )
                                        .Bind(
                                            Picker.SelectedItemProperty,
                                            static (LearnViewModel vm) => vm.SelectedContentType,
                                            mode: BindingMode.TwoWay,
                                            source: viewModel
                                        ),
                                    new CollectionView
                                    {
                                        ItemsLayout = new GridItemsLayout(
                                            2,
                                            ItemsLayoutOrientation.Vertical
                                        )
                                        {
                                            HorizontalItemSpacing = 10,
                                            VerticalItemSpacing = 10,
                                        },
                                        ItemTemplate = new DataTemplate(() =>
                                        {
                                            return new VerticalStackLayout
                                            {
                                                Padding = 10,

                                                Children =
                                                {
                                                    new Bold12Label { Text = "Creator" },
                                                    new Regular14Label { }.Bind(
                                                        Regular14Label.TextProperty,
                                                        static (LearningContent content) => content.PublishedBy
                                                    ),
                                                    new Bold12Label
                                                    {
                                                        Text = "Content Type",
                                                    }.Margins(0, 10, 0, 0),
                                                    new Regular14Label { }.Bind(
                                                        Regular14Label.TextProperty,
                                                        static (LearningContent content) => content.GameCategory
                                                    ),
                                                    new Bold12Label { Text = "Title" }.Margins(
                                                        0,
                                                        10,
                                                        0,
                                                        0
                                                    ),
                                                    new Regular14Label { }.Bind(
                                                        Regular14Label.TextProperty,
                                                        static (LearningContent content) => content.Title
                                                    ),
                                                    new Bold12Label
                                                    {
                                                        Text = "Description",
                                                    }.Margins(0, 10, 0, 0),
                                                    new Regular14Label { }.Bind(
                                                        Regular14Label.TextProperty,
                                                        static (LearningContent content) => content.Description
                                                    ),
                                                    new RegularButton
                                                    {
                                                        Text = "DELETE",
                                                        HorizontalOptions = LayoutOptions.Center,
                                                        FontSize = 14,
                                                        WidthRequest = 100,
                                                    }
                                                        .BindCommand(
                                                            nameof(
                                                                viewModel.DeleteLearningContentCommand
                                                            ),
                                                            source: viewModel
                                                        )
                                                        .Margins(0, 15, 0, 0),
                                                },
                                            }.AppThemeBinding(
                                                Border.BackgroundProperty,
                                                Color.FromArgb("#FAFAFA"),
                                                Color.FromArgb("#23262A")
                                            );
                                        }),
                                    }.Bind(
                                        ItemsView.ItemsSourceProperty,
                                        static (LearnViewModel vm) => vm.FilteredLearningContents,
                                        source: viewModel
                                    ),
                                },
                            },
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
