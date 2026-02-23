namespace Admin.CustomControls.CollectionViews;

public class AmenitiesControl : CollectionView
{
    public AmenitiesControl()
    {
        DataTemplate amenitiesTemplate = new DataTemplate(() =>
        {
            return new Border
            {
                HeightRequest = 30,
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Padding = new Thickness(10, 0),
                Content = new Bold10Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                }
                    .Bind(Bold10Label.TextProperty, nameof(Amenity.AmenityName))
                    .Bind(
                        Bold10Label.TextColorProperty,
                        nameof(Amenity.IsSelected),
                        convert: (bool state) => state == false ? Colors.Black : Colors.White
                    ),
            }.Bind(
                Border.BackgroundProperty,
                nameof(Amenity.IsSelected),
                convert: (bool state) =>
                    state == false ? Colors.LightGray : Color.FromArgb("#EF2F50")
            );
        });

        SelectionMode = SelectionMode.Multiple;
        ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal) { ItemSpacing = 5 };
        this.SetBinding(ItemsView.ItemsSourceProperty, "AllAmenities", BindingMode.OneWay);
        this.SetBinding(
            SelectableItemsView.SelectedItemsProperty,
            "SelectedAmenities",
            BindingMode.TwoWay
        );

        ItemTemplate = amenitiesTemplate;

        this.SetBinding(SelectionChangedCommandProperty, "AmenitySelectionChangedCommand");
    }
}
