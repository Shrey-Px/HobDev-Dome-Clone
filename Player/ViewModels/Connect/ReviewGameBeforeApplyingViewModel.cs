namespace Player.ViewModels.Connect;

public partial class ReviewGameBeforeApplyingViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    PlannedBooking selectedBooking;

    [NotifyPropertyChangedFor(nameof(ApplicantMessageLength))]
    [ObservableProperty]
    string? applicantMessage;

    [ObservableProperty]
    byte[]? playerImage;

    public string? ApplicantMessageLength => (ApplicantMessage?.Length ?? 0).ToString();

    Realm realm;

    private readonly INavigationService navigationService;
    private readonly IRealmService realmService;
    private readonly IConnectivity connectivity;

    public ReviewGameBeforeApplyingViewModel(
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

            realm = Realm.GetInstance(realmService.Config);

            VenueUser? Player = realm
                .All<VenueUser>()
                .Where(n => n.OwnerId == realmService.RealmUser.Id)
                .FirstOrDefault();
            PlayerImage = Player?.ProfileImage;
        }
        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
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
                    ObjectId Id = ObjectId.Parse(id.ToString());
                    SelectedBooking = realm.Find<PlannedBooking>(Id);
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }

    [RelayCommand]
    async Task Apply()
    {
        try
        {
            VenueUser? applicant = realm
                .All<VenueUser>()
                .Where(n => n.OwnerId == realmService.RealmUser.Id)
                .FirstOrDefault();
            JoinRequest request = new JoinRequest
            {
                AppliedBy = applicant,
                ApplicantMessage = ApplicantMessage,
                IsApproved = false,
            };
            await realm.WriteAsync(() =>
            {
                SelectedBooking.JoinRequests.Add(request);
            });

            await navigationService.PopAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
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
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
}
