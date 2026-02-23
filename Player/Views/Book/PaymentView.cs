using Microsoft.Maui.Layouts;
using Stripe.PaymentSheets;

namespace Player.Views.Book;

public class PaymentView : BaseView
{
    PaymentViewModel viewModel;
    int count = 0;

    public PaymentView(PaymentViewModel viewModel)
        : base(viewModel)
    {
        try
        {
            this.viewModel = viewModel;

            Border topBorder = new Border
            {
                StrokeThickness = 0,
                Padding = new Thickness(18, 14),
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(7) },
                Content = new VerticalStackLayout
                {
                    Spacing = 10,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Bold28Label { Text = "Your Booking \nReview" },
                        new Bold10Label { Text = "Confirm below details to proceed with booking" },
                    },
                },
            }
                .AppThemeBinding(Border.BackgroundProperty, Colors.White, Color.FromArgb("#23262A"))
                .Margins(16, 0, 16, 0);

            Border secondBorder = new Border
            {
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(3) },
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(16, 10, 16, 0),
                Content = new Grid
                {
                    RowDefinitions = Rows.Define(
                        (BodyRow.first, Auto),
                        (BodyRow.second, Auto),
                        (BodyRow.third, Auto),
                        (BodyRow.fourth, Auto),
                        (BodyRow.fifth, Auto),
                        (BodyRow.sixth, Auto)
                    ),

                    ColumnDefinitions = Columns.Define(
                        (BodyColumn.first, Star),
                        (BodyColumn.second, Auto)
                    ),
                    ColumnSpacing = 10,

                    Children =
                    {
                        new Image { Source = "background.png", Aspect = Aspect.AspectFill }
                            .Column(BodyColumn.first)
                            .ColumnSpan(2)
                            .Row(BodyRow.first)
                            .RowSpan(6)
                            .Margins(0, 0, 0, 0),
                        new SemiBold18Label { TextColor = Colors.White }
                            .Bind(Bold16Label.TextProperty, nameof(viewModel.VenueName))
                            .Column(BodyColumn.first)
                            .Row(BodyRow.first)
                            .Margins(17, 16, 17, 0),
                        new Medium10Label { TextColor = Colors.White }
                            .Bind(Bold14Label.TextProperty, nameof(viewModel.VenueAddress))
                            .Column(BodyColumn.first)
                            .Row(BodyRow.second)
                            .Margins(17, 0, 17, 0),
                        new MediumButton
                        {
                            Padding = new Thickness(10, 5),
                            HorizontalOptions = LayoutOptions.Start,
                            FontSize = 12,
                            WidthRequest = 110,
                        }
                            .Bind(
                                MediumButton.TextProperty,
                                nameof(Booking.StartTime),
                                stringFormat: "{0:dd MMM yyyy}"
                            )
                            .Column(BodyColumn.first)
                            .ColumnSpan(2)
                            .Row(BodyRow.third)
                            .Margins(17, 15, 17, 0),
                        new Regular14Label { TextColor = Colors.White }
                            .Bind(
                                Regular14Label.TextProperty,
                                binding1: new Binding(
                                    nameof(viewModel.StartTime),
                                    stringFormat: "{0:HH:mm}"
                                ),
                                binding2: new Binding(
                                    nameof(viewModel.EndTime),
                                    stringFormat: "{0:HH:mm}"
                                ),
                                convert: ((string? start, string? end) values) =>
                                    $"{values.start} to {values.end}"
                            )
                            .Column(BodyColumn.first)
                            .Row(BodyRow.fourth)
                            .Margins(17, 6, 17, 0),
                        new Regular14Label
                        {
                            HorizontalOptions = LayoutOptions.Start,
                            TextColor = Colors.White,
                        }
                            .Bind(Regular14Label.TextProperty, nameof(viewModel.Fields))
                            .Column(BodyColumn.first)
                            .Row(BodyRow.fifth)
                            .Margins(17, 3, 17, 16),
                    },
                },
            }.AppThemeBinding(Border.BackgroundProperty, Colors.White, Color.FromArgb("#23262A"));

            Span termsAndConditionsSpan = new Span
            {
                Text = "Terms and Conditions",
                TextDecorations = TextDecorations.Underline,
            }.AppThemeBinding(
                Span.TextColorProperty,
                Color.FromArgb("007AFF"),
                Color.FromArgb("4DA3FF")
            );

            termsAndConditionsSpan.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                        await Microsoft.Maui.ApplicationModel.Launcher.Default.OpenAsync(
                            "https://www.dome-sports.com/useragreement"
                        )
                    ),
                }
            );

            FormattedString bookingTermsFormattedText = new FormattedString();
            bookingTermsFormattedText.Spans.Add(
                new Span { Text = "Please review our Booking policies and " }
            );
            bookingTermsFormattedText.Spans.Add(termsAndConditionsSpan);
            bookingTermsFormattedText.Spans.Add(
                new Span
                {
                    Text =
                        "; by proceeding with your reservation, you acknowledge and agree to these terms",
                }
            );

            base.TopGrid.Children.Add(
                new ImageButton
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                }.AppThemeBinding(ImageButton.SourceProperty, "dome_light_theme", "dome_dark_theme")
            );

            base.BaseContentGrid.Add(
                new Grid
                {
                    RowDefinitions = Rows.Define(
                        (BodyRow.first, Star),
                        // this row is for cancel button and payment amount. The row is out of scrollview
                        (BodyRow.second, Auto)
                    ),
                    Children =
                    {
                        new ScrollView
                        {
                            Content = new Grid
                            {
                                RowDefinitions = Rows.Define(
                                    (BodyRow.first, Auto),
                                    (BodyRow.second, Auto),
                                    (BodyRow.third, Auto)
                                ),

                                Children =
                                {
                                    topBorder.Row(BodyRow.first).ColumnSpan(2),
                                    secondBorder.Row(BodyRow.second).ColumnSpan(2),
                                    new Grid
                                    {
                                        RowDefinitions = Rows.Define(
                                            (BodyRow.first, Auto),
                                            (BodyRow.second, Auto),
                                            (BodyRow.third, Auto),
                                            (BodyRow.fourth, Auto),
                                            (BodyRow.fifth, Auto),
                                            (BodyRow.sixth, Auto),
                                            (BodyRow.seventh, Auto),
                                            (BodyRow.eighth, Auto),
                                            (BodyRow.ninth, Auto),
                                            (BodyRow.tenth, Auto),
                                            (BodyRow.eleventh, Auto),
                                            (BodyRow.twelfth, Auto),
                                            (BodyRow.thirteenth, Auto),
                                            (BodyRow.fourteenth, Auto),
                                            (BodyRow.fifteenth, Auto),
                                            (BodyRow.sixteenth, Auto),
                                            (BodyRow.seventeenth, Auto),
                                            (BodyRow.eighteenth, Auto),
                                            (BodyRow.nineteenth, Auto),
                                            (BodyRow.twentieth, Auto),
                                            (BodyRow.twentyfirst, Auto),
                                            (BodyRow.twentysecond, Auto),
                                            (BodyRow.twentythird, Auto),
                                            (BodyRow.twentyfourth, Auto),
                                            (BodyRow.twentyfifth, Auto),
                                            (BodyRow.twentysixth, Auto),
                                            (BodyRow.twentyseventh, Auto),
                                            (BodyRow.twentyeighth, Auto),
                                            (BodyRow.twentyninth, Auto),
                                            (BodyRow.thirty, Auto),
                                            (BodyRow.thirtyone, Auto),
                                            (BodyRow.thirtytwo, Auto),
                                            (BodyRow.thirtythree, Auto),
                                            (BodyRow.thirtyfour, Auto),
                                            (BodyRow.thirtyfive, Auto),
                                            (BodyRow.thirtysix, Auto),
                                            (BodyRow.thirtyseven, Auto),
                                            (BodyRow.thirtyeight, Auto),
                                            (BodyRow.thirtynine, Auto),
                                            (BodyRow.forty, Auto),
                                            (BodyRow.fortyone, Auto),
                                            (BodyRow.fortytwo, Auto),
                                            (BodyRow.fortythree, Auto),
                                            (BodyRow.fortyfour, Auto),
                                            (BodyRow.fortyfive, Auto),
                                            (BodyRow.fortysix, Auto),
                                            (BodyRow.fortyseven, Auto),
                                            (BodyRow.fortyeight, Auto)
                                        ),

                                        ColumnDefinitions = Columns.Define(
                                            (BodyColumn.first, Stars(2)),
                                            (BodyColumn.second, Stars(1.5))
                                        ),
                                        Children =
                                        {
                                            new Regular14Label { Text = "Booking Price" }
                                                .Column(BodyColumn.first)
                                                .Row(BodyRow.first)
                                                .Margins(19, 9, 0, 0),
                                            new SemiBold14Label
                                            {
                                                HorizontalOptions = LayoutOptions.End,
                                            }
                                                .Bind(
                                                    SemiBold14Label.TextProperty,
                                                    nameof(viewModel.BookingPrice),
                                                    stringFormat: "$ {0}"
                                                )
                                                .Column(BodyColumn.second)
                                                .Row(BodyRow.first)
                                                .Margins(0, 9, 16, 0),
                                            new Regular14Label { Text = "Service Fee" }
                                                .Column(BodyColumn.first)
                                                .Row(BodyRow.second)
                                                .Margins(19, 9, 0, 0),
                                            new SemiBold14Label
                                            {
                                                HorizontalOptions = LayoutOptions.End,
                                            }
                                                .Bind(
                                                    SemiBold14Label.TextProperty,
                                                    nameof(viewModel.ConvenienceFee),
                                                    stringFormat: "$ {0}"
                                                )
                                                .Column(BodyColumn.second)
                                                .Row(BodyRow.second)
                                                .Margins(0, 9, 16, 0),
                                            new Regular14Label { Text = "Discount" }
                                                .Column(BodyColumn.first)
                                                .Row(BodyRow.third)
                                                .Margins(19, 9, 0, 0),
                                            new SemiBold14Label
                                            {
                                                HorizontalOptions = LayoutOptions.End,
                                                TextColor = Color.FromArgb("5FB904"),
                                            }
                                                .Bind(
                                                    SemiBold14Label.TextProperty,
                                                    nameof(viewModel.Discount),
                                                    stringFormat: "$ {0}"
                                                )
                                                .Column(BodyColumn.second)
                                                .Row(BodyRow.third)
                                                .Margins(0, 9, 16, 0),
                                            // new Regular14Label { Text = "Taxes" }
                                            //     .Column(BodyColumn.first)
                                            //     .Row(BodyRow.fourth)
                                            //     .Margins(19, 9, 0, 0),
                                            // new SemiBold14Label
                                            // {
                                            //     HorizontalOptions = LayoutOptions.End,
                                            // }
                                            //     .Bind(
                                            //         SemiBold14Label.TextProperty,
                                            //         nameof(viewModel.Taxes),
                                            //         stringFormat: "$ {0}"
                                            //     )
                                            //     .Column(BodyColumn.second)
                                            //     .Row(BodyRow.fourth)
                                            //     .Margins(0, 9, 16, 0),
                                            new Border
                                            {
                                                StrokeThickness = 0,

                                                StrokeShape = new RoundRectangle
                                                {
                                                    CornerRadius = new CornerRadius(14),
                                                },
                                                Padding = new Thickness(10, 0),
                                                Content = new Entry
                                                {
                                                    FontSize = 12,
                                                    FontFamily = "InterRegular",
                                                    Placeholder = "Enter Coupon",
                                                    VerticalOptions = LayoutOptions.Fill,
                                                    HorizontalOptions = LayoutOptions.Fill,
                                                    VerticalTextAlignment = TextAlignment.Center,
                                                    Background = Colors.Transparent,
                                                }
                                                    .Bind(
                                                        Entry.TextProperty,
                                                        nameof(viewModel.CouponCode)
                                                    )
                                                    .Bind(
                                                        Entry.IsReadOnlyProperty,
                                                        nameof(viewModel.FixedCouponCode)
                                                    )
                                                    .AppThemeBinding(
                                                        Entry.TextColorProperty,
                                                        Colors.Black,
                                                        Color.FromArgb("BBBBBB")
                                                    )
                                                    .AppThemeBinding(
                                                        Entry.PlaceholderColorProperty,
                                                        Colors.Black,
                                                        Color.FromArgb("BBBBBB")
                                                    ),
                                            }
                                                .AppThemeBinding(
                                                    Border.BackgroundProperty,
                                                    Color.FromArgb("F1F3F2"),
                                                    Color.FromArgb("23262A")
                                                )
                                                .Column(BodyColumn.first)
                                                .Row(BodyRow.fifth)
                                                .Margins(19, 18, 15, 0),
                                            new MediumButton
                                            {
                                                Text = "Apply",
                                                FontSize = 14,
                                                HeightRequest = 40,
                                                Padding = 0,
                                                CornerRadius = 14,
                                            }
                                                .BindCommand(nameof(viewModel.ApplyCouponCommand))
                                                .Column(BodyColumn.second)
                                                .Row(BodyRow.fifth)
                                                .Margins(15, 18, 16, 0),
                                            new Regular14Label
                                            {
                                                FormattedText = bookingTermsFormattedText,
                                            }
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.sixth)
                                                .Margins(6, 35, 0, 5),
                                            new Regular14Label { Text = "Booking Policies" }
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.seventh)
                                                .Margins(6, 35, 0, 5),
                                            new Bold12Label { Text = "Cancellation Policy" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.eighth)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Cancellation is only allowed within 24 hours of booking.\n\rExceptions: In rare cases of facility closure due to unforeseen circumstances (e.g., severe weather, facility maintenance, or other emergencies), we may offer a reschedule of the booking, subject to availability. This will be determined on case-by-case basis and is at the sole discretion of the facility.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.ninth)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Payment Policy" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.tenth)
                                                .Margins(6, 10, 0, 0),
                                            new Grid
                                            {
                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, Auto),
                                                    (BodyColumn.second, Star)
                                                ),
                                                ColumnSpacing = 10,
                                                Children =
                                                {
                                                    new Ellipse
                                                    {
                                                        VerticalOptions = LayoutOptions.Start,
                                                        HeightRequest = 6,
                                                        WidthRequest = 6,
                                                    }
                                                        .AppThemeBinding(
                                                            BackgroundProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(0, 6, 0, 0),
                                                    new Regular12Label
                                                    {
                                                        Text =
                                                            "Booking fees are applicable and must be paid at the time of booking. The fee structure is available on the booking page. ",
                                                    }
                                                        .Column(BodyColumn.second)
                                                        .AppThemeBinding(
                                                            Label.TextColorProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(5, 0, 0, 0),
                                                },
                                            }
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.eleventh)
                                                .Margins(15, 5, 0, 0),
                                            new Grid
                                            {
                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, Auto),
                                                    (BodyColumn.second, Star)
                                                ),
                                                ColumnSpacing = 10,
                                                Children =
                                                {
                                                    new Ellipse
                                                    {
                                                        VerticalOptions = LayoutOptions.Start,
                                                        HeightRequest = 6,
                                                        WidthRequest = 6,
                                                    }
                                                        .AppThemeBinding(
                                                            Ellipse.BackgroundProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(0, 6, 0, 0),
                                                    new Regular12Label
                                                    {
                                                        Text =
                                                            "All payment transaction on our platform are processed using the secure encryption technology like industry-standard  secure socket layer (SSL) encryption to protect your payment information during transmission",
                                                    }
                                                        .Column(BodyColumn.second)
                                                        .AppThemeBinding(
                                                            Label.TextColorProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(5, 0, 0, 0),
                                                },
                                            }
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twelfth)
                                                .Margins(15, 3, 0, 0),
                                            new Grid
                                            {
                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, Auto),
                                                    (BodyColumn.second, Star)
                                                ),
                                                ColumnSpacing = 10,
                                                Children =
                                                {
                                                    new Ellipse
                                                    {
                                                        VerticalOptions = LayoutOptions.Start,
                                                        HeightRequest = 6,
                                                        WidthRequest = 6,
                                                    }
                                                        .AppThemeBinding(
                                                            Ellipse.BackgroundProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(0, 6, 0, 0),
                                                    new Regular12Label
                                                    {
                                                        Text =
                                                            "We use the reputable Stripe as a third party payment processor to handle payment transactions. this processor complies with payment card industry Data Security Standard (PCI DSS) regulations to ensure the security of your payment information. ",
                                                    }
                                                        .Column(BodyColumn.second)
                                                        .AppThemeBinding(
                                                            Label.TextColorProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(5, 0, 0, 0),
                                                },
                                            }
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirteenth)
                                                .Margins(15, 3, 0, 0),
                                            new Grid
                                            {
                                                ColumnDefinitions = Columns.Define(
                                                    (BodyColumn.first, Auto),
                                                    (BodyColumn.second, Star)
                                                ),
                                                ColumnSpacing = 10,
                                                Children =
                                                {
                                                    new Ellipse
                                                    {
                                                        VerticalOptions = LayoutOptions.Start,
                                                        HeightRequest = 6,
                                                        WidthRequest = 6,
                                                    }
                                                        .AppThemeBinding(
                                                            Ellipse.BackgroundProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(0, 6, 0, 0),
                                                    new Regular12Label
                                                    {
                                                        Text =
                                                            "Any disputes related to payment should be reported to us promptly. we will work with you and our payment processors to resolve any issues.",
                                                    }
                                                        .Column(BodyColumn.second)
                                                        .AppThemeBinding(
                                                            Label.TextColorProperty,
                                                            Colors.Black,
                                                            Colors.White
                                                        )
                                                        .Margins(5, 0, 0, 0),
                                                },
                                            }
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fourteenth)
                                                .Margins(15, 3, 0, 0),
                                            new Bold12Label { Text = "Time Limits" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fifteenth)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users must adhere to their booked time slot. Overstaying may result in additional charges. ",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.sixteenth)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Check-In" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.seventeenth)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Upon arrival, all users must check in at the facilities front desk. ",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.eighteenth)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Facility Rules" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.nineteenth)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users must follow all facility rules and regulations, including those related to equipment use, safety and behaviour. Booking a court constitutes and agreement to adhere to all rules, prohibitions and restrictions set forth by the facility. ",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentieth)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Damage" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentyfirst)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users are responsible for any damage to the facility caused by them, including equipment provided or rented. Facility reserves the right to revoke participation at any point for non-compliance with these rules, prohibition or restrictions.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentysecond)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Food and Beverages Policy" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentythird)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "The consumption of outside food or beverage including chewing gum and alcohol is not permitted within the facility.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentyfourth)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Smoking Policy" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentyfifth)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Smoking, including the use of traditional cigarettes, e-cigarettes and other vaping devices are strictly prohibited with all indoor areas of the facility.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentysixth)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Behaviour" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentyseventh)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users must conduct themselves in a respectful and sportsmanlike manner. Abusive or disruptive language and behavior towards staff and other participants will not be tolerated and may result in termination of the booking and potential suspension from using the facility.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentyeighth)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Dress Code" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.twentyninth)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users must wear appropriate sports attire and non-marking footwear at all times.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirty)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label
                                            {
                                                Text = "Private Coaching Regulations",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtyone)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Private coaching in any capacity is strictly forbidden within the premises of the facility unless there is explicit, written consent and prior approval from the Facility Regulatory Body. Authorized coaches are required to use their own equipments unless specific permission and prior approval are obtained from the Facility",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtytwo)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Liability" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtythree)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users participate at their own risk. The facility or the DOME Platform is not liable for injuries or accidents that occur during the use of the facility.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtyfour)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Medical Conditions" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtyfive)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users are encouraged to consult with a healthcare professional before using the facility if they have any medical conditions or concerns.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtysix)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Assumption of Risk" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtyseven)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Users are deemed to be fully aware of and accept the risks, hazards, and dangers associated with their presence at the facility. This includes, but is not limited to, the risks of accidents, personal injury, death, illness, disease transmission, property damage or loss, and theft. Participation in any activity, including voluntary and involuntary competition, exposes participants to the risk of injuries and various forms of loss or damage, whether direct, indirect, economic, personal, or consequential. ",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtyeight)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Maintenance" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.thirtynine)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "The facility may be closed for maintenance or special events. Users will be notified in advance of any such closures.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.forty)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Weather Conditions" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortyone)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "Outdoor facility bookings are subject to weather conditions. In case of adverse weather, users will be notified of cancellations or rescheduling options.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortytwo)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Customer Service" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortythree)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "For any issues or inquiries regarding bookings, please contact our customer support team at info@dafloinnovations.com.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortyfour)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Feedback" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortyfive)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "We welcome feedback on your booking experience to help us improve our services.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortysix)
                                                .Margins(6, 5, 6, 0),
                                            new Bold12Label { Text = "Amendments" }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortyseven)
                                                .Margins(6, 10, 0, 0),
                                            new Regular12Label
                                            {
                                                Text =
                                                    "This booking policy may be updated periodically. Users will be notified of any significant changes.",
                                            }
                                                .AppThemeBinding(
                                                    Label.TextColorProperty,
                                                    Colors.Black,
                                                    Colors.White
                                                )
                                                .Column(BodyColumn.first)
                                                .ColumnSpan(2)
                                                .Row(BodyRow.fortyeight)
                                                .Margins(6, 5, 6, 0),
                                        },
                                    }
                                        .AppThemeBinding(
                                            Grid.BackgroundProperty,
                                            Colors.White,
                                            Colors.Black
                                        )
                                        .Row(BodyRow.third)
                                        .ColumnSpan(2)
                                        .Margins(16, 10, 16, 0),
                                },
                            },
                        }.Row(BodyRow.first),
                        new Grid
                        {
                            ColumnDefinitions = Columns.Define(
                                (BodyColumn.first, Stars(2)),
                                (BodyColumn.second, Stars(3))
                            ),
                            ColumnSpacing = 10,
                            Children =
                            {
                                new MediumButton
                                {
                                    Text = "Cancel",
                                    Background = Colors.Transparent,
                                    BorderWidth = .5,
                                    HeightRequest = 38,
                                }
                                    .AppThemeColorBinding(
                                        MediumButton.TextColorProperty,
                                        Colors.Black,
                                        Colors.White
                                    )
                                    .AppThemeColorBinding(
                                        MediumButton.BorderColorProperty,
                                        Colors.Black,
                                        Colors.White
                                    )
                                    .BindCommand(nameof(viewModel.CancelCommand)),
                                new Border
                                {
                                    HeightRequest = 38,
                                    StrokeThickness = 0,
                                    Background = Color.FromArgb("#EF2F50"),
                                    StrokeShape = new RoundRectangle
                                    {
                                        CornerRadius = new CornerRadius(10),
                                    },
                                    Padding = new Thickness(20, 9),
                                    Content = new FlexLayout
                                    {
                                        HeightRequest = 42,
                                        Background = Colors.Transparent,
                                        JustifyContent = Microsoft
                                            .Maui
                                            .Layouts
                                            .FlexJustify
                                            .SpaceBetween,
                                        AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,

                                        Children =
                                        {
                                            new Medium15Label { TextColor = Colors.White }.Bind(
                                                Medium15Label.TextProperty,
                                                nameof(viewModel.TotalAmount),
                                                stringFormat: "$ {0}"
                                            ),
                                            new Medium15Label
                                            {
                                                Text = "Pay Now >>",
                                                TextColor = Colors.White,
                                            },
                                        },
                                    },
                                }
                                    .Column(BodyColumn.second)
                                    .BindTapGesture(nameof(viewModel.ProceedToPayCommand)),
                            },
                        }
                            .Row(BodyRow.second)
                            .Margins(19, 20, 19, 10),
                    },
                }
                    .AppThemeBinding(Grid.BackgroundProperty, Colors.White, Colors.Black)
                    .Row(BodyRow.second)
                    .ColumnSpan(2)
            );

            Loaded += PaymentView_Loaded;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void PaymentView_Loaded(object? sender, EventArgs e)
    {
        count++;
        if (count == 1)
        {
            await viewModel.Init();
        }
    }
}
