using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;

namespace Player.Popups;

public class HoursPopup : Popup<VenueDate>
{
    public HoursPopup(ObservableCollection<VenueDate> venueDates)
    {
        this.Background = Colors.White;

        VerticalStackLayout stackLayout = new VerticalStackLayout { Spacing = 10 };
        BindableLayout.SetItemsSource(stackLayout, venueDates);

        DataTemplate dateTemplate = new DataTemplate(() =>
        {
            return new Button
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Colors.Black,
                FontSize = 15,
                Padding = 0,
                Background = Colors.Transparent,
                FontFamily = "InterMedium",
            }
                .Bind(Button.TextProperty, nameof(VenueDate.Date), stringFormat: "{0:HH:mm}")
                .Invoke(button => button.Clicked += Button_Clicked);
        });

        BindableLayout.SetItemTemplate(stackLayout, dateTemplate);

        Content = new ScrollView
        {
            HeightRequest = 400,
            WidthRequest = 100,
            Padding = new Thickness(20, 20),
            VerticalScrollBarVisibility = ScrollBarVisibility.Always,
            Content = stackLayout,
        };
    }

    private async void Button_Clicked(object? sender, EventArgs e)
    {
        Button? button = (Button?)sender;
        VenueDate? selectedDate = (VenueDate?)button?.BindingContext;
        await CloseAsync(selectedDate);
    }
}
