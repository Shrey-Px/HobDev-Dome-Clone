using Mopups.Pages;

namespace Admin.Popups
{
    public class AddCouponMopup : PopupPage
    {
        public AddCouponMopup(CouponViewModel viewModel)
        {
            this.Background = Color.FromArgb("#80000000");
            this.CloseWhenBackgroundIsClicked = false;

            this.Animation = new Mopups.Animations.ScaleAnimation
            {
                DurationIn = 700,
                EasingIn = Easing.SinIn,
                DurationOut = 700,
                EasingOut = Easing.SinIn,
                ScaleIn = 1.2,
                ScaleOut = 0.8,
                PositionIn = Mopups.Enums.MoveAnimationOptions.Bottom,
                PositionOut = Mopups.Enums.MoveAnimationOptions.Bottom,
            };

            Content = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Padding = 30,
                Background = Colors.LightGray,
                RowDefinitions = Rows.Define(
                    (BodyRow.first, Auto),
                    (BodyRow.second, Auto),
                    (BodyRow.third, Auto),
                    (BodyRow.fourth, Auto),
                    (BodyRow.fifth, Auto),
                    (BodyRow.sixth, Auto),
                    (BodyRow.seventh, Auto),
                    (BodyRow.eighth, Auto),
                    (BodyRow.ninth, Auto)
                ),

                ColumnDefinitions = Columns.Define(
                    (BodyColumn.first, 170),
                    (BodyColumn.second, 170)
                ),

                ColumnSpacing = 30,

                Children =
                {
                    new Bold20Label { Text = "Add new coupon" }
                        .Row(BodyRow.first)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                    new Regular14Label { Text = "Coupon Code" }
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.second)
                        .Column(BodyColumn.first),
                    new Entry { }
                        .Bind(Entry.TextProperty, nameof(viewModel.CouponCode))
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.third)
                        .Column(BodyColumn.first),
                    new Regular12Label { TextColor = Colors.Red }
                        .Bind(Regular12Label.TextProperty, nameof(viewModel.CouponCodeError))
                        .Bind(
                            Regular12Label.IsVisibleProperty,
                            nameof(viewModel.CouponCodeError),
                            converter: new IsStringNotNullOrEmptyConverter()
                        )
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.fourth)
                        .Column(BodyColumn.first),
                    new Regular14Label { Text = "Discount Percentage" }
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.second)
                        .Column(BodyColumn.second),
                    new Entry { }
                        .Bind(Entry.TextProperty, nameof(viewModel.DiscountPercentage))
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.third)
                        .Column(BodyColumn.second),
                    new Regular12Label { TextColor = Colors.Red }
                        .Bind(
                            Regular12Label.TextProperty,
                            nameof(viewModel.DiscountPercentageError)
                        )
                        .Bind(
                            Regular12Label.IsVisibleProperty,
                            nameof(viewModel.DiscountPercentageError),
                            converter: new IsStringNotNullOrEmptyConverter()
                        )
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.fourth)
                        .Column(BodyColumn.second),
                    new Regular14Label { Text = "Expiry Date" }
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.fifth)
                        .Column(BodyColumn.first),
                    new DatePicker
                    {
                        Date = DateTime.Today,
                        MinimumDate = new DateTime(2024, 06, 01),
                    }
                        .Bind(
                            DatePicker.DateProperty,
                            nameof(viewModel.ExpiryDate),
                            BindingMode.OneWayToSource
                        )
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.sixth)
                        .Column(BodyColumn.first),
                    new Regular12Label { TextColor = Colors.Red }
                        .Bind(Regular12Label.TextProperty, nameof(viewModel.ExpiryDateError))
                        .Bind(
                            Regular12Label.IsVisibleProperty,
                            nameof(viewModel.ExpiryDateError),
                            converter: new IsStringNotNullOrEmptyConverter()
                        )
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.seventh)
                        .Column(BodyColumn.first),
                    new Regular14Label { Text = "Coupon Upper Limit" }
                        .Margins(0, 15, 0, 0)
                        .Row(BodyRow.fifth)
                        .Column(BodyColumn.second),
                    new Entry { }
                        .Bind(Entry.TextProperty, nameof(viewModel.CouponUpperLimit))
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.sixth)
                        .Column(BodyColumn.second),
                    new Regular12Label { TextColor = Colors.Red }
                        .Bind(Regular12Label.TextProperty, nameof(viewModel.CouponUpperLimitError))
                        .Bind(
                            Regular12Label.IsVisibleProperty,
                            nameof(viewModel.CouponUpperLimitError),
                            converter: new IsStringNotNullOrEmptyConverter()
                        )
                        .Margins(0, 4, 0, 0)
                        .Row(BodyRow.seventh)
                        .Column(BodyColumn.second),
                    new RegularButton
                    {
                        Text = "Save",
                        WidthRequest = 200,
                        HorizontalOptions = LayoutOptions.Center,
                    }
                        .BindCommand(nameof(viewModel.SaveCouponCommand))
                        .Margins(0, 25, 0, 0)
                        .Row(BodyRow.eighth)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                    new RegularButton
                    {
                        Text = "Cancel",
                        WidthRequest = 200,
                        HorizontalOptions = LayoutOptions.Center,
                    }
                        .BindCommand(nameof(viewModel.CancelNewCouponCommand))
                        .Margins(0, 10, 0, 0)
                        .Row(BodyRow.ninth)
                        .Column(BodyColumn.first)
                        .ColumnSpan(2),
                },
            };

            BindingContext = viewModel;
        }
    }
}
