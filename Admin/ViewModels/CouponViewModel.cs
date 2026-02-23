namespace Admin.ViewModels
{
    public partial class CouponViewModel : ObservableObject
    {
        // properties for new coupon
        [ObservableProperty]
        string couponCode = string.Empty;

        [ObservableProperty]
        decimal discountPercentage;

        [ObservableProperty]
        DateTime expiryDate;

        [ObservableProperty]
        decimal couponUpperLimit;

        // errors in new coupon properties

        [ObservableProperty]
        string couponCodeError = string.Empty;

        [ObservableProperty]
        string discountPercentageError = string.Empty;

        [ObservableProperty]
        string expiryDateError = string.Empty;

        [ObservableProperty]
        string couponUpperLimitError = string.Empty;

        // properties for filtering existing coupons
        [ObservableProperty]
        List<string>? couponStatuses;

        [ObservableProperty]
        string? selectedCouponStatus;

        [ObservableProperty]
        ObservableCollection<Coupon>? allCoupons;

        [ObservableProperty]
        ObservableCollection<Coupon>? filteredCoupons;

        Realm realm;

        IDisposable couponToken;

        private readonly IPopupNavigation? popupNavigation;
        private readonly IRealmService? realmService;

        public CouponViewModel(IPopupNavigation popupNavigation, IRealmService realmService)
        {
            this.popupNavigation = popupNavigation;
            this.realmService = realmService;

            AllCoupons = new ObservableCollection<Coupon>();
            FilteredCoupons = new ObservableCollection<Coupon>();

            CouponStatuses = new List<string>() { "Expired", "Active", "Used" };
        }

        public async Task Initialize()
        {
            try
            {
                realm = Realm.GetInstance(realmService.Config);
                AllCoupons = new ObservableCollection<Coupon>(realm.All<Coupon>());
                FilteredCoupons = AllCoupons
                    .Where(c => c.CouponStatus == CouponPosition.unused.ToString())
                    .ToObservableCollection();

                SelectedCouponStatus = CouponStatuses[1];

                couponToken = realm
                    .All<Coupon>()
                    .SubscribeForNotifications(
                        async (sender, changes) =>
                        {
                            AllCoupons = new ObservableCollection<Coupon>(realm.All<Coupon>());
                            await SelectedCouponStatusChanged();
                        }
                    );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedCouponStatus))
            {
                await SelectedCouponStatusChanged();
            }
        }

        private async Task SelectedCouponStatusChanged()
        {
            try
            {
                if (SelectedCouponStatus == CouponStatuses[0])
                {
                    FilteredCoupons = AllCoupons
                        .Where(c => c.CouponStatus == CouponPosition.expired.ToString())
                        .ToObservableCollection();
                }
                else if (SelectedCouponStatus == CouponStatuses[1])
                {
                    FilteredCoupons = AllCoupons
                        .Where(c => c.CouponStatus == CouponPosition.unused.ToString())
                        .ToObservableCollection();
                }
                else if (SelectedCouponStatus == CouponStatuses[2])
                {
                    FilteredCoupons = AllCoupons
                        .Where(c => c.CouponStatus == CouponPosition.used.ToString())
                        .ToObservableCollection();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// delete existing coupon
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task DeleteCoupon(Coupon coupon)
        {
            try
            {
                bool userResponse = await Shell.Current.DisplayAlert(
                    "delete the coupon",
                    "the coupon will be delted permanently",
                    "Yes",
                    "No"
                );
                if (userResponse)
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Remove(coupon);
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        /// <summary>
        /// mopup for adding new coupon
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task AddCoupon()
        {
            try
            {
                await popupNavigation.PushAsync(new AddCouponMopup(this));
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        /// <summary>
        /// on Save button clicked in new coupon mopup
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task SaveCoupon()
        {
            try
            {
                bool hasErrors = FindErrors();
                if (hasErrors)
                {
                    return;
                }

                DateTimeOffset dateTime = DateHelper.GetDate(ExpiryDate.Date);
                Coupon coupon = new Coupon(
                    CouponCode,
                    DiscountPercentage,
                    dateTime,
                    CouponPosition.used.ToString(),
                    maximumDiscount: CouponUpperLimit
                );

                await realm.WriteAsync(() =>
                {
                    realm.Add(coupon);
                });

                CouponCode = string.Empty;
                DiscountPercentage = 0;
                ExpiryDate = DateTime.Now;
                CouponUpperLimit = 0;

                await popupNavigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        /// <summary>
        /// validate all the fields in new coupon mopup before saving new coupon
        /// </summary>
        /// <returns></returns>
        bool FindErrors()
        {
            CouponCodeError = string.IsNullOrEmpty(CouponCode)
                ? "Coupon code is required"
                : string.Empty;

            DiscountPercentageError =
                DiscountPercentage <= 0 ? "should be greater than 0" : string.Empty;

            ExpiryDateError =
                ExpiryDate < DateTime.Today
                    ? "should be equal to or greater than today"
                    : string.Empty;

            CouponUpperLimitError =
                CouponUpperLimit <= 0 ? "should be greater than 0" : string.Empty;

            List<Coupon> activeCoupons = AllCoupons
                .Where(c => c.CouponStatus == CouponPosition.unused.ToString())
                .ToList();

            if (activeCoupons.Count > 0)
            {
                if (!string.IsNullOrEmpty(CouponCode))
                {
                    foreach (Coupon activeCoupon in activeCoupons)
                    {
                        if (activeCoupon.CouponCode == CouponCode)
                        {
                            CouponCodeError = "Coupon code already exists";
                        }
                    }
                }
            }

            return (
                !string.IsNullOrEmpty(CouponCodeError)
                || !string.IsNullOrEmpty(DiscountPercentageError)
                || !string.IsNullOrEmpty(ExpiryDateError)
                || !string.IsNullOrEmpty(CouponUpperLimitError)
            );
        }

        /// <summary>
        /// close the new coupon mopup
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task CancelNewCoupon()
        {
            try
            {
                await popupNavigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
