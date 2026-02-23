namespace Admin.ViewModels
{
    public partial class CoachViewModel : BaseViewModel
    {
        [ObservableProperty]
        Coach coachData;

        [ObservableProperty]
        List<string> competancyTypes;

        [ObservableProperty]
        List<string> allGames;

        [ObservableProperty]
        List<Venue> allVenues;

        [ObservableProperty]
        Venue? selectedVenue;

        Realm realm;

        private readonly IImageService imageService;

        public CoachViewModel(
            IImageService imageService,
            IRealmService realmService,
            INavigationService navigationService,
            IConnectivity connectivity
        )
            : base(realmService, navigationService, connectivity)
        {
            try
            {
                this.imageService = imageService;

                CompetancyTypes = new List<string>() { "Beginner", "Intermediate", "Advanced" };

                realm = Realm.GetInstance(realmService.Config);

                IEnumerable<Game> games = realm.All<Game>();

                AllGames = games.Select(p => p.GameName).ToList();

                // Load all venues for selection
                AllVenues = realm.All<Venue>().Where(v => v.IsActive).ToList();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task OnLoaded()
        {
            try
            {
                SelectedVenue = null;
                CoachData = new Coach(
                    id: ObjectId.GenerateNewId(),
                    coachName: string.Empty,
                    gameCategory: string.Empty,
                    coachDescription: string.Empty,
                    venue: null,
                    competancy: string.Empty,
                    phoneCode: "+1",
                    contactNumber: string.Empty,
                    email: string.Empty
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task UploadCoach()
        {
            try
            {
                if (
                    string.IsNullOrEmpty(CoachData.CoachName)
                    || string.IsNullOrEmpty(CoachData.CoachDescription)
                    || SelectedVenue == null
                    || string.IsNullOrEmpty(CoachData.Competancy)
                    || string.IsNullOrEmpty(CoachData.ContactNumber)
                    || string.IsNullOrEmpty(CoachData.Email)
                    || string.IsNullOrEmpty(CoachData.GameCategory)
                )
                {
                    await Shell.Current.DisplayAlert("Error", "Please fill all the fields", "OK");
                }
                else
                {
                    // Set the selected venue before saving
                    CoachData.Venue = SelectedVenue;
                    
                    await realm.WriteAsync(() =>
                    {
                        realm.Add(CoachData);
                    });
                    await Shell.Current.DisplayAlert(
                        "Success",
                        "Content uploaded successfully",
                        "OK"
                    );
                    await OnLoaded();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Cancel()
        {
            try
            {
                await OnLoaded();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
