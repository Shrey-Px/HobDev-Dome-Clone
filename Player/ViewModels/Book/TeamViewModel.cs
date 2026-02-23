using Microsoft.Maui.Networking;
using Realms;

namespace Player.ViewModels.Book
{
    public partial class TeamViewModel : BaseViewModel, IQueryAttributable
    {
        [ObservableProperty]
        Booking? selectedBooking;

        [ObservableProperty]
        string? playerCount;

        [ObservableProperty]
        string? nameAndLevel;

        [ObservableProperty]
        string? otherDetails;

        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        string? userName;

        [ObservableProperty]
        int? collectionHeight;

        VenueUser? gamer;

        Realm realm;

        private readonly INavigationService? navigationService;

        public TeamViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.navigationService = navigationService;
                realm = Realm.GetInstance(realmService.Config);
                gamer = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();
                UserName = gamer?.FirstName;
                PlayerImage = gamer?.ProfileImage;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            try
            {
                query.TryGetValue("booking", out object? booking);
                if (booking is Booking)
                {
                    SelectedBooking = booking as Booking;
                    await LoadPlayers();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async Task LoadPlayers()
        {
            try
            {
                // +1 for host of the game who has booked the game
                PlayerCount = $"{SelectedBooking?.JoinedPlayers.Count + 1}";
                NameAndLevel =
                    $"{SelectedBooking?.GameName} | {SelectedBooking?.PlayerCompetancyLevel}";
                OtherDetails =
                    $"{SelectedBooking?.VenueName}  | {SelectedBooking?.Venue.Address.City}";
                int? joinedPlayers = SelectedBooking?.JoinedPlayers.Count;
                CollectionHeight = (SelectedBooking?.JoinedPlayers.Count + 1) * 130;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        [RelayCommand]
        public async Task Close()
        {
            try
            {
                await navigationService.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}
