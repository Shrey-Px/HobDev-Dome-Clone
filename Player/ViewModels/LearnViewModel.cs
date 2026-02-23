namespace Player.ViewModels
{
    public partial class LearnViewModel : BaseViewModel
    {
        [ObservableProperty]
        byte[]? playerImage;

        [ObservableProperty]
        string? userName;

        [ObservableProperty]
        int selectedIndex;

        [ObservableProperty]
        ObservableCollection<LearningContent>? contentList;

        [ObservableProperty]
        ObservableCollection<LearningContent>? filteredContentList;

        readonly IRealmService realmService;
        readonly INavigationService navigationService;
        readonly ILauncher launcher;
        readonly IBrowser browser;

        VenueUser? Player;
        Realm realm;

        public LearnViewModel(
            IRealmService realmService,
            INavigationService navigationService,
            ILauncher launcher,
            IConnectivity connectivity,
            IBrowser browser
        )
            : base(navigationService, realmService, connectivity)
        {
            try
            {
                this.realmService = realmService;
                this.navigationService = navigationService;
                this.launcher = launcher;
                this.browser = browser;

                ContentList = new ObservableCollection<LearningContent>();
                FilteredContentList = new ObservableCollection<LearningContent>();

                realm = Realm.GetInstance(realmService.Config);
                VenueUser? Player = realm
                    .All<VenueUser>()
                    .Where(n => n.OwnerId == realmService.RealmUser.Id)
                    .FirstOrDefault();

                FilteredContentList = new ObservableCollection<LearningContent>();
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
                ContentList = realm.All<LearningContent>().ToObservableCollection();

                FilteredContentList = new ObservableCollection<LearningContent>(
                    ContentList.Where(n => n.GameCategory == "Pickleball")
                );
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        partial void OnSelectedIndexChanged(int oldValue, int newValue)
        {
            try
            {
                if (oldValue == newValue)
                    if (ContentList.Count == 0)
                    {
                        return;
                    }
                FilteredContentList.Clear();
                if (SelectedIndex == 0)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Pickleball")
                    );
                }
                else if (SelectedIndex == 1)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Badminton")
                    );
                }
                else if (SelectedIndex == 2)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Volleyball")
                    );
                }
                else if (SelectedIndex == 3)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Basketball")
                    );
                }
                else if (SelectedIndex == 4)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Archery")
                    );
                }
                else if (SelectedIndex == 5)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Bowling")
                    );
                }
                else if (SelectedIndex == 6)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Golf")
                    );
                }
                else if (SelectedIndex == 7)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Escaperoom")
                    );
                }
                else if (SelectedIndex == 8)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Karting")
                    );
                }
                else if (SelectedIndex == 9)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Snooker")
                    );
                }
                else if (SelectedIndex == 10)
                {
                    FilteredContentList = new ObservableCollection<LearningContent>(
                        ContentList.Where(n => n.GameCategory == "Table tennis")
                    );
                }
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        [RelayCommand]
        async Task ShowContent(LearningContent content)
        {
            try
            {
                if (content.Type == "Video")
                {
                    await launcher.OpenAsync(content.Content);
                }
                else if (content.Type == "Article")
                {
                    await launcher.OpenAsync(content.Content);

                    //BrowserLaunchOptions browserLaunchOptions = new BrowserLaunchOptions
                    //{
                    //    PreferredToolbarColor = Color.FromArgb("#EF2F50"),
                    //    PreferredControlColor = Color.FromArgb("#EF2F50"),
                    //    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    //    TitleMode = BrowserTitleMode.Show,
                    //    Flags = BrowserLaunchFlags.None
                    //};
                    //await browser.OpenAsync(content.Content, browserLaunchOptions);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
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
}
