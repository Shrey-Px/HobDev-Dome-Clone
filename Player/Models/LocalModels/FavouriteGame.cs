namespace Player.Models.LocalModels
{
    /// <summary>
    /// the FavouriteGame is a class that represents the favourite game of the user. The games are created in memory and the user select favourite games from all the games while creating the user profile.
    /// </summary>
    public partial class FavouriteGame : ObservableObject
    {
        public FavouriteGame(string gameName)
        {
            GameName = gameName;
        }

        public string GameName { get; set; }

        [ObservableProperty]
        public bool isSelected;
    }
}
