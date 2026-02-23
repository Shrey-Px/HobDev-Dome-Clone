namespace Admin.View;

public class CouponView : ContentPage
{
    CouponViewModel viewModel;

    public CouponView(CouponViewModel viewModel)
    {
        Title = "Coupons";
        this.viewModel = viewModel;
        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                WidthRequest = 600,
                Padding = 20,
                Spacing = 10,
                Children =
                {
                    new HorizontalStackLayout
                    {
                        Spacing = 40,
                        Children =
                        {
                            new Picker { WidthRequest = 305 }
                                .Bind(Picker.ItemsSourceProperty, static (CouponViewModel vm) => vm.CouponStatuses, source: viewModel)
                                .Bind(
                                    Picker.SelectedItemProperty,
                                    static (CouponViewModel vm) => vm.SelectedCouponStatus,
                                    mode: BindingMode.TwoWay,
                                    source: viewModel
                                ),
                            new RegularButton
                            {
                                Text = "New Coupon",
                                WidthRequest = 200,
                            }.BindCommand(static (CouponViewModel vm) => vm.AddCouponCommand, source: viewModel),
                        },
                    }.Row(BodyRow.first),
                    new CollectionView
                    {
                        ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical)
                        {
                            HorizontalItemSpacing = 10,
                            VerticalItemSpacing = 10,
                        },
                        ItemTemplate = new DataTemplate(() =>
                        {
                            return new Grid
                            {
                                RowDefinitions = Rows.Define(
                                    (BodyRow.first, Auto),
                                    (BodyRow.second, Auto),
                                    (BodyRow.third, Auto),
                                    (BodyRow.fourth, Auto),
                                    (BodyRow.fifth, Auto),
                                    (BodyRow.sixth, Auto),
                                    (BodyRow.seventh, Auto)
                                ),

                                Padding = 20,
                                ColumnSpacing = 10,
                                Children =
                                {
                                    new Bold12Label { Text = "Coupon Code" }.Row(BodyRow.first),
                                    new Regular14Label { }
                                        .Bind(
                                            Regular14Label.TextProperty,
                                            static (Coupon coupon) => coupon.CouponCode
                                        )
                                        .Row(BodyRow.second),
                                    new Bold12Label { Text = "Discount" }
                                        .Row(BodyRow.third)
                                        .ColumnSpan(2)
                                        .Margins(0, 10, 0, 0),
                                    new Regular14Label { }
                                        .Bind(
                                            Regular14Label.TextProperty,
                                            static (Coupon coupon) => coupon.DiscountPercentage,
                                            stringFormat: "{0}%"
                                        )
                                        .Row(BodyRow.fourth),
                                    new Bold12Label { Text = "Expiry" }
                                        .Row(BodyRow.fifth)
                                        .Margins(0, 10, 0, 0),
                                    new Regular14Label { }
                                        .Bind(
                                            Regular14Label.TextProperty,
                                            static (Coupon coupon) => coupon.ExpiryDate,
                                            stringFormat: "{0:dd MMMM, yyyy}"
                                        )
                                        .Row(BodyRow.sixth),
                                    new RegularButton
                                    {
                                        Text = "DELETE",
                                        FontSize = 14,
                                        WidthRequest = 100,
                                    }
                                        .BindCommand(
                                            static (CouponViewModel vm) => vm.DeleteCouponCommand,
                                            source: viewModel
                                        )
                                        .Row(BodyRow.seventh)
                                        .Margins(0, 15, 0, 0),
                                },
                            }.AppThemeBinding(
                                Border.BackgroundProperty,
                                Color.FromArgb("#FAFAFA"),
                                Color.FromArgb("#23262A")
                            );
                        }),
                    }.Bind(ItemsView.ItemsSourceProperty, static (CouponViewModel vm) => vm.FilteredCoupons, source: viewModel),
                },
            },
        };

        BindingContext = viewModel;

        Loaded += CouponView_Loaded;
    }

    private async void CouponView_Loaded(object? sender, EventArgs e)
    {
        await viewModel.Initialize();
    }
}
