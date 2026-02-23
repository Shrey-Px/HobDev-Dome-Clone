namespace Dome.Shared.Synced
{
    /// <summary>
    /// the game is a class that represents the games that are available in the Sport Venues. The games are pre stored in the database and the admin select the games that are available for each venue from all stored games while creating the venue. The games are also used to categorize the learning content.
    /// </summary>
    public partial class Game : IRealmObject
    {
        private Game() { }

        public Game(
            ObjectId id,
            string gameName,
            string fieldType,
            byte[] lightModeGameIcon,
            byte[] darkModeGameIcon
        )
        {
            Id = id;
            GameName = gameName;
            FieldType = fieldType;
            LightModeGameIcon = lightModeGameIcon;
            DarkModeGameIcon = darkModeGameIcon;
        }

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [MapTo("gameName")]
        [Indexed]
        public string GameName { get; set; }

        [MapTo("fieldType")]
        public string FieldType { get; set; }

        [MapTo("lightModeGameIcon")]
        public byte[] LightModeGameIcon { get; set; }

        [MapTo("darkModeGameIcon")]
        public byte[] DarkModeGameIcon { get; set; }

        // this will be used to bind to the UI and assign the DarkModeGameIcon or LightModeGameIcon based on the theme
        public byte[] gameIcon;

        [Ignored]
        public byte[] GameIcon
        {
            get => gameIcon;
            set
            {
                if (gameIcon != value)
                {
                    gameIcon = value;
                    RaisePropertyChanged();
                }
            }
        }

        // this is used to change the color of the border and icon based on the selection in JoinAGameView

        private bool isSelected;

        [Ignored]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}
