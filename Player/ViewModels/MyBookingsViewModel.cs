using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using Stripe;

namespace Player.ViewModels
{
    public partial class MyBookingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        ObservableCollection<PlannedBooking> plannedBookings;

        [ObservableProperty]
        ObservableCollection<byte[]> plannedBookingImages;

        [ObservableProperty]
        ObservableCollection<Booking> upComingBookings;

        [ObservableProperty]
        ObservableCollection<byte[]> upComingBookingImages;

        [ObservableProperty]
        ObservableCollection<Booking> pastBookings;

        [ObservableProperty]
        ObservableCollection<byte[]> pastBookingImages;

        [ObservableProperty]
        ObservableCollection<Booking> cancelledBookings;

        [ObservableProperty]
        ObservableCollection<byte[]> cancelledBookingImages;

        [ObservableProperty]
        Booking? selectedBooking;

        [ObservableProperty]
        int selectedIndex;

        [ObservableProperty]
        string? userName;

        readonly Realm realm;

        VenueUser? Player;

        readonly IDisposable bookingsToken;

        readonly IDisposable joinedBookingsToken;

        readonly IDisposable plannedBookingsToken;

        readonly IDisposable appliedBookings;

        IDisposable? emailBookingsToken;

        private readonly IImageService imageService;
        private readonly IPopupNavigation popupNavigation;
        private readonly IRealmService realmService;
        private readonly INavigationService navigationService;
        private readonly IStripeService stripeService;

        public MyBookingsViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            IImageService imageService,
            IPopupNavigation popupNavigation,
            IStripeService stripeService
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.imageService = imageService;
                this.popupNavigation = popupNavigation;
                this.stripeService = stripeService;

                PlannedBookings = new ObservableCollection<PlannedBooking>();
                UpComingBookings = new ObservableCollection<Booking>();
                PastBookings = new ObservableCollection<Booking>();
                CancelledBookings = new ObservableCollection<Booking>();

                this.realmService = realmService;
                this.navigationService = navigationService;

                realm = Realm.GetInstance(realmService.Config);

                Player = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                // Observe collection notifications
                bookingsToken = Player.Bookings.SubscribeForNotifications(
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

                            await FilterBookings();

                            return;
                        }
                        foreach (int i in changes.InsertedIndices)
                        {
                            // ... handle insertions ...

                            await FilterBookings();
                            return;
                        }
                        foreach (int i in changes.NewModifiedIndices)
                        {
                            // ... handle modifications ...
                            await FilterBookings();
                            return;
                        }
                    }
                );

                // Observe collection notifications
                joinedBookingsToken = Player.JoinedBookings.SubscribeForNotifications(
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

                            await FilterBookings();

                            return;
                        }
                        foreach (int i in changes.InsertedIndices)
                        {
                            // ... handle insertions ...

                            await FilterBookings();
                            return;
                        }
                        foreach (int i in changes.NewModifiedIndices)
                        {
                            // ... handle modifications ...
                            await FilterBookings();
                            return;
                        }
                    }
                );

                // Observe collection notifications
                plannedBookingsToken = Player.PlannedBookings.SubscribeForNotifications(
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

                            await FilterBookings();

                            return;
                        }
                        foreach (int i in changes.InsertedIndices)
                        {
                            // ... handle insertions ...

                            await FilterBookings();
                            return;
                        }
                        foreach (int i in changes.NewModifiedIndices)
                        {
                            // ... handle modifications ...
                            await FilterBookings();
                            return;
                        }
                    }
                );

                // Observe collection notifications
                appliedBookings = Player.AppliedBookings.SubscribeForNotifications(
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

                            await FilterBookings();

                            return;
                        }
                        foreach (int i in changes.InsertedIndices)
                        {
                            // ... handle insertions ...

                            await FilterBookings();
                            return;
                        }
                        foreach (int i in changes.NewModifiedIndices)
                        {
                            // ... handle modifications ...
                            await FilterBookings();
                            return;
                        }
                    }
                );

                // Observe bookings matched by customerEmail for web-created bookings
                if (!string.IsNullOrEmpty(Player?.Email))
                {
                    string playerEmail = Player.Email.ToLowerInvariant();
                    IQueryable<Booking> emailBookings = realm
                        .All<Booking>()
                        .Where(b => b.CustomerEmail == playerEmail);

                    emailBookingsToken = emailBookings.AsRealmCollection().SubscribeForNotifications(
                        async (sender, changes) =>
                        {
                            if (changes == null)
                                return;

                            await FilterBookings();
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public async Task LoadData()
        {
            try
            {
                UserName = Player?.FirstName;
                PlayerImage = Player?.ProfileImage;
                await FilterBookings();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        async Task FilterBookings()
        {
            try
            {
                ObservableCollection<JoinRequest> joinRequests =
                    Player.AppliedBookings.ToObservableCollection();
                PlannedBookings = Player.PlannedBookings.ToObservableCollection();
                foreach (JoinRequest joinRequest in joinRequests)
                {
                    PlannedBooking plannedBooking = (PlannedBooking)joinRequest.Parent;
                    plannedBooking.IsApplied = true;
                    if (joinRequest.IsApproved == true)
                    {
                        plannedBooking.IsApproved = true;
                    }
                    PlannedBookings.Add(plannedBooking);
                }
                PlannedBookings = PlannedBookings
                    .OrderByDescending(d => d.PlannedDate)
                    .ToObservableCollection();

                // Collect email-matched bookings (web-created bookings that may not have the player link set)
                IEnumerable<Booking> emailMatchedBookings = Enumerable.Empty<Booking>();
                if (!string.IsNullOrEmpty(Player?.Email))
                {
                    string playerEmail = Player.Email.ToLowerInvariant();
                    emailMatchedBookings = realm
                        .All<Booking>()
                        .Where(b => b.CustomerEmail == playerEmail);
                }

                // --- Upcoming ---
                IEnumerable<Booking> upComingJoinedBookings = Player.JoinedBookings.Where(b =>
                    b.BookingStatus == Dome.Shared.Constants.AppConstants.Booked
                );
                UpComingBookings = Player
                    .Bookings.Where(b => b.BookingStatus == AppConstants.Booked)
                    .ToObservableCollection();
                HashSet<ObjectId> upcomingIds = new HashSet<ObjectId>(UpComingBookings.Select(b => b.Id));
                foreach (Booking booking in upComingJoinedBookings)
                {
                    if (!upcomingIds.Contains(booking.Id))
                    {
                        booking.IsJoined = true;
                        UpComingBookings.Add(booking);
                        upcomingIds.Add(booking.Id);
                    }
                }
                foreach (Booking booking in emailMatchedBookings.Where(b => b.BookingStatus == AppConstants.Booked))
                {
                    if (!upcomingIds.Contains(booking.Id))
                    {
                        UpComingBookings.Add(booking);
                        upcomingIds.Add(booking.Id);
                    }
                }
                UpComingBookings = UpComingBookings
                    .OrderByDescending(d => d.StartTime)
                    .ToObservableCollection();

                // --- Past ---
                IEnumerable<Booking> pastJoinedBookings = Player.JoinedBookings.Where(b =>
                    b.BookingStatus == Dome.Shared.Constants.AppConstants.Completed
                );
                PastBookings = Player
                    .Bookings.Where(b => b.BookingStatus == AppConstants.Completed)
                    .ToObservableCollection();
                HashSet<ObjectId> pastIds = new HashSet<ObjectId>(PastBookings.Select(b => b.Id));
                foreach (Booking booking in pastJoinedBookings)
                {
                    if (!pastIds.Contains(booking.Id))
                    {
                        booking.IsJoined = true;
                        PastBookings.Add(booking);
                        pastIds.Add(booking.Id);
                    }
                }
                foreach (Booking booking in emailMatchedBookings.Where(b => b.BookingStatus == AppConstants.Completed))
                {
                    if (!pastIds.Contains(booking.Id))
                    {
                        PastBookings.Add(booking);
                        pastIds.Add(booking.Id);
                    }
                }
                PastBookings = PastBookings
                    .OrderByDescending(d => d.StartTime)
                    .ToObservableCollection();

                // --- Cancelled ---
                IEnumerable<Booking> cancelledJoinedBookings = Player.JoinedBookings.Where(b =>
                    b.BookingStatus == Dome.Shared.Constants.AppConstants.Cancelled
                );
                CancelledBookings = Player
                    .Bookings.Where(b => b.BookingStatus == AppConstants.Cancelled)
                    .ToObservableCollection();
                HashSet<ObjectId> cancelledIds = new HashSet<ObjectId>(CancelledBookings.Select(b => b.Id));
                foreach (Booking booking in cancelledJoinedBookings)
                {
                    if (!cancelledIds.Contains(booking.Id))
                    {
                        booking.IsJoined = true;
                        CancelledBookings.Add(booking);
                        cancelledIds.Add(booking.Id);
                    }
                }
                foreach (Booking booking in emailMatchedBookings.Where(b => b.BookingStatus == AppConstants.Cancelled))
                {
                    if (!cancelledIds.Contains(booking.Id))
                    {
                        CancelledBookings.Add(booking);
                        cancelledIds.Add(booking.Id);
                    }
                }
                CancelledBookings = CancelledBookings
                    .OrderByDescending(d => d.StartTime)
                    .ToObservableCollection();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        /// <summary>
        /// Navigate to the team view when a booking is selected from the list of Purchased upcoming bookings
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task SelectionChanged()
        {
            if (SelectedBooking != null)
            {
                ShellNavigationQueryParameters query = new ShellNavigationQueryParameters
                {
                    { "booking", SelectedBooking },
                };
                await navigationService.NavigateToAsync(nameof(TeamView), query);
            }

            SelectedBooking = null;
        }

        /// <summary>
        ///  Navigate to the join requests view when the review requests button is clicked from the planned booking
        /// </summary>
        /// <param name="plannedBooking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task ReviewRequests(PlannedBooking plannedBooking)
        {
            try
            {
                if (plannedBooking != null)
                {
                    await navigationService.NavigateToAsync(
                        nameof(JoinRequestsView),
                        new ShellNavigationQueryParameters
                        {
                            { "plannedBookingId", plannedBooking.Id },
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

       

        /// <summary>
        /// Connect the game with the Planned Booking.
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task ShareGame(Booking booking)
        {
            try
            {
                await navigationService.NavigateToAsync(
                    nameof(PlannedBookingsListView),
                    new ShellNavigationQueryParameters { { "Id", booking.Id } }
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        /// <summary>
        /// Quit the game when the user is joined in the game
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task QuitGame(Booking booking)
        {
            try
            {
                bool result = await Shell.Current.DisplayAlert(
                    "Quit Game",
                    "Are you sure you want to quit the game?",
                    "Yes",
                    "No"
                );
                if (result)
                {
                    await realm.WriteAsync(() =>
                    {
                        booking.JoinedPlayers.Remove(Player);
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        

        /// <summary>
        /// Cancel the upcoming purchased booking and initiate refund
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task CancelBooking(Booking booking)
        {
            try
            {
                if (booking.AppliedCoupon != null || booking.IsFirstBookingCouponApplied)
                {
                    await Shell.Current.DisplayAlert(
                        "Error",
                        "You cannot cancel a discounted booking",
                        "Okay"
                    );
                    return;
                }

                bool result = await Shell.Current.DisplayAlert(
                    "Hey, Wait!!!",
                    "Are you sure you want to cancel this \n booking?",
                    "Yes",
                    "No"
                );

                if (result)
                {
                    // call api to cancel booking and initiate refund
                    string idInString = booking.Id.ToString();
                    List<string> bookingIdsInString = new List<string> { idInString };

                    RequestWithBookingIds request = new RequestWithBookingIds(
                        bookingIds: bookingIdsInString,
                        environment: AppConstants.Environment
                    );

                    CancelBookingResponse? cancelBookingResponse =
                        await stripeService.CancelBooking(request);

                    if (cancelBookingResponse.BookingIds != null)
                    {
                        await Shell.Current.DisplayAlert(
                            "Booking cancelled and refund has been initiated",
                            "",
                            "OK"
                        );
                    }
                    else if (cancelBookingResponse.BookingIds == null)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                            Shell.Current.DisplayAlert(
                                "API Error",
                                cancelBookingResponse?.Error,
                                "OK"
                            )
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        /// <summary>
        /// Cancel the planned booking
        /// </summary>
        /// <param name="plannedBooking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task CancelPlannedBooking(PlannedBooking plannedBooking)
        {
            try
            {
                bool result = await Shell.Current.DisplayAlert(
                    "Cancel Game",
                    "Are you sure you want to cancel the game?",
                    "Yes",
                    "No"
                );
                if (result)
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Remove(plannedBooking);
                    });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        /// <summary>
        /// Quit the planned booking
        /// </summary>
        /// <param name="plannedBooking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task QuitPlannedBooking(PlannedBooking plannedBooking)
        {
            try
            {
                bool result = await Shell.Current.DisplayAlert(
                    "Quit Game",
                    "Are you sure you want to quit the game?",
                    "Yes",
                    "No"
                );
                if (result)
                {
                    JoinRequest? joinRequest = plannedBooking
                        .JoinRequests.Where(j => j.AppliedBy == Player)
                        .FirstOrDefault();
                    if (joinRequest != null)
                    {
                        await realm.WriteAsync(() =>
                        {
                            plannedBooking.JoinRequests.Remove(joinRequest);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        /// <summary>
        /// Navigate to the chat view from the planned booking
        /// </summary>
        /// <param name="plannedBooking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task ChatFromPlannedBooking(PlannedBooking plannedBooking)
        {
            try
            {
                if (plannedBooking.IsApplied && !plannedBooking.IsApproved)
                {
                    await Shell.Current.DisplayAlert(
                        "Not approved",
                        "You can only chat with the team if your application is approved",
                        "OK"
                    );
                }
                else
                {
                    await navigationService.NavigateToAsync(
                        nameof(ChatView),
                        new ShellNavigationQueryParameters
                        {
                            { "plannedBookingId", plannedBooking.Id },
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        /// <summary>
        /// Navigate to the chat view from the purchased booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task ChatFromPurchasedBooking(Booking booking)
        {
            try
            {
                await navigationService.NavigateToAsync(
                    nameof(ChatView),
                    new ShellNavigationQueryParameters { { "purchasedBookingId", booking.Id } }
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }
    }
}
