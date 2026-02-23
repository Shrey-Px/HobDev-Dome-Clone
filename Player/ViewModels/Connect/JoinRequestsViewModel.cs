namespace Player.ViewModels.Connect;

public partial class JoinRequestsViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    public PlannedBooking? futureBooking;

    [ObservableProperty]
    public Booking? purchasedBooking;

    [ObservableProperty]
    ObservableCollection<JoinRequest>? acceptedRequests;

    [ObservableProperty]
    ObservableCollection<JoinRequest>? appliedRequests;

    [ObservableProperty]
    string? venueName;

    [ObservableProperty]
    string? bookingInformation;

    [ObservableProperty]
    byte[]? playerImage;

    VenueUser player;

    Realm realm;

    private readonly INavigationService navigationService;
    private readonly IRealmService realmService;
    private readonly IConnectivity connectivity;

    private readonly IChatService chatService;

    public JoinRequestsViewModel(
        INavigationService navigationService,
        IRealmService realmService,
        IConnectivity connectivity,
        IChatService chatService
    )
        : base(navigationService, realmService, connectivity)
    {
        this.navigationService = navigationService;
        this.realmService = realmService;
        this.connectivity = connectivity;
        this.chatService = chatService;

        AppliedRequests = new ObservableCollection<JoinRequest>();
        AcceptedRequests = new ObservableCollection<JoinRequest>();

        realm = Realm.GetInstance(realmService.Config);
        player = realm.All<VenueUser>().First(x => x.OwnerId == realmService.RealmUser.Id);
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
            if (FutureBooking == null)
            {
                FutureBooking = realm.Find<PlannedBooking>(
                    PurchasedBooking.ConnectedPlannedBooking.Id
                );
            }
            if (FutureBooking != null)
            {
                foreach (JoinRequest request in FutureBooking.JoinRequests)
                {
                    if (request.IsApproved == true)
                    {
                        AcceptedRequests.Add(request);
                    }
                    else
                    {
                        AppliedRequests.Add(request);
                    }
                }
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

    [RelayCommand]
    public async Task UpdateJoinRequests(JoinRequest joinRequest)
    {
        try
        {
            bool isApproved;
            if (joinRequest.IsApproved)
            {
                isApproved = await Shell.Current.DisplayAlert(
                    "Remove Request",
                    "Do you want to remove this request?",
                    "Yes",
                    "No"
                );
            }
            else
            {
                // the host can only approve the number of players required for the booking -1 as the host is already included in the booking
                if (AcceptedRequests.Count == FutureBooking.NumberOfPlayers - 1)
                {
                    await Shell.Current.DisplayAlert(
                        "Alert",
                        "You have reached the maximum number of players",
                        "OK"
                    );
                    return;
                }
                isApproved = await Shell.Current.DisplayAlert(
                    "Approve Request",
                    "Do you want to approve this request?",
                    "Yes",
                    "No"
                );
            }
            if (isApproved)
            {
                await realm.WriteAsync(() =>
                {
                    // if the planned booking is connected with the Purchased booking the user is removed from the joinedPlayers of the purchased booking
                    if (PurchasedBooking != null)
                    {
                        VenueUser user = realm
                            .All<VenueUser>()
                            .First(x => x.OwnerId == joinRequest.AppliedBy.OwnerId);
                        if (joinRequest.IsApproved)
                        {
                            PurchasedBooking.JoinedPlayers.Remove(joinRequest.AppliedBy);
                        }
                    }

                    joinRequest.IsApproved = !joinRequest.IsApproved;
                });

                AcceptedRequests.Clear();
                AppliedRequests.Clear();
                foreach (JoinRequest request in FutureBooking.JoinRequests)
                {
                    if (request.IsApproved == true)
                    {
                        AcceptedRequests.Add(request);
                    }
                    else
                    {
                        AppliedRequests.Add(request);
                    }
                }

                await UpdateConversation(joinRequest);
            }
            else
            {
                await realm.WriteAsync(() =>
                {
                    joinRequest.IsApproved = joinRequest.IsApproved;
                });
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// the request is deleted from the planned booking and the if the planned booking is connected to a purchased booking, the user is also removed from the joinedPlayers of the purchased booking
    /// </summary>
    /// <param name="joinRequest"></param>
    /// <returns></returns>
    [RelayCommand]
    public async Task DeleteRequest(JoinRequest joinRequest)
    {
        try
        {
            bool isApproved = await Shell.Current.DisplayAlert(
                "Delete Request",
                "Do you want to delete this request?",
                "Yes",
                "No"
            );
            if (isApproved)
            {
                await realm.WriteAsync(() =>
                {
                    // if the planned booking is connected to a purchased booking, the user is removed from the purchased booking
                    if (PurchasedBooking != null)
                    {
                        VenueUser user = realm
                            .All<VenueUser>()
                            .First(x => x.OwnerId == joinRequest.AppliedBy.OwnerId);

                        PurchasedBooking.JoinedPlayers.Remove(joinRequest.AppliedBy);
                    }
                    FutureBooking.JoinRequests.Remove(joinRequest);
                });
                await Initialize();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    /// <summary>
    /// A new conversation is created whenever a new user is added to the group. the conversation Id is added to the Planned Booking
    /// </summary>
    /// <param name="joinRequest"></param>
    /// <returns></returns>
    private async Task UpdateConversation(JoinRequest joinRequest)
    {
        try
        {
            if (joinRequest.IsApproved)
            {
                if (string.IsNullOrWhiteSpace(FutureBooking.ConversationId))
                {
                    string conversationId = string.Empty;
                    // string acceptedUserNames= string.Join(",", AcceptedRequests.Select(x=> x.AppliedBy.Email).ToList());
                    ConversationCreateResponse? response =
                        await chatService.CreateGroupConversation(
                            player.Email,
                            joinRequest.AppliedBy.Email
                        );
                    if (string.IsNullOrWhiteSpace(response?.Error))
                    {
                        conversationId = response?.ConversationId;
                        await realm.WriteAsync(() =>
                        {
                            FutureBooking.ConversationId = conversationId;
                        });
                    }
                }
                else
                {
                    AddRemoveUserResponse? response = await chatService.AddConversationUsers(
                        FutureBooking.ConversationId,
                        player.Email,
                        joinRequest.AppliedBy.Email
                    );
                    if (!string.IsNullOrWhiteSpace(response?.Error))
                    {
                        await Shell.Current.DisplayAlert("Failure", "try after some time", "OK");
                    }
                }
            }
            else
            {
                AddRemoveUserResponse? response = await chatService.RemoveConversationUser(
                    FutureBooking.ConversationId,
                    player.Email,
                    joinRequest.AppliedBy.Email
                );
                if (!string.IsNullOrWhiteSpace(response?.Error))
                {
                    await Shell.Current.DisplayAlert("Failure", "try after some time", "OK");
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
