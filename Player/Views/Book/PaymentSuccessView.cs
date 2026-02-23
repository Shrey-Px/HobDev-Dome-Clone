namespace Player.Views.Book;

public class PaymentSuccessView : ContentPage
{
    public PaymentSuccessView(PaymentSuccessViewModel viewModel)
    {
        try
        {
            this.AppThemeBinding(
                ContentPage.BackgroundProperty,
                Color.FromArgb("#F1F3F2"),
                Colors.Black
            );
            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                RowDefinitions = Rows.Define(
                    (BodyRow.first, 100),
                    (BodyRow.second, 35),
                    (BodyRow.third, 25),
                    (BodyRow.fourth, Auto)
                ),

                Children =
                {
                    new Image
                    {
                        Source = "booking_success",
                        HorizontalOptions = LayoutOptions.Center,
                    }.Row(BodyRow.first),
                    new SemiBold24Label
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Booking Successful",
                    }.Row(BodyRow.second),
                    new Regular14Label
                    {
                        TextColor = Color.FromArgb("A2A2A2"),
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "A confirmation email will be sent soon",
                    }.Row(BodyRow.third),
                    new MediumButton
                    {
                        Text = "DONE",
                        WidthRequest = 250,
                        HorizontalOptions = LayoutOptions.Center,
                    }
                        .Margins(0, 15, 0, 0)
                        .BindCommand(nameof(viewModel.CloseBookingCommand))
                        .Row(BodyRow.fourth),
                },
            };

            BindingContext = viewModel;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
