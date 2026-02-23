using CommunityToolkit.Mvvm.DependencyInjection;
using Twilio.TwiML.Voice;
using Task = System.Threading.Tasks.Task;


namespace Player.ViewModels.Connect
{
    public partial class PlanBookingViewModel : BaseViewModel
    {
        [ObservableProperty]
        ObservableCollection<Venue> activeVendors;

        [ObservableProperty]
        ObservableCollection<Venue> filteredVendors;

        [ObservableProperty]
        ObservableCollection<Game>? onboardedGames;

        [ObservableProperty]
        ObservableCollection<string> gameNames;

        [ObservableProperty]
        ObservableCollection<string> cityNames;

        [ObservableProperty]
        ObservableCollection<string> timeList;

        [ObservableProperty]
        string selectedTime;

        [ObservableProperty]
        string selectedStartTime;

        [ObservableProperty]
        string selectedEndTime;

        [ObservableProperty]
        bool isSpecificTimeSelected;

        [ObservableProperty]
        double numberOfPlayers;

        [ObservableProperty]
        double requiredPlayers;

        [ObservableProperty]
        string skillLevel;

        [ObservableProperty]
        string timing;

        [ObservableProperty]
        List<VenueDate>? venueDates;

        [ObservableProperty]
        VenueDate plannedDate;

        [ObservableProperty]
        // this property shows date in ReviewPlannedBookingView (PopupPage)
        DateTimeOffset selectedDate;

        [ObservableProperty]
        Game? selectedGame;

        [ObservableProperty]
        string? selectedCity;

        [ObservableProperty]
        string? cityProvince;

        [NotifyPropertyChangedFor(nameof(HostMessageCharacterCount))]
        [ObservableProperty]
        string hostMessage;

        [ObservableProperty]
        string timingDisplay;

        public string HostMessageCharacterCount => (HostMessage?.Length ?? 0).ToString();

        [ObservableProperty]
        Venue? selectedVendor;

        [ObservableProperty]
        string? userName;

        [ObservableProperty]
        Color beginnerBackgroundColor;

        [ObservableProperty]
        Color beginnerTextColor;

        [ObservableProperty]
        Color intermediateBackgroundColor;

        [ObservableProperty]
        Color intermediateTextColor;

        [ObservableProperty]
        Color advancedBackgroundColor;

        [ObservableProperty]
        Color advancedTextColor;

        [ObservableProperty]
        string morningImage;

        [ObservableProperty]
        Color morningTextColor;

        [ObservableProperty]
        string afternoonImage;

        [ObservableProperty]
        Color afternoonTextColor;

        [ObservableProperty]
        string eveningImage;

        [ObservableProperty]
        Color eveningTextColor;

        [ObservableProperty]
        string nightImage;

        [ObservableProperty]
        Color nightTextColor;

        Popup reviewPlannedBookingView;

        AppTheme currentTheme;

        readonly Realm realm;

        readonly IDisposable vendorsToken;

        private readonly INavigationService navigationService;
        private readonly IRealmService realmService;
        private readonly IConnectivity connectivity;

        private readonly IPopupNavigation popupNavigation;

        public PlanBookingViewModel(
            INavigationService navigationService,
            IRealmService realmService,
            IConnectivity connectivity,
            IPopupNavigation popupNavigation
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.connectivity = connectivity;
                this.realmService = realmService;
                this.navigationService = navigationService;
                this.popupNavigation = popupNavigation;

                ActiveVendors = new ObservableCollection<Venue>();
                FilteredVendors = new ObservableCollection<Venue>();
                OnboardedGames = new ObservableCollection<Game>();
                GameNames = new ObservableCollection<string>();
                CityNames = new ObservableCollection<string>();
                VenueDates = new List<VenueDate>();

                realm = Realm.GetInstance(realmService.Config);
                UserName = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault()
                    ?.FirstName;

                Microsoft.Maui.Controls.Application.Current.RequestedThemeChanged +=
                    Current_RequestedThemeChanged;

                currentTheme = Microsoft.Maui.Controls.Application.Current.RequestedTheme;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        public async Task Initialize()
        {
            try
            {
                ActiveVendors = realm.All<Venue>().ToObservableCollection();
                List<AvailableGame> Games = ActiveVendors
                    .SelectMany(m => m.AvailableGames)
                    .ToList();
                await GetGames();
                GameNames = Games
                    .DistinctBy(m => m.Name)
                    .Select(m => m.Name)
                    .ToObservableCollection();
                CityNames = ActiveVendors
                    .DistinctBy(m => m.Address.City)
                    .Select(n => n.Address.City)
                    .ToObservableCollection();
                SelectedGame = OnboardedGames.FirstOrDefault();
                SelectedCity = CityNames.FirstOrDefault();
                // string? city = SelectedCity?.Split(',')[1].Trim();
                FilteredVendors = ActiveVendors
                    .Where(v =>
                        v.Address.City.Contains(SelectedCity)
                        && v.AvailableGames.Any(g => g.Name == SelectedGame.GameName)
                    )
                    .ToObservableCollection();
                SelectedVendor = FilteredVendors.FirstOrDefault();
                NumberOfPlayers =
                    SelectedVendor
                        ?.AvailableGames.Where(g => g.Name == SelectedGame.GameName)
                        .FirstOrDefault()
                        .PeoplePerGame ?? 2;
                RequiredPlayers = 2;

                await BeginnerSelected();
                await MorningSelected();
                IsSpecificTimeSelected = false;
                Timing = PlayerConstants.morning;

                TimeList = new ObservableCollection<string>
                {
                    "00:00",
                    "1:00",
                    "2:00",
                    "3:00",
                    "4:00",
                    "5:00",
                    "6:00",
                    "7:00",
                    "8:00",
                    "9:00",
                    "10:00",
                    "11:00",
                    "12:00",
                    "13:00",
                    "14:00",
                    "15:00",
                    "16:00",
                    "17:00",
                    "18:00",
                    "19:00",
                    "20:00",
                    "21:00",
                    "22:00",
                    "23:00",
                };

                SelectedStartTime = TimeList.ElementAt(6);
                SelectedEndTime = TimeList.ElementAt(7);

                for (
                    DateTime date = DateTime.Now.AddDays(1);
                    date < DateTime.Now.AddDays(90);
                    date = date.AddDays(1)
                )
                {
                    VenueDates?.Add(new VenueDate(date: date));
                }

                PlannedDate = VenueDates.FirstOrDefault();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task GetGames()
        {
            try
            {
                List<Venue>? ActiveVendors = realm
                    .All<Venue>()
                    .Where(v => v.IsActive == true)
                    .ToList();

                List<AvailableGame>? Games = ActiveVendors
                    ?.SelectMany(m => m.AvailableGames)
                    .DistinctBy(x => x.Name)
                    .ToList();

                OnboardedGames.Clear();

                foreach (AvailableGame availableGame in Games)
                {
                    Game? game = realm
                        .All<Game>()
                        .Where(g => g.GameName == availableGame.Name)
                        .FirstOrDefault();
                    if (game != null)
                    {
                        OnboardedGames.Add(game);
                    }
                }

                ChangeGameIcons();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Okay");
            }
        }

        private async void Current_RequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
        {
            currentTheme = e.RequestedTheme;
            ChangeGameIcons();
            if (SkillLevel == "Beginner")
            {
                await BeginnerSelected();
            }
            else if (SkillLevel == "Intermediate")
            {
                await IntermediateSelected();
            }
            else if (SkillLevel == "Advanced")
            {
                await AdvancedSelected();
            }
            if (Timing == PlayerConstants.morning)
            {
                await MorningSelected();
            }
            else if (Timing == PlayerConstants.afternoon)
            {
                await AfternoonSelected();
            }
            else if (Timing == PlayerConstants.evening)
            {
                await EveningSelected();
            }
            else if (Timing == PlayerConstants.night)
            {
                await NightSelected();
            }
        }

        private void ChangeGameIcons()
        {
            if (currentTheme == AppTheme.Dark)
            {
                foreach (Game game in OnboardedGames)
                {
                    game.GameIcon = game.DarkModeGameIcon;
                }
            }
            else
            {
                foreach (Game game in OnboardedGames)
                {
                    if (game.IsSelected)
                    {
                        game.GameIcon = game.DarkModeGameIcon;
                    }
                    else
                    {
                        game.GameIcon = game.LightModeGameIcon;
                    }
                }
            }
        }

        [RelayCommand]
        async Task BeginnerSelected()
        {
            try
            {
                SkillLevel = "Beginner";
                BeginnerBackgroundColor = Color.FromArgb("#EF2F50");
                BeginnerTextColor = Colors.White;
                if (currentTheme == AppTheme.Dark)
                {
                    IntermediateBackgroundColor = Colors.Transparent;
                    IntermediateTextColor = Colors.White;
                    AdvancedBackgroundColor = Colors.Transparent;
                    AdvancedTextColor = Colors.White;
                }
                else if (currentTheme == AppTheme.Light)
                {
                    IntermediateBackgroundColor = Colors.Transparent;
                    IntermediateTextColor = Colors.Black;
                    AdvancedBackgroundColor = Colors.Transparent;
                    AdvancedTextColor = Colors.Black;
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task IntermediateSelected()
        {
            try
            {
                SkillLevel = "Intermediate";
                IntermediateBackgroundColor = Color.FromArgb("#EF2F50");
                IntermediateTextColor = Colors.White;
                if (currentTheme == AppTheme.Dark)
                {
                    BeginnerBackgroundColor = Colors.Transparent;
                    BeginnerTextColor = Colors.White;
                    AdvancedBackgroundColor = Colors.Transparent;
                    AdvancedTextColor = Colors.White;
                }
                else if (currentTheme == AppTheme.Light)
                {
                    BeginnerBackgroundColor = Colors.Transparent;
                    BeginnerTextColor = Colors.Black;
                    AdvancedBackgroundColor = Colors.Transparent;
                    AdvancedTextColor = Colors.Black;
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task AdvancedSelected()
        {
            try
            {
                SkillLevel = "Advanced";
                AdvancedBackgroundColor = Color.FromArgb("#EF2F50");
                AdvancedTextColor = Colors.White;
                if (currentTheme == AppTheme.Dark)
                {
                    BeginnerBackgroundColor = Colors.Transparent;
                    BeginnerTextColor = Colors.White;
                    IntermediateBackgroundColor = Colors.Transparent;
                    IntermediateTextColor = Colors.White;
                }
                else if (currentTheme == AppTheme.Light)
                {
                    BeginnerBackgroundColor = Colors.Transparent;
                    BeginnerTextColor = Colors.Black;
                    IntermediateBackgroundColor = Colors.Transparent;
                    IntermediateTextColor = Colors.Black;
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task SelectTiming()
        {
            try
            {
                await popupNavigation.PushAsync(new PlanBookingTimingPopup(this));
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task MorningSelected()
        {
            try
            {
                Timing = PlayerConstants.morning;
                MorningTextColor = Colors.White;
                MorningImage = "morning_timing_darkmode.png";
                if (currentTheme == AppTheme.Dark)
                {
                    AfternoonTextColor = Colors.White;
                    EveningTextColor = Colors.White;
                    NightTextColor = Colors.White;
                    AfternoonImage = "afternoon_timing_darkmode.png";
                    EveningImage = "evening_timing_darkmode.png";
                    NightImage = "night_timing_darkmode.png";
                }
                else if (currentTheme == AppTheme.Light)
                {
                    AfternoonTextColor = Colors.Black;
                    EveningTextColor = Colors.Black;
                    NightTextColor = Colors.Black;
                    AfternoonImage = "afternoon_timing_lightmode.png";
                    EveningImage = "evening_timing_lightmode.png";
                    NightImage = "night_timing_lightmode.png";
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task AfternoonSelected()
        {
            try
            {
                Timing = PlayerConstants.afternoon;
                AfternoonTextColor = Colors.White;
                AfternoonImage = "afternoon_timing_darkmode.png";
                if (currentTheme == AppTheme.Dark)
                {
                    MorningTextColor = Colors.White;
                    EveningTextColor = Colors.White;
                    NightTextColor = Colors.White;
                    MorningImage = "morning_timing_darkmode.png";
                    EveningImage = "evening_timing_darkmode.png";
                    NightImage = "night_timing_darkmode.png";
                }
                else if (currentTheme == AppTheme.Light)
                {
                    MorningTextColor = Colors.Black;
                    EveningTextColor = Colors.Black;
                    NightTextColor = Colors.Black;
                    MorningImage = "morning_timing_lightmode.png";
                    EveningImage = "evening_timing_lightmode.png";
                    NightImage = "night_timing_lightmode.png";
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task EveningSelected()
        {
            try
            {
                Timing = PlayerConstants.evening;
                EveningTextColor = Colors.White;
                EveningImage = "evening_timing_darkmode.png";
                if (currentTheme == AppTheme.Dark)
                {
                    MorningTextColor = Colors.White;
                    AfternoonTextColor = Colors.White;
                    NightTextColor = Colors.White;
                    MorningImage = "morning_timing_darkmode.png";
                    AfternoonImage = "afternoon_timing_darkmode.png";
                    NightImage = "night_timing_darkmode.png";
                }
                else if (currentTheme == AppTheme.Light)
                {
                    MorningTextColor = Colors.Black;
                    AfternoonTextColor = Colors.Black;
                    NightTextColor = Colors.Black;
                    MorningImage = "morning_timing_lightmode.png";
                    AfternoonImage = "afternoon_timing_lightmode.png";
                    NightImage = "night_timing_lightmode.png";
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task NightSelected()
        {
            try
            {
                Timing = PlayerConstants.night;
                NightTextColor = Colors.White;
                NightImage = "night_timing_darkmode.png";
                if (currentTheme == AppTheme.Dark)
                {
                    MorningTextColor = Colors.White;
                    AfternoonTextColor = Colors.White;
                    EveningTextColor = Colors.White;
                    MorningImage = "morning_timing_darkmode.png";
                    AfternoonImage = "afternoon_timing_darkmode.png";
                    EveningImage = "evening_timing_darkmode.png";
                }
                else if (currentTheme == AppTheme.Light)
                {
                    MorningTextColor = Colors.Black;
                    AfternoonTextColor = Colors.Black;
                    EveningTextColor = Colors.Black;
                    MorningImage = "morning_timing_lightmode.png";
                    AfternoonImage = "afternoon_timing_lightmode.png";
                    EveningImage = "evening_timing_lightmode.png";
                }
            }
            catch (System.Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task TimeSelectionComplete()
        {
            try
            {
                if (IsSpecificTimeSelected)
                {
                    Timing = $"{SelectedStartTime} - {SelectedEndTime}";
                }

                await popupNavigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        partial void OnSelectedEndTimeChanged(string value)
        {
            // the index starts from 0 and ends at 23. The time list has 24 elements
            // The list start from 00:00 (index 0) and ends at 23:00  (index 23)
            int startTimeIndex = TimeList.IndexOf(SelectedStartTime);
            int endTimeIndex = TimeList.IndexOf(SelectedEndTime);
            // endtime should not be more than 3 hours than start time
            int maxEndTimeIndex = startTimeIndex + 3;
            if (startTimeIndex >= endTimeIndex)
            {
                if (endTimeIndex == 0)
                {
                    SelectedStartTime = TimeList.ElementAt(23);
                }
                else
                {
                    SelectedStartTime = TimeList.ElementAt(endTimeIndex - 1);
                }
            }
            else if (endTimeIndex > maxEndTimeIndex)
            {
                if (endTimeIndex == 0)
                {
                    SelectedStartTime = TimeList.ElementAt(23);
                }
                else if (endTimeIndex == 1 || endTimeIndex == 2)
                {
                    SelectedStartTime = TimeList.ElementAt(0);
                }
                else
                {
                    SelectedStartTime = TimeList.ElementAt(endTimeIndex - 3);
                }
            }
        }

        partial void OnSelectedStartTimeChanged(string value)
        {
            // the index starts from 0 and ends at 23. The time list has 24 elements
            // The list start from 00:00 (index 0) and ends at 23:00  (index 23)
            int startTimeIndex = TimeList.IndexOf(SelectedStartTime);
            int endTimeIndex = TimeList.IndexOf(SelectedEndTime);
            // endtime should not be more than 3 hours than start time
            int maxEndTimeIndex = startTimeIndex + 3;
            if (startTimeIndex >= endTimeIndex)
            {
                if (startTimeIndex == 23)
                {
                    SelectedEndTime = TimeList.ElementAt(0);
                }
                else
                {
                    SelectedEndTime = TimeList.ElementAt(startTimeIndex + 1);
                }
            }
            else if (endTimeIndex > maxEndTimeIndex)
            {
                SelectedEndTime = TimeList.ElementAt(maxEndTimeIndex);
            }
        }

        /// <summary>
        /// validate all data and display ReviewPlannedBookingView (PopupPage)
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task ReviewHosting()
        {
            try
            {
                if (String.IsNullOrEmpty(SelectedGame?.GameName))
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a game", "OK");
                }
                else if (string.IsNullOrEmpty(SkillLevel))
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a skill level", "OK");
                }
                else if (string.IsNullOrEmpty(SelectedCity))
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a city", "OK");
                }
                else if (SelectedVendor == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a venue", "OK");
                }
                else if (RequiredPlayers < 2)
                {
                    await Shell.Current.DisplayAlert(
                        "Error",
                        "Number of players should be greater than 1",
                        "OK"
                    );
                }
                else if (string.IsNullOrEmpty(Timing))
                {
                    await Shell.Current.DisplayAlert("Error", "Please select a timing", "OK");
                }
                else
                {
                    CityProvince =
                        $"{SelectedVendor?.Address?.City}, {SelectedVendor?.Address?.Province}";
                    if (IsSpecificTimeSelected)
                    {
                        Timing = $"{SelectedStartTime} - {SelectedEndTime}";
                    }

                    // set the date to display it in ReviewPlannedBookingView (PopupPage)
                    SelectedDate = PlannedDate.Date;
                    await popupNavigation.PushAsync(new ReviewPlannedBookingPopup(this));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task Host()
        {
            try
            {
                VenueUser? Host = realm
                    .All<VenueUser>()
                    .Where(v => v.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();
                if (Host == null)
                {
                    await Shell.Current.DisplayAlert("Error", "host required", "OK");
                    return;
                }
                PlannedBooking plannedBooking = new PlannedBooking(
                    gameName: SelectedGame.GameName,
                    host: Host,
                    skillLevel: SkillLevel,
                    city: SelectedCity,
                    selectedVenue: SelectedVendor,
                    numberOfPlayers: (int)RequiredPlayers,
                    timing: Timing,
                    plannedDate: PlannedDate.Date
                );
                plannedBooking.HostMessage = HostMessage;
                await realm.WriteAsync(() =>
                {
                    realm.Add(plannedBooking);
                });
                await popupNavigation.PopAllAsync();
                await navigationService.NavigateToAsync($"///{nameof(MyBookingsView)}");
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
                await popupNavigation.PopAllAsync();
            }
            catch (Exception ex)
            {
                await popupNavigation.PopAllAsync();
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            try
            {
                base.OnPropertyChanged(e);
                if (e.PropertyName == nameof(SelectedGame))
                {
                    OnboardedGames.ToList().ForEach(g => g.IsSelected = false);
                    Game? choosenGame = OnboardedGames
                        .Where(g => g.GameName == SelectedGame.GameName)
                        .FirstOrDefault();
                    choosenGame.IsSelected = true;
                    SelectedGame.IsSelected = true;

                    ChangeGameIcons();

                    await Search();
                }
                else if (e.PropertyName == nameof(SelectedCity))
                {
                    await Search();
                }
                else if (e.PropertyName == nameof(SelectedVendor))
                {
                    NumberOfPlayers =
                        SelectedVendor
                            ?.AvailableGames.Where(g => g.Name == SelectedGame.GameName)
                            .FirstOrDefault()
                            .PeoplePerGame ?? 2;
                }
                else if (e.PropertyName == nameof(PlannedDate))
                {
                    // change Is selected property of selected date
                    VenueDates.ForEach(d => d.IsSelected = false);
                    PlannedDate.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        async Task Search()
        {
            try
            {
                if (SelectedGame != null && SelectedCity != null)
                {
                    // string? city = SelectedCity?.Split(',')[1].Trim();
                    string gameName = SelectedGame.GameName;

                    FilteredVendors = ActiveVendors
                        .Where(v =>
                            v.Address.City.Contains(SelectedCity)
                            && v.AvailableGames.Any(g => g.Name == gameName)
                        )
                        .ToObservableCollection();
                    if (SelectedVendor != null)
                    {
                        bool venueExists = FilteredVendors.Any(v =>
                            v.FullName == SelectedVendor.FullName
                        );
                        if (!venueExists)
                        {
                            SelectedVendor = FilteredVendors.FirstOrDefault();
                        }
                    }
                    else
                    {
                        SelectedVendor = FilteredVendors.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
