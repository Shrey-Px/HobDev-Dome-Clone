namespace Player.ViewModels;

using Dome.Shared.Synced.Player;

public partial class BookingTimingViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    byte[]? playerImage;

    [ObservableProperty]
    string? gameName;

    // this is the venue selected by the user in VenuesList view
    [ObservableProperty]
    Venue? selectedVenue;

    // this time is set by assigning venueOpen hour and minute to the selected date
    [ObservableProperty]
    DateTime venueOpenTime;

    // this time is set by assigning venueClose hour and minute to the selected date
    [ObservableProperty]
    DateTime venueCloseTime;

    [ObservableProperty]
    VenueUser? gamer;

    [ObservableProperty]
    ObservableCollection<Field>? availableFields;

    [ObservableProperty]
    ObservableCollection<object>? selectedFields;

    [ObservableProperty]
    IEnumerable<Holiday> holidays;

    [ObservableProperty]
    List<VenueDate>? venueDates;

    [ObservableProperty]
    VenueDate? selectedDate;

    [ObservableProperty]
    AvailableGame? selectedGame;

    [ObservableProperty]
    double bookingDuration;

    [ObservableProperty]
    ObservableCollection<VenueDate>? startHoursList;

    // the start time of the booking which is the combination of the selected date, selected hour and selected minute
    [ObservableProperty]
    VenueDate? selectedStartTime;

    bool isWeekDay;

    Realm? syncRealm;

    // Constants
    private const int MinBookingDurationMinutes = 60;
    private const int MaxBookingDurationMinutes = 180;
    private const int TimeSlotIntervalMinutes = 30;
    private const int BookingWindowDays = 90;
    private const int MinTimeRequiredToBookTodayMinutes = 75; // MinBookingDuration + buffer for checkout
    private const int AllowedPastBookingMinutes = 15;
    private const int MaxTimeSlotsFor24HourVenue = 22;

    private readonly IRealmService? realmService;
    private readonly IConnectivity? connectivity;
    private readonly INavigationService? navigationService;
    private readonly IAddToCartService? addToCartService;
    private readonly IPopupNavigation popupNavigation;
    private readonly IStripeService stripeService;

    public BookingTimingViewModel(
        IRealmService realmService,
        IConnectivity connectivity,
        INavigationService navigationService,
        IAddToCartService addToCartService,
        IPopupNavigation popupNavigation,
        IStripeService stripeService
    )
        : base(navigationService, realmService, connectivity)
    {
        try
        {
            this.realmService = realmService;
            this.connectivity = connectivity;
            this.navigationService = navigationService;
            this.addToCartService = addToCartService;
            this.popupNavigation = popupNavigation;
            this.stripeService = stripeService;

            syncRealm = Realm.GetInstance(realmService.Config);

            StartHoursList = new ObservableCollection<VenueDate>();
            AvailableFields = new ObservableCollection<Field>();
            SelectedFields = new ObservableCollection<object>();
            Holidays= new List<Holiday>();
            VenueDates = new List<VenueDate>();
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    #region Helper Methods

    /// <summary>
    /// Checks if the given date falls on a weekend (Saturday or Sunday)
    /// </summary>
    private static bool IsWeekend(DateTimeOffset date) =>
        date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

    /// <summary>
    /// Creates a DateTime by combining a date with specific hour and minute values
    /// </summary>
    private static DateTime BuildDateTime(DateTime date, int hour, int minute, int addDays = 0) =>
        new DateTime(date.Year, date.Month, date.Day, hour, minute, 0).AddDays(addDays);

    /// <summary>
    /// Creates a DateTime by combining a DateTimeOffset date with specific hour and minute values
    /// </summary>
    private static DateTime BuildDateTime(
        DateTimeOffset date,
        int hour,
        int minute,
        int addDays = 0
    ) => new DateTime(date.Year, date.Month, date.Day, hour, minute, 0).AddDays(addDays);

    /// <summary>
    /// Creates a DateTime by combining a date with the time components from another DateTime
    /// </summary>
    private static DateTime BuildDateTime(DateTime date, DateTime time, int addDays = 0) =>
        new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second).AddDays(
            addDays
        );

    /// <summary>
    /// Creates a DateTime by combining a DateTimeOffset date with the time components from another DateTime
    /// </summary>
    private static DateTime BuildDateTime(DateTimeOffset date, DateTime time, int addDays = 0) =>
        new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second).AddDays(
            addDays
        );

    /// <summary>
    /// Gets the open and close times based on whether it's a weekday or weekend
    /// </summary>
    private (DateTime openTime, DateTime closeTime) GetVenueTiming(bool isWeekend)
    {
        if (isWeekend)
        {
            return (
                SelectedGame.Timing.WeekendOpenTime.DateTime,
                SelectedGame.Timing.WeekendCloseTime.DateTime
            );
        }
        return (
            SelectedGame.Timing.WeekdayOpenTime.DateTime,
            SelectedGame.Timing.WeekdayCloseTime.DateTime
        );
    }

    /// <summary>
    /// Rounds the current time to the nearest time slot (30-minute intervals)
    /// </summary>
    private static DateTime RoundToNearestSlot(DateTime date)
    {
        int hour = date.Hour;
        int minute = date.Minute >= 30 ? 30 : 0;
        return BuildDateTime(date, hour, minute);
    }

    /// <summary>
    /// Checks if the given date is a Canada holiday
    /// </summary>
    private bool IsCanadaHoliday(DateTimeOffset date) =>
        Holidays?.Any(h => h.Date.Date == date.Date) == true;

    #endregion

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            foreach (var item in query)
            {
                if (item.Key == "gameName")
                {
                    GameName = item.Value.ToString();
                }
                if (item.Key == "venueId")
                {
                    ObjectId? id = (ObjectId)item.Value;
                    if (id != null)
                    {
                        await GetVenueFromDB(id);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await AppShell.Current.DisplayAlert("error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// called when the view model gets venue id from the query string. get the venue from the database and set the selected venue and selected game. Finally calls AddDates method
    /// </summary>
    /// <returns></returns>
    private async Task GetVenueFromDB(ObjectId? venueId)
    {
        try
        {
            SelectedVenue = syncRealm.Find<Venue>(venueId);
            Holidays = SelectedVenue.Holidays.Select(h => new Dome.Shared.Synced.Player.Holiday(h.Date, h.Description)).ToList();

            Gamer = syncRealm
                .All<VenueUser>()
                .Where(n => n.OwnerId == realmService.RealmUser.Id)
                .FirstOrDefault();

            PlayerImage = Gamer?.ProfileImage;

            SelectedGame = SelectedVenue
                ?.AvailableGames.Where(g => g.Name == GameName)
                .FirstOrDefault();

            await AddDates();

            // this is to remove the reserved bookings if any because they are not valid anymore as the app don't support cart system
            IEnumerable<Booking>? ReservedBookings = Gamer
                ?.Bookings.Where(g => g.BookingStatus == AppConstants.Reserved)
                .ToObservableCollection();

            if (ReservedBookings != null && ReservedBookings.Any())
            {
                await syncRealm.WriteAsync(() =>
                {
                    foreach (Booking reservedBooking in ReservedBookings)
                    {
                        syncRealm.Remove(reservedBooking);
                    }
                });
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// add dates to the VenueDates list for 90 days from today. set the selected date to today which will trigger the DateSelected method
    /// </summary>
    /// <returns></returns>
    async Task AddDates()
    {
        try
        {
            isWeekDay = !IsWeekend(DateTime.Today);
            var (openTime, closeTime) = GetVenueTiming(!isWeekDay);

            // if close time is equal to open time it means the venue remains open for 24 hours
            if (closeTime.TimeOfDay == openTime.TimeOfDay)
            {
                for (
                    DateTime date = DateTime.Now;
                    date < DateTime.Now.AddDays(BookingWindowDays - 1);
                    date = date.AddDays(1)
                )
                {
                    VenueDates?.Add(new VenueDate(date: date));
                }
            }
            // if venue closes on same day as it opens or closes after midnight
            else
            {
                // if the close time is less than open time it means the venue closes on the next day
                int addDays = closeTime.TimeOfDay < openTime.TimeOfDay ? 1 : 0;
                closeTime = BuildDateTime(DateTime.Today, closeTime, addDays);

                // if less than minimum required time to book today, start from tomorrow
                if (
                    closeTime - DateTime.Now
                    < TimeSpan.FromMinutes(MinTimeRequiredToBookTodayMinutes)
                )
                {
                    for (
                        DateTime date = DateTime.Now.AddDays(1);
                        date < DateTime.Now.AddDays(BookingWindowDays);
                        date = date.AddDays(1)
                    )
                    {
                        VenueDates?.Add(new VenueDate(date: date));
                    }
                }
                // start the date from today
                else
                {
                    for (
                        DateTime date = DateTime.Now;
                        date < DateTime.Now.AddDays(BookingWindowDays - 1);
                        date = date.AddDays(1)
                    )
                    {
                        VenueDates?.Add(new VenueDate(date: date));
                    }
                }
            }

            SelectedDate = VenueDates?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Highlight the selected date and get start time and close time of the venue according to the weekday or weekend. Finally calls SetTimingLists method
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task DateSelected()
    {
        try
        {
            VenueDates.ToList().ForEach(m => m.IsSelected = false);
            // highlight the selected date in the view
            SelectedDate.IsSelected = true;

            isWeekDay = !IsWeekend(SelectedDate.Date);
            var (openTime, closeTime) = GetVenueTiming(!isWeekDay);

            // the venue closes on the next day if close time < open time
            int addDays = closeTime.TimeOfDay < openTime.TimeOfDay ? 1 : 0;
            VenueCloseTime = BuildDateTime(SelectedDate.Date, closeTime, addDays);
            VenueOpenTime = BuildDateTime(SelectedDate.Date, openTime);

            await SetTimingList(VenueOpenTime, VenueCloseTime);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// invoked when the user select the date from the date picker or when initial date is set after dates where added. save the time list in StartHoursList according to the open time and close time of the venue. calls SetStartTime method and set the booking duration to 60 minutes
    /// </summary>
    /// <returns></returns>
    public async Task SetTimingList(DateTime openTime, DateTime closeTime)
    {
        try
        {
            BookingDuration = MinBookingDurationMinutes;
            await SetSelectedStartTime();
            StartHoursList.Clear();
            if (openTime == closeTime)
            {
                // 24-hour venue: show time slots for display purposes
                int count = 1;
                while (count <= MaxTimeSlotsFor24HourVenue)
                {
                    StartHoursList.Add(new VenueDate(date: closeTime));
                    closeTime = closeTime.AddMinutes(TimeSlotIntervalMinutes);
                    count++;
                }
            }
            else
            {
                while (openTime < closeTime)
                {
                    StartHoursList.Add(new VenueDate(date: openTime));
                    // break when only one slot remaining to allow booking the last slot
                    if (closeTime - openTime == TimeSpan.FromMinutes(TimeSlotIntervalMinutes))
                    {
                        break;
                    }
                    openTime = openTime.AddMinutes(TimeSlotIntervalMinutes);
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// When ever new date is selected the new timing list is created in SetTimingList().
    /// The SetTimingList() calls SetSelectedStartTime().
    /// if the venue is open 24 hours set the selected time to current time.
    /// if the selected date is today and the venue closes on the next day set the selected time to the open time of the venue.
    /// if the selected date is today and the venue closes on the same day set the selected time to the open time of the venue.
    /// if the selected date is greater than today set the selected time to the open time of the venue.
    /// finally calls OnTimeChanged method
    /// </summary>
    /// <param name="openTime"></param>
    /// <param name="closeTime"></param>
    /// <returns></returns>
    private async Task SetSelectedStartTime()
    {
        try
        {
            DateTime selectedTime = DetermineInitialStartTime();

            await OnTimeChanged(selectedTime);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Determines the initial start time based on venue type and current time
    /// </summary>
    private DateTime DetermineInitialStartTime()
    {
        // 24-hour venue
        if (VenueOpenTime == VenueCloseTime)
        {
            return SelectedDate?.Date.Date == DateTime.Today
                ? RoundToNearestSlot(DateTime.Now)
                : VenueOpenTime;
        }

        // Future date - use venue open time
        if (SelectedDate?.Date.Date != DateTime.Today)
        {
            return VenueOpenTime;
        }

        // Today - determine based on current time relative to venue hours
        return DetermineStartTimeForToday();
    }

    /// <summary>
    /// Determines the start time when booking for today
    /// </summary>
    private DateTime DetermineStartTimeForToday()
    {
        var now = DateTime.Now;
        bool venueClosesNextDay = VenueCloseTime.TimeOfDay < VenueOpenTime.TimeOfDay;

        // Check if current time is within venue operating hours
        bool isWithinOperatingHours = venueClosesNextDay
            ? (now >= VenueOpenTime && now < VenueCloseTime)
            : (
                now.TimeOfDay >= VenueOpenTime.TimeOfDay && now.TimeOfDay < VenueCloseTime.TimeOfDay
            );

        if (isWithinOperatingHours)
        {
            return GetNextAvailableSlot();
        }

        // Before opening - use open time
        if (now.TimeOfDay < VenueOpenTime.TimeOfDay)
        {
            return VenueOpenTime;
        }

        // After closing - use close time (edge case)
        return VenueCloseTime;
    }

    /// <summary>
    /// Gets the next available time slot rounded up from current time
    /// </summary>
    private DateTime GetNextAvailableSlot()
    {
        int hour = DateTime.Now.Hour;
        int minute = VenueOpenTime.Minute;

        if (minute == 30)
        {
            minute = 0;
            hour++;
        }
        else
        {
            minute = 30;
        }

        return BuildDateTime(DateTime.Today, hour, minute);
    }

    /// <summary>
    /// increase the booking duration by 30 minutes if the booking duration is less than 180 minutes and the end time of the booking is less than the close time of the venue. finally calls SelectFieldsWhereSlotIsFree method
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task AddTime()
    {
        try
        {
            if (BookingDuration < MaxBookingDurationMinutes)
            {
                double requestedBookingDuration = BookingDuration + TimeSlotIntervalMinutes;
                if (
                    SelectedStartTime.Date.DateTime.AddMinutes(requestedBookingDuration)
                    <= VenueCloseTime
                )
                {
                    BookingDuration += TimeSlotIntervalMinutes;
                    await SelectFieldsWhereSlotIsFree();
                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Out of Range",
                        "The selected time exceeds the venue's closing time",
                        "OK"
                    );
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// decrease the booking duration by 30 minutes if the booking duration is greater than 60 minutes. finally calls SelectFieldsWhereSlotIsFree method
    ///
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task SubtractTime()
    {
        try
        {
            if (BookingDuration > MinBookingDurationMinutes)
            {
                BookingDuration -= TimeSlotIntervalMinutes;
                await SelectFieldsWhereSlotIsFree();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task CloseMopupCommand()
    {
        try
        {
            await popupNavigation.PopAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
        }
    }

    /// <summary>
    /// this is the command which shows time popup to select the start time for the booking
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task SelectStartTime()
    {
        try
        {
            IPopupResult<VenueDate> result = await Shell.Current.ShowPopupAsync<VenueDate>(
                new HoursPopup(StartHoursList)
            );
            if (result.WasDismissedByTappingOutsideOfPopup)
            {
                return;
            }
            else if (result != null)
            {
                VenueDate time = result.Result;
                await OnTimeChanged(time.Date.DateTime);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// whenever the user select time from the time picker if the booking close time is less than the close time of the venue set the selected start time to the selected time. finally calls SelectFieldsWhereSlotIsFree method
    /// </summary>
    /// <returns></returns>
    async Task OnTimeChanged(DateTime selectedTime)
    {
        try
        {
            int addDays = VenueCloseTime < VenueOpenTime ? 1 : 0;
            DateTime requestedBookingStartTime = BuildDateTime(
                SelectedDate.Date,
                selectedTime.Hour,
                selectedTime.Minute,
                addDays
            );

            // Validate booking time for today
            if (SelectedDate.Date.Date == DateTime.Now.Date)
            {
                DateTime todayTime = BuildDateTime(
                    SelectedDate.Date,
                    DateTime.Now.Hour,
                    DateTime.Now.Minute
                );

                if (todayTime - selectedTime > TimeSpan.FromMinutes(AllowedPastBookingMinutes))
                {
                    await Shell.Current.DisplayAlert(
                        "Out of Range",
                        $"Booking is allowed only up to {AllowedPastBookingMinutes} minutes before the current time",
                        "OK"
                    );
                    return;
                }
            }

            DateTime requestedBookingEndTime = requestedBookingStartTime.AddMinutes(
                BookingDuration
            );

            if (requestedBookingEndTime <= VenueCloseTime || VenueCloseTime == VenueOpenTime)
            {
                SelectedStartTime = new VenueDate(date: selectedTime);
                OnPropertyChanged(nameof(SelectedStartTime));
                await SelectFieldsWhereSlotIsFree();
            }
            else
            {
                SelectedDate = new VenueDate(SelectedDate.Date.AddDays(1));
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// Returns the effective list of fields for the current sport flow.
    /// For multi-sport capable sports (e.g., Pickleball), this merges the sport's own fields
    /// with multi-sport courts (Courts 19-22) sourced from other games at the venue.
    /// </summary>
    private IEnumerable<Field> GetEffectiveFields()
    {
        IList<Field>? sportFields = SelectedGame?.Fields;
        if (sportFields == null || SelectedVenue == null)
            return Enumerable.Empty<Field>();

        if (string.IsNullOrEmpty(GameName) || !AppConstants.MultiSportOptions.Contains(GameName))
            return sportFields;

        HashSet<int> existingFieldNumbers = new HashSet<int>(sportFields.Select(f => f.FieldNumber));
        int[] missingMultiSportCourts = AppConstants.MultiSportCourtNumbers
            .Where(n => !existingFieldNumbers.Contains(n))
            .ToArray();

        if (missingMultiSportCourts.Length == 0)
            return sportFields;

        List<Field> mergedFields = new List<Field>(sportFields);
        foreach (AvailableGame otherGame in SelectedVenue.AvailableGames)
        {
            if (otherGame.Name == GameName)
                continue;

            foreach (Field field in otherGame.Fields)
            {
                if (missingMultiSportCourts.Contains(field.FieldNumber)
                    && !mergedFields.Any(f => f.FieldNumber == field.FieldNumber))
                {
                    mergedFields.Add(field);
                }
            }
        }

        return mergedFields;
    }

    /// <summary>
    /// each field for the selected game and selected venue is added to the AvailableFields list if the field is not booked in the selected time range. finally calls OnPropertyChanged method to update the AvailableFields collection. if there are no available fields display a message "All fields are full in the selected date and time".
    /// </summary>
    /// <returns></returns>
    private async Task SelectFieldsWhereSlotIsFree()
    {
        try
        {
            IEnumerable<Field>? AllFields = GetEffectiveFields();

            SelectedFields?.Clear();
            AvailableFields?.Clear();
            // this list is used to store the fields which are available in the selected time range and finally set the AvailableFields property to this list. We can't directly set the AvailableFields property because the UI on iOS does not show ordered list.
            List<Field> tempFields = [];

            if (AllFields == null || !AllFields.Any())
            {
                return;
            }
            // to check if the selected time range is already booked, reserved or saved by the user in his cart for the selected field before adding to the available fields list
            foreach (Field field in AllFields)
            {
                ObjectId selectedVenueId = SelectedVenue.Id;
                string selectedFieldName = field.FieldName;
                DateTime selectedDate = SelectedDate.Date.Date;
                DateTime selectedStartTime = SelectedStartTime.Date.DateTime;
                DateTime selectedEndTime = selectedStartTime.AddMinutes(BookingDuration);

                bool isSlotAvailable = await addToCartService.IsSlotAvailableAsync(
                    selectedVenueId,
                    selectedFieldName,
                    selectedDate,
                    selectedStartTime,
                    selectedEndTime
                );

                if (isSlotAvailable)
                {
                    tempFields.Add(field);
                }
            }

            if (tempFields.Count > 0)
            {
                AvailableFields = tempFields.OrderBy(m => m.FieldNumber).ToObservableCollection();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// when user selects a field set IsSelected Property of all the fields in the AvailableFields to false and then set IsSelected property of all the selected fields to true
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task FieldSelected()
    {
        try
        {
            int? fieldsCount = SelectedFields?.Count;
            AvailableFields
                ?.ToList()
                .ForEach(field =>
                {
                    field.IsSelected = false;
                });
            if (fieldsCount > 0)
            {
                foreach (Field field in SelectedFields.Cast<Field>())
                {
                    field.IsSelected = true;
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// save the selected slot as booking in the Booking table with the booking status to saved. finally call SelectFieldsWhereSlotIsFree method and display a message "your selection added to cart"
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task BookNow()
    {
        if (SelectedFields.Count == 0)
        {
            await Shell.Current.DisplayAlert(
                "No fields",
                "Select minimum one field for a booking",
                "OK"
            );
            return;
        }

        List<NewBooking?> selectedBookings = new List<NewBooking?>();
        try
        {
            decimal peakHoursRate = 0;
            decimal peakHoursDuration = 0;
            decimal nonPeakHoursRate = 0;
            decimal nonPeakHoursDuration = 0;

            (
                decimal peakHoursRate,
                decimal peakHoursDuration,
                decimal nonPeakHoursRate,
                decimal nonPeakHoursDuration
            )? peakHourData = GetRateAndDuration();

            int year = SelectedDate.Date.Year;
            int month = SelectedDate.Date.Month;
            int day = SelectedDate.Date.Day;
            TimeOnly startTime = new TimeOnly(
                SelectedStartTime.Date.Hour,
                SelectedStartTime.Date.Minute
            );

            if (peakHourData != null)
            {
                peakHoursRate = peakHourData.Value.peakHoursRate;
                peakHoursDuration = peakHourData.Value.peakHoursDuration / 60;
                nonPeakHoursRate = peakHourData.Value.nonPeakHoursRate;
                nonPeakHoursDuration = peakHourData.Value.nonPeakHoursDuration / 60;
            }

            decimal peakHoursPrice = peakHoursRate * peakHoursDuration;
            decimal nonPeakHoursPrice = nonPeakHoursRate * nonPeakHoursDuration;

            TimeOnly endTime = startTime.AddMinutes(BookingDuration);

            decimal Price = peakHoursPrice + nonPeakHoursPrice;
            Charge? charge = syncRealm?.All<Charge>().FirstOrDefault();
            decimal ConPercent = charge?.ConvenienceFeePercent ?? 0;
            decimal taxPercent = charge?.TaxPercent ?? 0;
            decimal ConFee = (Price / 100) * ConPercent;
            decimal Total = Price + ConFee;
            decimal TaxAmount = (Total / 100) * taxPercent;
            decimal TotalPrice = Total + TaxAmount;

            foreach (Field field in SelectedFields.Cast<Field>())
            {
                NewBooking booking = new NewBooking(
                    _id: ObjectId.GenerateNewId().ToString(),
                    venue: SelectedVenue.Id.ToString(),
                    ownerId: SelectedVenue.Id.ToString(),
                    bookingStatus: AppConstants.Created,
                    fieldName: field.FieldName,
                    gameName: GameName,
                    price: Price,
                    pricePerHour: 0.0M,
                    totalPrice: TotalPrice,
                    convenienceFeePercentage: ConPercent,
                    taxPercentage: taxPercent,
                    convenienceFee: ConFee,
                    tax: TaxAmount,
                    priceAfterAddingConvenienceFee: Total,
                    discountPercentage: 0.0M,
                    discount: 0.0M,
                    priceAfterDeductingDiscount: 0.0M,
                    duration: 0.0M,
                    startTime: new DateTimeOffset(
                        new DateTime(year, month, day, startTime.Hour, startTime.Minute, 0),
                        TimeZoneInfo.Local.GetUtcOffset(new DateTime(year, month, day, startTime.Hour, startTime.Minute, 0))
                    ),
                    endTime: new DateTimeOffset(
                        new DateTime(year, month, day, endTime.Hour, endTime.Minute, 0),
                        TimeZoneInfo.Local.GetUtcOffset(new DateTime(year, month, day, endTime.Hour, endTime.Minute, 0))
                    ),
                    bookingDate: DateHelper.GetDate(DateTime.Now),
                    currency: "cad",
                    paymentIntentStatus: "",
                    cartId: Guid.NewGuid().ToString(),
                    peoplePerGame: SelectedGame.PeoplePerGame,
                    isHosted: false,
                    numberOfPlayersRequiredForHosting: 0,
                    isBookedByVendor: false,
                    costShared: true,
                    bringYourOwnEquipment: true,
                    playerCompetancyLevel: PlayerCompetancy.Beginner,
                    player: Gamer.Id.ToString(),
                    customerName: Gamer.FullName,
                    customerEmail: Gamer.Email,
                    customerPhone: Gamer.MobileNumber,
                    source: "mobile",
                    notes: "",
                    isFirstBookingCouponApplied: false,
                    nonPeakHoursDuration: nonPeakHoursDuration,
                    peakHoursDuration: peakHoursDuration,
                    totalDuration: nonPeakHoursDuration + peakHoursDuration,
                    nonPeakPricePerHour: nonPeakHoursRate,
                    peakPricePerHour: peakHoursRate
                );

                selectedBookings.Add(booking);
            }
        // the server use data sent in New Booking/s to create Booking.
            await CreateBookings(selectedBookings);

            SelectedFields.Clear();
            await FieldSelected();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private (
        decimal peakHoursRate,
        decimal peakHoursDuration,
        decimal nonPeakHoursRate,
        decimal nonPeakHoursDuration
    )? GetRateAndDuration()
    {
        try
        {
            TimeSpan startTime = SelectedStartTime.Date.TimeOfDay;
            TimeSpan endTime = SelectedStartTime.Date.AddMinutes(BookingDuration).TimeOfDay;
            decimal peakHoursDuration = 0;
            decimal peakHoursRate = 0;
            decimal nonPeakHoursDuration = 0;
            decimal nonPeakHoursRate = 0;

            bool useWeekendRates = !isWeekDay || IsCanadaHoliday(SelectedStartTime.Date);

            if (!useWeekendRates)
            {
                for (
                    TimeSpan time = startTime;
                    time < endTime;
                    time = time.Add(TimeSpan.FromMinutes(TimeSlotIntervalMinutes))
                )
                {
                    if (SelectedGame.WeekdayPeakHoursHourlyRate == 0)
                    {
                        nonPeakHoursDuration += TimeSlotIntervalMinutes;
                    }
                    else
                    {
                        bool isInPeakHours = IsPeakHoursInWeekday(
                            time,
                            time.Add(TimeSpan.FromMinutes(TimeSlotIntervalMinutes))
                        );

                        if (isInPeakHours)
                        {
                            peakHoursDuration += TimeSlotIntervalMinutes;
                        }
                        else
                        {
                            nonPeakHoursDuration += TimeSlotIntervalMinutes;
                        }
                    }
                }
                if (peakHoursDuration > 0)
                {
                    peakHoursRate = SelectedGame.WeekdayPeakHoursHourlyRate;
                }
                if (nonPeakHoursDuration > 0)
                {
                    nonPeakHoursRate = SelectedGame.WeekdayHourlyRate;
                }
            }
            else
            {
                for (
                    TimeSpan time = startTime;
                    time < endTime;
                    time = time.Add(TimeSpan.FromMinutes(TimeSlotIntervalMinutes))
                )
                {
                    if (SelectedGame.WeekendPeakHoursHourlyRate == 0)
                    {
                        nonPeakHoursDuration += TimeSlotIntervalMinutes;
                    }
                    else
                    {
                        bool isInPeakHours = IsPeakHoursInWeekend(
                            time,
                            time.Add(TimeSpan.FromMinutes(TimeSlotIntervalMinutes))
                        );

                        if (isInPeakHours)
                        {
                            peakHoursDuration += TimeSlotIntervalMinutes;
                        }
                        else
                        {
                            nonPeakHoursDuration += TimeSlotIntervalMinutes;
                        }
                    }
                }
                if (peakHoursDuration > 0)
                {
                    peakHoursRate = SelectedGame.WeekendPeakHoursHourlyRate;
                }
                if (nonPeakHoursDuration > 0)
                {
                    nonPeakHoursRate = SelectedGame.WeekendHourlyRate;
                }
            }

            return (peakHoursRate, peakHoursDuration, nonPeakHoursRate, nonPeakHoursDuration);
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        return null;
    }

    private bool IsPeakHoursInWeekday(TimeSpan startTime, TimeSpan endTime)
    {
        return startTime >= SelectedGame?.Timing?.WeekdayPeakHoursStartTime.TimeOfDay
            && endTime <= SelectedGame?.Timing?.WeekdayPeakHoursEndTime.TimeOfDay;
    }

    private bool IsPeakHoursInWeekend(TimeSpan startTime, TimeSpan endTime)
    {
        return startTime >= SelectedGame?.Timing?.WeekendPeakHoursStartTime.TimeOfDay
            && endTime <= SelectedGame?.Timing?.WeekendPeakHoursEndTime.TimeOfDay;
    }

    /// <summary>
    /// Calls AddToCart() method in order to save the SelectedFields, if any to the Cart and then navigate to CartView.
    /// </summary>
    /// <returns></returns>
    async Task CreateBookings(List<NewBooking> selectedBookings)
    {
        try
        {
            // validate the selected fields and only add navigate to the paymentview if the fields are available on selected time range
            if (selectedBookings.Count > 0)
            {
                await popupNavigation.PushAsync(new BusyMopup());
                CreateBookingsRequest request = new CreateBookingsRequest(
                    bookingData: selectedBookings,
                    environment: AppConstants.Environment
                );

                ValidateAvailabilityResponse? apiResponse = await stripeService.CreateBooking(
                    request
                );
                if (apiResponse.Available != null)
                {
                    Realm realm = await Realm.GetInstanceAsync(realmService.Config);
                    Task syncTask = realm.Subscriptions.WaitForSynchronizationAsync();
                    Task timeoutTask = Task.Delay(3000);
                    await Task.WhenAny(syncTask, timeoutTask);
                    await navigationService.NavigateToAsync(nameof(PaymentView));
                    await popupNavigation.PopAsync();
                }
                else
                {
                    await popupNavigation.PopAsync();
                    await Shell.Current.DisplayAlert("Error", apiResponse.Error, "OK");
                    await SelectFieldsWhereSlotIsFree();
                }
            }
            else
            {
                await Shell.Current.DisplayAlert(
                    "Error",
                    "All fields are full in the selected date and time",
                    "OK"
                );
            }
        }
        catch (Exception ex)
        {
            await popupNavigation.PopAsync();
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task NavigateBack()
    {
        try
        {
            await navigationService.PopAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}
