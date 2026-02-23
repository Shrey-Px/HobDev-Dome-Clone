using Microsoft.Maui.Graphics.Text;
using Player.Models.Chat;

namespace Player.Views.Connect;

public class ChatView : BaseView
{
    ChatViewModel? viewModel;

    Editor messageEditor = new Editor
    {
        TextColor = Colors.Black,
        Placeholder = "Type a message",
        PlaceholderColor = Color.FromArgb("#4E4E4E"),
        FontFamily = "InterRegular",
        FontSize = 14,
        MaxLength = 100,
        Background = Colors.Transparent,
    };

    CollectionView? messagesView;

    public ChatView(ChatViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;
            // Hide the keyboard when tapped outside of an Entry or Editor. if this is enabled it causes the collapse of Keyboard when the user taps on the Send button instead of sending the message
            //  HideSoftInputOnTapped=true;
            messageEditor.Bind(Editor.TextProperty, nameof(viewModel.Text));
            messagesView = new CollectionView
            {
                ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepLastItemInView,

                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 10,
                    SnapPointsType = SnapPointsType.MandatorySingle,
                    SnapPointsAlignment = SnapPointsAlignment.End,
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    return new Grid
                    {
                        RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Auto)),
                        Children =
                        {
                            new Regular10Label { HorizontalOptions = LayoutOptions.Center }
                                .Bind(
                                    Regular10Label.TextProperty,
                                    nameof(LoadMessagesResponse.DateSend)
                                )
                                .Row(BodyRow.first),
                            new Border
                            {
                                Margin = new Thickness(0, 10, 0, 0),
                                MaximumWidthRequest = 300,
                                StrokeThickness = 0,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(7),
                                },
                                Padding = new Thickness(10),
                                Content = new VerticalStackLayout
                                {
                                    Spacing = 7,

                                    Children =
                                    {
                                        new Regular12Label
                                        {
                                            TextColor = Color.FromArgb("#EF2F50"),
                                        }.Bind(
                                            Label.TextProperty,
                                            nameof(LoadMessagesResponse.AuthorName)
                                        ),
                                        new Regular14Label { }.Bind(
                                            Label.TextProperty,
                                            nameof(LoadMessagesResponse.Body)
                                        ),
                                        new Regular10Label
                                        {
                                            HorizontalOptions = LayoutOptions.End,
                                        }.Bind(
                                            Regular10Label.TextProperty,
                                            nameof(LoadMessagesResponse.TimeSend)
                                        ),
                                    },
                                },
                            }
                                .Bind(
                                    Border.HorizontalOptionsProperty,
                                    nameof(LoadMessagesResponse.IsMine),
                                    convert: (bool mine) =>
                                        mine == true ? LayoutOptions.End : LayoutOptions.Start
                                )
                                .AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                )
                                .Row(BodyRow.second),
                        },
                    };
                }),
            }
                .Row(BodyRow.fourth)
                .Margins(16, 10, 16, 10)
                .Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.Messages));

            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }
                    .AppThemeBinding(
                        ImageButton.SourceProperty,
                        "back_button_light_theme",
                        "back_button_dark_theme"
                    )
                    .BindCommand(nameof(viewModel.NavigateBackCommand))
            );

            base.BaseContentGrid.Children.Add(
                new ScrollView
                {
                    Content = new Grid
                    {
                        RowDefinitions = Rows.Define(
                            (BodyRow.first, Auto),
                            (BodyRow.second, Auto),
                            (BodyRow.third, Auto),
                            (BodyRow.fourth, Star),
                            (BodyRow.fifth, Auto),
                            (BodyRow.sixth, Auto)
                        ),

                        Children =
                        {
                            new Grid
                            {
                                ColumnDefinitions = Columns.Define(
                                    (BodyColumn.first, Star),
                                    (BodyColumn.second, Auto)
                                ),
                                RowDefinitions = Rows.Define(
                                    (BodyRow.first, Auto),
                                    (BodyRow.second, Auto)
                                ),
                                ColumnSpacing = 10,
                                Children =
                                {
                                    new Regular14Label { }
                                        .Bind(Label.TextProperty, nameof(viewModel.VenueName))
                                        .Row(BodyRow.first),
                                    new Regular14Label { }
                                        .Bind(
                                            Label.TextProperty,
                                            nameof(viewModel.BookingInformation)
                                        )
                                        .Row(BodyRow.second),
                                    new ImageButton
                                    {
                                        HorizontalOptions = LayoutOptions.End,
                                        VerticalOptions = LayoutOptions.Center,
                                    }
                                        .BindCommand(nameof(viewModel.ReviewCommand))
                                        .Bind(
                                            ImageButton.IsVisibleProperty,
                                            nameof(viewModel.IsHost)
                                        )
                                        .AppThemeBinding(
                                            ImageButton.SourceProperty,
                                            "review_requests_black.png",
                                            "review_requests_white.png"
                                        )
                                        .Row(BodyRow.first)
                                        .RowSpan(2)
                                        .Column(BodyColumn.second),
                                    new MediumButton
                                    {
                                        Text = "Join",
                                        HorizontalOptions = LayoutOptions.End,
                                        VerticalOptions = LayoutOptions.Center,
                                    }
                                        .BindCommand(nameof(viewModel.JoinGameCommand))
                                        .Bind(
                                            MediumButton.IsVisibleProperty,
                                            nameof(viewModel.DisplayJoinButton)
                                        )
                                        .Row(BodyRow.first)
                                        .RowSpan(2)
                                        .Column(BodyColumn.second),
                                },
                            }
                                .Row(BodyRow.first)
                                .Margins(16, 0, 16, 10),
                            new Rectangle
                            {
                                HeightRequest = 2,
                                Background = Color.FromArgb("4E4E4E"),
                            }.Row(BodyRow.second),
                            new Border
                            {
                                StrokeThickness = 0,
                                StrokeShape = new RoundRectangle
                                {
                                    CornerRadius = new CornerRadius(7),
                                },
                                Padding = new Thickness(10),
                                Content = new Regular10Label
                                {
                                    Text =
                                        "This is a temporary group that will be automatically deleted 7 days after the game concludes.",
                                },
                            }
                                .AppThemeBinding(
                                    Border.BackgroundProperty,
                                    Colors.White,
                                    Color.FromArgb("#23262A")
                                )
                                .Row(BodyRow.third)
                                .Margins(16, 10, 16, 0),
                            messagesView,
                            new Rectangle
                            {
                                HeightRequest = 2,
                                Background = Color.FromArgb("4E4E4E"),
                                VerticalOptions = LayoutOptions.End,
                            }.Row(BodyRow.fifth),
                            new Grid
                            {
                                Background = Colors.White,

                                Padding = new Thickness(10),
                                VerticalOptions = LayoutOptions.End,
                                ColumnDefinitions = Columns.Define(
                                    (BodyColumn.first, Star),
                                    (BodyColumn.second, 50)
                                ),

                                ColumnSpacing = 10,
                                Children =
                                {
                                    new Border
                                    {
                                        StrokeThickness = 0,
                                        HeightRequest = 45,
                                        StrokeShape = new RoundRectangle
                                        {
                                            CornerRadius = new CornerRadius(7),
                                        },
                                        Padding = new Thickness(0),
                                        Content = messageEditor,
                                    },
                                    new ImageButton
                                    {
                                        Source = "send.png",
                                        HeightRequest = 30,
                                        WidthRequest = 30,
                                    }
                                        .Invoke(btn => btn.Clicked += SendButtonPressed)
                                        .Column(BodyColumn.second)
                                        .Margins(0, 0, 0, 0),
                                },
                            }.Row(BodyRow.sixth),
                        },
                    },
                }
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            Loaded += async (sender, e) => await viewModel.LoadMessages();
            Unloaded += async (sender, e) => await viewModel.Unload();
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    private async void SendButtonPressed(object? sender, EventArgs e)
    {
        if (sender is ImageButton btn)
        {
            await Task.Delay(500);

            messageEditor.Focus();
        }
        await viewModel.SendMessageCommand.ExecuteAsync(null);
    }
}
