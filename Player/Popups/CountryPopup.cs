using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;

namespace Player.Popups;

/// <summary>
/// this is a popup that shows a list of countries to select from when entering the phone number.This will be required when the app is published to multiple countries. For now the cananda is the default country in phone number entry
/// </summary>
public class CountryPopup : Popup<Country>
{
    public CountryPopup()
    {
        HeightRequest = 200;
        WidthRequest = 200;
        this.AppThemeBinding(Popup.BackgroundProperty, Colors.White, Colors.Black);
        CollectionView collection = new CollectionView
        {
            SelectionMode = SelectionMode.Single,
            ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem,
            ItemTemplate = new DataTemplate(() =>
            {
                HorizontalStackLayout layout = new HorizontalStackLayout
                {
                    Spacing = 10,
                    Children =
                    {
                        new Label { }.Bind(Label.TextProperty, nameof(Country.FlagImage)),
                        new Regular16Label { HorizontalTextAlignment = TextAlignment.Center }.Bind(
                            Label.TextProperty,
                            nameof(Country.CountryName)
                        ),
                        new Regular16Label { HorizontalTextAlignment = TextAlignment.Center }.Bind(
                            Label.TextProperty,
                            nameof(Country.Code)
                        ),
                    },
                }.Margins(20, 20, 0, 0);
                return layout;
            }),
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 0,
            },
            ItemsSource = new List<Country>
            {
                new Country(countryName: "Canada", code: "+1", flagImage: "????"),
                new Country(countryName: "USA", code: "+1", flagImage: "????"),
                new Country(countryName: "India", code: "+91", flagImage: "????"),
            },
        };
        collection.SelectionChanged += Collection_SelectionChanged;

        Content = collection;
    }

    private async void Collection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            if (e.CurrentSelection.Count == 0)
            {
                return;
            }
            Country? selectedCountry = e.CurrentSelection.FirstOrDefault() as Country;

            await CloseAsync(selectedCountry);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("error", ex.Message, "OK");
        }
    }
}
