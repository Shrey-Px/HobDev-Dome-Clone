using System;
using Player.Models.Chat;

namespace Player.ViewModels.Connect;

public partial class ChatViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    public PlannedBooking? futureBooking;

    [ObservableProperty]
    public Booking? purchasedBooking;

    [ObservableProperty]
    string? venueName;

    [ObservableProperty]
    string? bookingInformation;

    [ObservableProperty]
    bool isHost;

    [ObservableProperty]
    byte[]? playerImage;

    [ObservableProperty]
    string? text;

    [ObservableProperty]
    bool displayJoinButton;

    [ObservableProperty]
    string? cityProvince;

    [ObservableProperty]
    ObservableCollection<LoadMessagesResponse>? messages;

    //string? conversationId;

    string? userEmail;

    VenueUser player;

    PeriodicTimer? timer;

    Realm realm;

    private readonly INavigationService navigationService;

    private readonly IRealmService realmService;

    private readonly IConnectivity connectivity;

    private readonly IChatService chatService;

    private readonly IPopupNavigation popupNavigation;

    public ChatViewModel(
        INavigationService navigationService,
        IRealmService realmService,
        IConnectivity connectivity,
        IChatService chatService,
        IPopupNavigation popupNavigation
    )
        : base(navigationService, realmService, connectivity)
    {
        this.navigationService = navigationService;
        this.realmService = realmService;
        this.connectivity = connectivity;
        this.chatService = chatService;
        this.popupNavigation = popupNavigation;

        Messages = new ObservableCollection<LoadMessagesResponse>();

        realm = Realm.GetInstance(realmService.Config);

        player = realm.All<VenueUser>().First(x => x.OwnerId == realmService.RealmUser.Id);
        userEmail = realmService.RealmUser.Profile.Email;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            if (query.TryGetValue("plannedBookingId", out object? id))
            {
                if (id != null)
                {
                    ObjectId objectId = ObjectId.Parse(id.ToString());
                    FutureBooking = realm.Find<PlannedBooking>(objectId);
                    await Initialize();
                }
            }
            if (query.TryGetValue("purchasedBookingId", out object? bookingId))
            {
                if (bookingId != null)
                {
                    ObjectId objectId = ObjectId.Parse(bookingId.ToString());
                    PurchasedBooking = realm.Find<Booking>(objectId);
                    await Initialize();
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public async Task Initialize()
    {
        try
        {
            if (FutureBooking != null)
            {
                VenueName = FutureBooking.SelectedVenue.FullName;
                string date = FutureBooking.PlannedDate.ToString("dd, MMM, yyyy");
                string timing = FutureBooking.TimingDisplay;
                string playerCount = FutureBooking.NumberOfPlayers.ToString();
                BookingInformation = $"{date} | {timing} | {playerCount} players";
                IsHost = FutureBooking.Host.OwnerId == realmService.RealmUser.Id;
                // conversationId= FutureBooking.ConversationId;
                if (!IsHost && FutureBooking.ConnectedBooking != null)
                {
                    DisplayJoinButton = true;
                    PurchasedBooking = FutureBooking.ConnectedBooking;
                    CityProvince =
                        $"{PurchasedBooking.Venue.Address.City}, {PurchasedBooking.Venue.Address.Province}";
                }
            }
            else if (PurchasedBooking != null)
            {
                VenueName = PurchasedBooking.Venue.FullName;
                string date = PurchasedBooking.StartTime.ToString("dd, MMM, yyyy");
                string startTime = PurchasedBooking.StartTime.ToString("HH:mm");
                string endTime = PurchasedBooking.EndTime.ToString("HH:mm");
                string timing = $"{startTime} - {endTime}";
                string playerCount = PurchasedBooking.NumberOfPlayersRequiredForHosting.ToString();
                BookingInformation = $"{date} | {timing} | {playerCount} players";
                IsHost = PurchasedBooking.Player.OwnerId == realmService.RealmUser.Id;
                // conversationId= PurchasedBooking.ConversationId;
            }

            if (player != null)
            {
                PlayerImage = player.ProfileImage;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public async Task LoadMessages()
    {
        try
        {
            //a new conversation Id is created whenever a user joins a game in JoinRequestsViewModel
            string conversationId = string.Empty;
            if (FutureBooking != null)
            {
                conversationId = FutureBooking.ConversationId;
            }
            else if (PurchasedBooking != null)
            {
                conversationId = PurchasedBooking.ConversationId;
            }
            if (!string.IsNullOrWhiteSpace(conversationId))
            {
                timer ??= new(TimeSpan.FromSeconds(3));

                while (await timer.WaitForNextTickAsync())
                {
                    List<LoadMessagesResponse?> apiResponse = await chatService.LoadMessages(
                        conversationId
                    );
                    if (apiResponse == null)
                    {
                        return;
                    }
                    // because every api call returns the entire list of messages, we need to keep track of the messages that have already been loaded and only load the new messages
                    int localMessageCount = Messages.Count;
                    int remoteMessageCount = apiResponse.Count;
                    DateTime previousDate = DateTime.Today;
                    while (localMessageCount < remoteMessageCount)
                    {
                        LoadMessagesResponse remoteMessage = apiResponse.ElementAt(
                            localMessageCount
                        );
                        DateTime messageDate = DateTime.Parse(remoteMessage.TimeStamp);
                        remoteMessage.TimeSend = messageDate.ToString("HH:mm");
                        if (
                            messageDate.Date < DateTime.Today
                            && (
                                previousDate.Date == DateTime.Today
                                || previousDate.Date != messageDate.Date
                            )
                        )
                        {
                            previousDate = messageDate;
                            remoteMessage.DateSend = messageDate.ToString("dd, MMM, yyyy");
                        }
                        else if (
                            messageDate.Date == DateTime.Today
                            && previousDate.Date != DateTime.Today
                        )
                        {
                            previousDate = messageDate;
                            remoteMessage.DateSend = "Today";
                        }
                        else
                        {
                            remoteMessage.DateSend = string.Empty;
                        }
                        if (remoteMessage.Author == userEmail)
                        {
                            remoteMessage.IsMine = true;
                        }

                        remoteMessage.AuthorName = realm
                            .All<VenueUser>()
                            .Where(x => x.Email == remoteMessage.Author)
                            .First()
                            .FullName;

                        Messages.Insert(localMessageCount, remoteMessage);
                        localMessageCount++;
                    }
                }
                ;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public async Task Unload()
    {
        try
        {
            timer?.Dispose();
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    [RelayCommand]
    public async Task JoinGame()
    {
        try
        {
            if (PurchasedBooking != null)
            {
                if (PurchasedBooking.JoinedPlayers.Contains(player))
                {
                    await Shell.Current.DisplayAlert(
                        "Joined",
                        "You have already joined this game",
                        "OK"
                    );
                }
                else
                {
                    await popupNavigation.PushAsync(new ReviewGameBeforeJoiningPopup(this));
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// This method is called when the user clicks on the join button in the ReviewGameBeforeJoiningPopup. The user is added to the list of joined players in the shared booking
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    async Task Join()
    {
        try
        {
            Booking booking = FutureBooking.ConnectedBooking;
            // one seat is reserved for the host so the joined players should be equal to the number of players required for hosting minus 1
            if (booking.NumberOfPlayersRequiredForHosting - 1 == booking.JoinedPlayers.Count)
            {
                await popupNavigation.PopAllAsync();
                await Shell.Current.DisplayAlert(
                    "Seats Full",
                    "the required number of players have already joined",
                    "OK"
                );
            }
            else
            {
                await realm.WriteAsync(() =>
                {
                    booking.JoinedPlayers.Add(player);
                });
                await popupNavigation.PopAllAsync();
            }
        }
        catch (Exception ex)
        {
            await popupNavigation.PopAllAsync();
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task Cancel()
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

    [RelayCommand]
    public async Task Review()
    {
        try
        {
            if (FutureBooking != null)
            {
                await navigationService.NavigateToAsync(
                    nameof(JoinRequestsView),
                    new ShellNavigationQueryParameters { { "plannedBookingId", FutureBooking.Id } }
                );
            }
            else if (PurchasedBooking != null)
            {
                await navigationService.NavigateToAsync(
                    nameof(JoinRequestsView),
                    new ShellNavigationQueryParameters
                    {
                        { "purchasedBookingId", PurchasedBooking.Id },
                    }
                );
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    [RelayCommand]
    public async Task SendMessage()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                //a new conversation Id is created whenever a user joins a game in JoinRequestsViewModel
                string conversationId = string.Empty;
                if (FutureBooking != null)
                {
                    conversationId = FutureBooking.ConversationId;
                }
                else if (PurchasedBooking != null)
                {
                    conversationId = PurchasedBooking.ConversationId;
                }
                SendMessageResponse? apiResponse = await chatService.SendMessage(
                    userEmail,
                    conversationId,
                    Text
                );
                if (apiResponse != null)
                {
                    if (apiResponse.Status == "Message sent")
                    {
                        Text = string.Empty;
                    }
                }
            }
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
