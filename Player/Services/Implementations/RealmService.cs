namespace Player.Services.Implementations
{
    public class RealmService : IRealmService
    {
        public FlexibleSyncConfiguration? Config { get; }

        public User? RealmUser { get; }

        public RealmService(INavigationService navigationService)
        {
            RealmUser = App.RealmApp.CurrentUser;
            if (
                RealmUser == null
                || RealmUser.State == UserState.Removed
                || RealmUser.State == UserState.LoggedOut
            )
            {
                navigationService.NavigateToAsync(nameof(LoginView));
                return;
            }
            else
            {
                Config ??= new FlexibleSyncConfiguration(RealmUser)
                {
                    // if the below property is set to true, the app will cancel all async operations when a non-fatal error occurs. If set to false the app will continue to try to connect to the server
                    CancelAsyncOperationsOnNonFatalErrors = true,
                    SchemaVersion = 0,

                    PopulateInitialSubscriptions = (realm) =>
                    {
                        IQueryable<Province> provinces = realm.All<Province>();
                        IQueryable<Game> games = realm.All<Game>();
                        IQueryable<LearningContent> learningContents = realm.All<LearningContent>();
                        IQueryable<Coach> coaches = realm.All<Coach>();
                        IQueryable<Amenity> amenities = realm.All<Amenity>();
                        IQueryable<Venue> venues = realm
                            .All<Venue>()
                            .Where(c => c.IsActive == true);
                        IQueryable<VenueUser> players = realm.All<VenueUser>();
                        IQueryable<Booking> bookings = realm.All<Booking>();
                        IQueryable<Charge> serviceCharges = realm.All<Charge>();
                        IQueryable<Coupon> coupons = realm.All<Coupon>();
                        IQueryable<PlannedBooking> plannedBookings = realm.All<PlannedBooking>();

                        realm.Subscriptions.Add(venues);
                        realm.Subscriptions.Add(players);
                        realm.Subscriptions.Add(games);
                        realm.Subscriptions.Add(bookings);
                        realm.Subscriptions.Add(amenities);
                        realm.Subscriptions.Add(provinces);
                        realm.Subscriptions.Add(learningContents);
                        realm.Subscriptions.Add(coaches);
                        realm.Subscriptions.Add(serviceCharges);
                        realm.Subscriptions.Add(coupons);
                        realm.Subscriptions.Add(plannedBookings);
                    },
                };

                Config.OnSessionError = (session, sessionException) => {
                    // See https://www.mongodb.com/docs/realm-sdks/dotnet/latest/reference/Realms.Sync.Exceptions.ErrorCode.html for all of the error codes
                };

                Config.ClientResetHandler = new RecoverOrDiscardUnsyncedChangesHandler
                {
                    ManualResetFallback = (err) =>
                    {
                        if (err != null)
                        {
                            new ManualRecoveryHandler(HandleClientResetError);
                        }
                    },
                };
            }
        }

        private void HandleClientResetError(ClientResetException clientResetException)
        {
            Console.WriteLine($"Client Reset requested: {clientResetException.Message}");
            // Prompt user to perform a client reset immediately. If they don't,
            // they won't receive any data from the server until they restart the app
            // and all changes they make will be discarded when the app restarts.
            // var didUserConfirmReset = ShowUserAConfirmationDialog();
            //if (didUserConfirmReset)
            //{
            // Close the Realm before doing the reset. It must be
            // deleted as part of the reset.
            // fsRealm.Dispose();
            // perform the client reset
            var didReset = clientResetException.InitiateClientReset();
            if (didReset)
            {
                // Navigate the user back to the main page or reopen the
                // the Realm and reinitialize the current page
            }
            else
            {
                // Reset failed - notify user that they'll need to
                // update the app
            }
        }
    }
}
