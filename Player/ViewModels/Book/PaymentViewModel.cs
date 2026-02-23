namespace Player.ViewModels
{
    public partial class PaymentViewModel : BaseViewModel
    {
        [ObservableProperty]
        IEnumerable<Booking>? bookedBookings;

        [ObservableProperty]
        IList<Booking>? selectedBookings;

        [ObservableProperty]
        IEnumerable<Booking>? createdBookings;

        [ObservableProperty]
        IEnumerable<Booking>? reservedBookings;

        [ObservableProperty]
        string? venueName;

        [ObservableProperty]
        string? venueAddress;

        [ObservableProperty]
        DateTimeOffset startTime;

        [ObservableProperty]
        DateTimeOffset endTime;

        [ObservableProperty]
        ObservableCollection<string>? selectedFields;

        [ObservableProperty]
        string? fields;

        [ObservableProperty]
        byte[]? playerImage;

        // properties for coupon
        [ObservableProperty]
        decimal? discount;

        [ObservableProperty]
        string? couponCode;

        [ObservableProperty]
        bool fixedCouponCode;

        [ObservableProperty]
        Coupon? selectedCoupon;

        [ObservableProperty]
        string? couponType;

        readonly Realm? syncRealm;

        [ObservableProperty]
        decimal? bookingPrice;

        [ObservableProperty]
        decimal? taxes;

        [ObservableProperty]
        decimal? convenienceFee;

        [ObservableProperty]
        decimal? totalAmount;

        [ObservableProperty]
        VenueUser? gamer;

        readonly IDisposable? bookingsToken;

        private readonly INavigationService? navigationService;
        private readonly IRealmService? realmService;
        private readonly IConnectivity? connectivity;
        private IPaymentSheet? paymentSheet;
        private IStripeService? stripeService;

        public PaymentViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            IPaymentSheet paymentSheet,
            IStripeService stripeService
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.realmService = realmService;
                this.connectivity = connectivity;
                this.navigationService = navigationService;
                this.paymentSheet = paymentSheet;
                this.stripeService = stripeService;

                syncRealm = Realm.GetInstance(realmService.Config);

                Gamer = syncRealm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                PlayerImage = Gamer?.ProfileImage;

                // Observe collection notifications
                bookingsToken = Gamer?.Bookings.SubscribeForNotifications(
                    async (sender, changes) =>
                    {
                        if (changes == null)
                        {
                            // This is the case when the notification is called
                            // for the first time.
                            // Populate tableview/listview with all the items
                            // from `collection`

                            return;
                        }
                        //Handle individual changes

                        foreach (int i in changes.DeletedIndices)
                        {
                            // ... handle deletions ...

                            await Init();

                            return;
                        }
                        foreach (int i in changes.InsertedIndices)
                        {
                            // ... handle insertions ...

                            await Init();
                            return;
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task Init()
        {
            try
            {
                CreatedBookings = new ObservableCollection<Booking>();
                ReservedBookings = new ObservableCollection<Booking>();
                SelectedBookings = new List<Booking>();
                bookedBookings = new ObservableCollection<Booking>();
                SelectedFields = new ObservableCollection<string>();

                BookedBookings = Gamer?.Bookings.Where(g =>
                    g.BookingStatus == Dome.Shared.Constants.AppConstants.Booked
                    || g.BookingStatus == Dome.Shared.Constants.AppConstants.Completed
                );

                CreatedBookings = Gamer?.Bookings.Where(g =>
                    g.BookingStatus == Dome.Shared.Constants.AppConstants.Created
                );
                ReservedBookings = Gamer?.Bookings.Where(g =>
                    g.BookingStatus == Dome.Shared.Constants.AppConstants.Reserved
                );
                SelectedBookings.Clear();

                if (CreatedBookings?.Any() == true && ReservedBookings?.Any() != true)
                {
                    SelectedBookings = Gamer
                        .Bookings.Where(g => g.BookingStatus == AppConstants.Created)
                        .ToObservableCollection();
                }
                else if (CreatedBookings?.Any() != true && ReservedBookings?.Any() == true)
                {
                    SelectedBookings = Gamer
                        .Bookings.Where(g => g.BookingStatus == AppConstants.Reserved)
                        .ToObservableCollection();
                }
                else if (CreatedBookings?.Any() == true && ReservedBookings?.Any() == true)
                {
                    await Shell.Current.DisplayAlert(
                        "Alert",
                        "Please select either Created or Reserved Bookings",
                        "OK"
                    );
                }

                if (SelectedBookings.Any())
                {
                    VenueName = SelectedBookings.Select(g => g.Venue.FullName).FirstOrDefault();
                    await SetValues();
                }
                else
                {
                    await navigationService.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        partial void OnCouponCodeChanged(string? value)
        {
            // if user clear the coupon code, set the values to default
            if (String.IsNullOrWhiteSpace(value))
            {
                SelectedCoupon = null;
                SetValues();
            }
        }

        [RelayCommand]
        async Task ApplyCoupon()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(CouponCode))
                {
                    await Shell.Current.DisplayAlert("Alert", "Please enter a coupon code", "OK");
                    return;
                }

                if (BookedBookings.Count() == 0)
                {
                    await Shell.Current.DisplayAlert(
                        "Alert",
                        "20% discount for first time booking is already applied",
                        "OK"
                    );
                }
                else
                {
                    string? unUsedStatus = CouponPosition.unused.ToString();
                    List<Coupon> ActiveCoupons = syncRealm
                        .All<Coupon>()
                        .Where(c => c.CouponStatus == unUsedStatus)
                        .ToList();
                    Coupon? coupon = ActiveCoupons
                        .Where(c => c.CouponCode == CouponCode)
                        .FirstOrDefault();
                    if (coupon == null)
                    {
                        await Shell.Current.DisplayAlert("Alert", "Invalid Coupon Code", "OK");
                    }
                    else
                    {
                        SelectedCoupon = coupon;
                        await SetValues();

                        await Shell.Current.DisplayAlert("Alert", "Coupon Applied", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task Cancel()
        {
            try
            {
                bool result = await Shell.Current.DisplayAlert(
                    "Your selection will be cleared",
                    "Do you want to clear your selection?",
                    "Yes",
                    "No"
                );
                if (result)
                {
                    if (CreatedBookings.Any() && !ReservedBookings.Any())
                    {
                        await syncRealm.WriteAsync(() =>
                        {
                            foreach (Booking selectedBooking in SelectedBookings)
                            {
                                syncRealm.Remove(selectedBooking);
                            }
                        });

                        await navigationService.PopAsync();
                    }
                    else if (!CreatedBookings.Any() && ReservedBookings.Any())
                    {
                        // call api to cancel the payment Intent and delete the reserved bookings of the user
                        List<string> bookingIdsInString = new List<string>();
                        List<ObjectId> Ids = SelectedBookings.Select(g => g.Id).ToList();
                        foreach (ObjectId id in Ids)
                        {
                            string idInString = id.ToString();
                            bookingIdsInString.Add(idInString);
                        }
                        RequestWithBookingIds request = new RequestWithBookingIds(
                            bookingIds: bookingIdsInString,
                            environment: AppConstants.Environment
                        );

                        CancelPaymentIntentResponse? cancelPaymentIntentResponse =
                            await stripeService.CancelPaymentIntent(request);

                        if (cancelPaymentIntentResponse.BookingIds == null)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                                Shell.Current.DisplayAlert(
                                    "API Error",
                                    cancelPaymentIntentResponse?.Error,
                                    "OK"
                                )
                            );
                        }
                        else
                        {
                            await navigationService.PopAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async Task SetValues()
        {
            try
            {
                StartTime = SelectedBookings.Select(g => g.StartTime).FirstOrDefault();
                EndTime = SelectedBookings.Select(g => g.EndTime).FirstOrDefault();
                SelectedFields = SelectedBookings.Select(g => g.FieldName).ToObservableCollection();
                for (int i = 0; i < SelectedFields.Count; i++)
                {
                    if (i == 0)
                    {
                        Fields = SelectedFields[i];
                    }
                    else
                    {
                        Fields = Fields + ", " + SelectedFields[i];
                    }
                }
                VenueAddress? address = SelectedBookings
                    .Select(g => g.Venue?.Address)
                    .FirstOrDefault();
                VenueAddress =
                    address != null
                        ? $"{address.Street}, {address.City}, {address.Province}, {address.PostCode}"
                        : string.Empty;

                decimal discountPercentage = 0;
                Discount = 0;

                if (!BookedBookings.Any())
                {
                    CouponCode = "FIRST20";
                    FixedCouponCode = true;
                    // for first time booking, 20% discount is applied
                    discountPercentage = 20;
                    SelectedCoupon = null;
                }
                else if (BookedBookings.Any())
                {
                    if (SelectedCoupon != null)
                    {
                        discountPercentage = SelectedCoupon.DiscountPercentage;
                    }
                }

                // Ensure discount percentage never exceeds 100%
                if (discountPercentage > 100)
                {
                    discountPercentage = 100;
                }

                // Determine the maximum discount amount
                decimal maxDiscountAmount = SelectedCoupon?.MaximumDiscount ?? 100;

                // the payable amount is calculated by deducting the discount from price and then adding the convenience fee to the discounted price. Finally adding the taxes to the price after adding the convenience fee

                foreach (Booking selectedBooking in SelectedBookings)
                {
                    await syncRealm.WriteAsync(() =>
                    {
                        if (discountPercentage != 0)
                        {
                            if (Discount == 0)
                            {
                                selectedBooking.DiscountPercentage = discountPercentage;
                                selectedBooking.Discount = Math.Round(
                                    (selectedBooking.Price / 100) * discountPercentage,
                                    2
                                );

                                // Cap the discount at maximum allowed amount
                                if (selectedBooking.Discount > maxDiscountAmount)
                                {
                                    selectedBooking.Discount = maxDiscountAmount;
                                }
                            }
                            else if (Discount > 0)
                            {
                                decimal? discountForABooking = Discount / SelectedBookings.Count();
                                selectedBooking.Discount = Math.Round(
                                    (decimal)discountForABooking,
                                    2
                                );
                            }
                            selectedBooking.PriceAfterDeductingDiscount = Math.Round(
                                (decimal)(selectedBooking.Price - selectedBooking.Discount),
                                2
                            );
                        }
                        // if no discount than the price after deducting discount is the actual price
                        else
                        {
                            selectedBooking.PriceAfterDeductingDiscount = selectedBooking.Price;
                        }

                        selectedBooking.ConvenienceFee = Math.Round(
                            (decimal)(
                                selectedBooking.Price
                                * selectedBooking.ConvenienceFeePercentage
                                / 100
                            ),
                            2
                        );
                        selectedBooking.PriceAfterAddingConvenienceFee = Math.Round(
                            (decimal)(
                                selectedBooking.PriceAfterDeductingDiscount
                                + selectedBooking.ConvenienceFee
                            ),
                            2
                        );
                        selectedBooking.Tax = Math.Round(
                            (decimal)(
                                selectedBooking.PriceAfterAddingConvenienceFee
                                * selectedBooking.TaxPercentage
                                / 100
                            ),
                            2
                        );
                        selectedBooking.TotalPrice = Math.Round(
                            (decimal)(
                                selectedBooking.PriceAfterAddingConvenienceFee + selectedBooking.Tax
                            ),
                            2
                        );
                    });
                }

                //this is the actual price of the booking
                BookingPrice = Math.Round((decimal)SelectedBookings.Select(g => g.Price).Sum(), 2);

                // this is the total discount for the booking
                Discount = Math.Round((decimal)SelectedBookings.Select(g => g.Discount).Sum(), 2);

                // this is the convenience fee for the booking
                ConvenienceFee = Math.Round(
                    (decimal)SelectedBookings.Select(g => g.ConvenienceFee).Sum(),
                    2
                );

                // this is the total taxes for the booking
                Taxes = Math.Round((decimal)SelectedBookings.Select(g => g.Tax).Sum(), 2);

                // this is the total amount after adding the taxes
                TotalAmount = Math.Round(
                    (decimal)SelectedBookings.Select(g => g.TotalPrice).Sum(),
                    2
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task ProceedToPay()
        {
            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Alert", "No Network Connection", "OK");
                    return;
                }

                if (SelectedBookings.Any())
                {
                    if (BookedBookings.Any() || SelectedCoupon != null)
                    {
                        if (SelectedCoupon != null)
                        {
                            await syncRealm.WriteAsync(() =>
                            {
                                foreach (Booking reservedBooking in SelectedBookings)
                                {
                                    reservedBooking.AppliedCoupon = SelectedCoupon;
                                }
                            });
                        }
                    }
                    else if (!BookedBookings.Any())
                    {
                        await syncRealm.WriteAsync(() =>
                        {
                            foreach (Booking reservedBooking in SelectedBookings)
                            {
                                reservedBooking.IsFirstBookingCouponApplied = true;
                            }
                        });
                    }

                    string? clientSecret = await GetClientSecretFromServer();
                    if (clientSecret != null)
                    {
                        await DisplayPaymentSheet(clientSecret);
                    }
                }
                else
                {
                    await navigationService.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<string?> GetClientSecretFromServer()
        {
            try
            {
                string? gameName = SelectedBookings.Select(g => g.GameName).FirstOrDefault();
                string? venueName = SelectedBookings.Select(g => g.Venue.FullName).FirstOrDefault();
                string bookingDate = SelectedBookings
                    .Select(g => g.BookingDate)
                    .FirstOrDefault()
                    .ToString("dd/MM/yyyy");
                string startTime = SelectedBookings
                    .Select(g => g.StartTime)
                    .FirstOrDefault()
                    .ToString("HH:mm ");
                string endTime = SelectedBookings
                    .Select(g => g.EndTime)
                    .FirstOrDefault()
                    .ToString("HH:mm ");
                string? connectedAccountId = SelectedBookings
                    .Select(g => g.Venue.StripeUserId)
                    .FirstOrDefault();
                string? cartId = SelectedBookings.Select(g => g.CartId).FirstOrDefault();

                List<string> bookingIdsInString = new List<string>();
                List<ObjectId> Ids = SelectedBookings.Select(g => g.Id).ToList();
                foreach (ObjectId id in Ids)
                {
                    string idInString = id.ToString();
                    bookingIdsInString.Add(idInString);
                }

                if (string.IsNullOrWhiteSpace(connectedAccountId))
                {
                    await Shell.Current.DisplayAlert(
                        "Alert",
                        " Stripe Connect Account Id is missing for the vendor",
                        "OK"
                    );
                    return null;
                }

                RequestWithBookingIds request = new RequestWithBookingIds(
                    bookingIds: bookingIdsInString,
                    environment: AppConstants.Environment
                );

                // The helper method comes from the downloaded source code
                PaymentIntentCreateResponse? apiResponse = await stripeService.FetchPaymentIntent(
                    request
                );
                if (apiResponse?.ClientSecret == null)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                        Shell.Current.DisplayAlert("API Error", apiResponse?.Error, "OK")
                    );
                }
                else
                {
                    return apiResponse.ClientSecret;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            return null;
        }

        private async Task DisplayPaymentSheet(string clientSecret)
        {
            try
            {
                var configuration = new Configuration("Dome Sports");

                PaymentSheetResult result = await paymentSheet.PresentWithPaymentIntentAsync(
                    clientSecret,
                    configuration
                );

                switch (result)
                {
                    // if payment is successful, navigate to payment success page. The server will update the booking status to booked
                    case PaymentSheetResult.Completed:

                        await navigationService.NavigateToAsync(nameof(PaymentSuccessView));
                        break;
                    case PaymentSheetResult.Canceled:
                        await Shell.Current.DisplayAlert("Payment Result", "Payment failed!", "Ok");
                        break;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }
    }
}
