using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace Player.Views
{
    public class BaseView : ContentPage
    {
        public Grid BaseContentGrid;
        public Grid TopGrid;

        public BaseView(BaseViewModel viewModel)
        {
            try
            {
                BindingContext = viewModel;

                this.AppThemeBinding(
                    ContentPage.BackgroundProperty,
                    Color.FromArgb("#F1F3F2"),
                    Colors.Black
                );

                TopGrid = new Grid
                {
                    Padding = new Thickness(29, 27, 20, 12),
                    Children =
                    {
                        {
                            new HorizontalStackLayout
                            {
                                HorizontalOptions = LayoutOptions.End,
                                Spacing = 19,
                                Children =
                                {
                                    new Border
                                    {
                                        VerticalOptions = LayoutOptions.Start,
                                        HeightRequest = 35,
                                        WidthRequest = 35,
                                        StrokeShape = new RoundRectangle { CornerRadius = 17.5 },
                                        Padding = 0,
                                        StrokeThickness = 0,
                                        Content = new ImageButton
                                        {
                                            HeightRequest = 22,
                                            WidthRequest = 22,
                                        }
                                            .AppThemeBinding(
                                                ImageButton.SourceProperty,
                                                "notification_top_light_theme",
                                                "notification_top_dark_theme"
                                            )
                                            .Bind(
                                                ImageButton.CommandProperty,
                                                static (BaseViewModel vm) => vm.NotificationCommand,
                                                source: viewModel
                                            ),
                                    }.AppThemeBinding(
                                        Border.BackgroundProperty,
                                        Color.FromArgb("#F1F3F2"),
                                        Colors.Black
                                    ),
                                    new AvatarView
                                    {
                                        BorderWidth = 1,
                                        Padding = 0,
                                        HeightRequest = 35,
                                        WidthRequest = 35,
                                        CornerRadius = 17.5,
                                    }
                                        .AppThemeBinding(
                                            AvatarView.BorderColorProperty,
                                            Colors.Black,
                                            Colors.White
                                        )
                                        .Bind(
                                            AvatarView.ImageSourceProperty,
                                            "PlayerImage",
                                            converter: new ByteArrayToImageSourceConverter(),
                                            targetNullValue: "profile_image_fallback.png"
                                        ),
                                },
                            }.Column(BodyColumn.second)
                        },
                    },
                }
                    .AppThemeBinding(
                        Grid.BackgroundProperty,
                        Colors.White,
                        Color.FromArgb("#23262A")
                    )
                    .Row(BodyRow.first)
                    .ColumnSpan(2);

                Content = BaseContentGrid = new Grid
                {
                    RowDefinitions = Rows.Define((BodyRow.first, Auto), (BodyRow.second, Star)),
                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Auto),
                        (BodyColumn.second, Star)
                    ),
                    RowSpacing = 15,
                    Children = { TopGrid },
                };
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
