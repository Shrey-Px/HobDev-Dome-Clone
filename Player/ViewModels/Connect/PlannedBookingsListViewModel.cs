using System;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace Player.ViewModels.Connect;

/// <summary>
/// This class represents the view model for the PlannedBookingsList. It is used to display the planned bookings list from which the user can select a planned booking to host (connect the planned booking with the purchased booking)
/// </summary>
public partial class PlannedBookingsListViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    public Booking? purchasedBooking;

    [ObservableProperty]
    ObservableCollection<PlannedBooking> plannedBookings;

    [ObservableProperty]
    byte[]? playerImage;

    string? conversationId;

    VenueUser? player;

    Realm realm;

    IDisposable? plannedBookingsToken;

    private readonly INavigationService? navigationService;

    private readonly IRealmService? realmService;

    private readonly IConnectivity? connectivity;

    public PlannedBookingsListViewModel(
        INavigationService navigationService,
        IRealmService realmService,
        IConnectivity connectivity
    )
        : base(navigationService, realmService, connectivity)
    {
        try
        {
            this.navigationService = navigationService;
            this.realmService = realmService;
            this.connectivity = connectivity;

            PlannedBookings = new ObservableCollection<PlannedBooking>();

            realm = Realm.GetInstance(realmService.Config);

            player = realm
                .All<VenueUser>()
                .Where(g => g.OwnerId == realmService.RealmUser.Id)
                .FirstOrDefault();

            realm = Realm.GetInstance(realmService.Config);

            plannedBookingsToken = realm
                .All<PlannedBooking>()
                .SubscribeForNotifications(
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

                            await Initialize();
                            return;
                        }
                        foreach (int i in changes.InsertedIndices)
                        {
                            // ... handle insertions ...

                            await Initialize();
                            return;
                        }
                        foreach (int i in changes.NewModifiedIndices)
                        {
                            // ... handle modifications ...
                            await Initialize();
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

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            if (query.TryGetValue("Id", out object? id))
            {
                if (id != null)
                {
                    ObjectId objectId = ObjectId.Parse(id.ToString());
                    PurchasedBooking = realm.Find<Booking>(objectId);
                    await Initialize();
                }
            }
            ;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async Task Initialize()
    {
        try
        {
            if (player != null)
            {
                PlayerImage = player.ProfileImage;
            }
            if (PurchasedBooking != null)
            {
                PlannedBookings = player
                    .PlannedBookings.Where(g => g.ConnectedBooking == null)
                    .ToObservableCollection();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task Connect(PlannedBooking plannedBooking)
    {
        try
        {
            await realm.WriteAsync(() =>
            {
                PurchasedBooking.ConnectedPlannedBooking = plannedBooking;
                plannedBooking.ConnectedBooking = PurchasedBooking;
                PurchasedBooking.ConversationId = plannedBooking.ConversationId;
                PurchasedBooking.IsHosted = true;
                PurchasedBooking.NumberOfPlayersRequiredForHosting = plannedBooking.NumberOfPlayers;
            });

            await NavigateBack();
        }
        catch (Exception ex)
        {
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
