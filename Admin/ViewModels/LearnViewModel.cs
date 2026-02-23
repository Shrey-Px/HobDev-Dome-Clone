namespace Admin.ViewModels
{
    public partial class LearnViewModel : BaseViewModel
    {
        // property for selecting content type when creating new content
        [ObservableProperty]
        LearningContent content;

        [ObservableProperty]
        List<string> contentTypes;

        [ObservableProperty]
        Game? selectedGame;

        [ObservableProperty]
        List<Game> allGames;

        // property for selecting content type for filtering learn list
        [ObservableProperty]
        string? selectedContentType;

        [ObservableProperty]
        ObservableCollection<LearningContent> allLearningContents;

        [ObservableProperty]
        ObservableCollection<LearningContent> filteredLearningContents;

        Realm realm;

        private readonly IImageService imageService;

        public LearnViewModel(
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

                ContentTypes = new List<string>() { "Article", "Video" };

                AllLearningContents = new ObservableCollection<LearningContent>();
                FilteredLearningContents = new ObservableCollection<LearningContent>();
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedContentType))
            {
                await SelectedContentTypeChanged();
            }
        }

        public async Task OnLoaded()
        {
            try
            {
                realm = Realm.GetInstance(realmService.Config);

                AllGames = realm.All<Game>().ToList();

                await InitializeNewContent();

                SelectedContentType = ContentTypes[0];
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task InitializeNewContent()
        {
            SelectedGame = null;
            Content = new LearningContent(
                id: ObjectId.GenerateNewId(),
                type: string.Empty,
                title: string.Empty,
                description: string.Empty,
                content: string.Empty,
                created: DateHelper.GetDate(DateTime.Now),
                publishedBy: string.Empty,
                gameCategory: string.Empty
            );
        }

        [RelayCommand]
        async Task UploadLearningContent()
        {
            try
            {
                if (SelectedGame != null)
                {
                    Content.GameCategory = SelectedGame.GameName;
                }
                if (
                    string.IsNullOrEmpty(Content.Title)
                    || string.IsNullOrEmpty(Content.Description)
                    || string.IsNullOrEmpty(Content.Type)
                    || string.IsNullOrEmpty(Content.Content)
                    || string.IsNullOrEmpty(Content.PublishedBy)
                    || string.IsNullOrEmpty(Content.GameCategory)
                )
                {
                    await Shell.Current.DisplayAlert("Error", "Please fill all the fields", "OK");
                }
                else
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Add(Content);
                    });
                    await Shell.Current.DisplayAlert(
                        "Success",
                        "Content uploaded successfully",
                        "OK"
                    );
                    await InitializeNewContent();
                    await SelectedContentTypeChanged();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        async Task SelectedContentTypeChanged()
        {
            try
            {
                AllLearningContents = new ObservableCollection<LearningContent>(
                    realm.All<LearningContent>().ToList()
                );
                FilteredLearningContents.Clear();
                if (AllLearningContents.Count > 0)
                {
                    if (SelectedContentType == "Article")
                    {
                        List<LearningContent> articles = AllLearningContents
                            .Where(x => x.Type == "Article")
                            .ToList();
                        if (articles.Count > 0)
                        {
                            foreach (var item in articles)
                            {
                                FilteredLearningContents.Add(item);
                            }
                        }
                    }
                    else
                    {
                        List<LearningContent> videos = AllLearningContents
                            .Where(x => x.Type == "Video")
                            .ToList();
                        if (videos.Count > 0)
                        {
                            foreach (var item in videos)
                            {
                                FilteredLearningContents.Add(item);
                            }
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
        async Task DeleteLearningContent(LearningContent learningContent)
        {
            try
            {
                if (learningContent != null)
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Remove(learningContent);
                    });
                    FilteredLearningContents.Remove(learningContent);
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
