namespace Dome.Shared.Synced
{
    /// <summary>
    /// the available game model is the model that is used to store the information about the games that are available to be played at the venue.
    /// Embedded inside Venue.
    /// </summary>
    public partial class AvailableGame : IEmbeddedObject
    {
        private AvailableGame() { }

        public AvailableGame(
            string name,
            int peoplePerGame,
            string fieldType,
            int fieldsCount,
            decimal weekdayHourlyRate,
            decimal weekdayPeakHoursHourlyRate,
            decimal weekendHourlyRate,
            decimal weekendPeakHoursHourlyRate
        )
        {
            Name = name;
            PeoplePerGame = peoplePerGame;
            FieldType = fieldType;
            FieldsCount = fieldsCount;
            WeekdayHourlyRate = weekdayHourlyRate;
            WeekdayPeakHoursHourlyRate = weekdayPeakHoursHourlyRate;
            WeekendHourlyRate = weekendHourlyRate;
            WeekendPeakHoursHourlyRate = weekendPeakHoursHourlyRate;
        }

        [MapTo("name")]
        public string Name { get; set; }

        [MapTo("firstGameImageName")]
        public string? FirstImageName { get; set; }

        [MapTo("secondGameImageName")]
        public string? SecondImageName { get; set; }

        [MapTo("peoplePerGame")]
        public int PeoplePerGame { get; set; }

        [MapTo("fieldType")]
        public string FieldType { get; set; }

        [MapTo("fieldsCount")]
        public int FieldsCount { get; set; } = 0;

        // rate
        [MapTo("weekdayHourlyRate")]
        public decimal WeekdayHourlyRate { get; set; }

        [MapTo("weekdayPeakHoursHourlyRate")]
        public decimal WeekdayPeakHoursHourlyRate { get; set; }

        [MapTo("weekendHourlyRate")]
        public decimal WeekendHourlyRate { get; set; }

        [MapTo("weekendPeakHoursHourlyRate")]
        public decimal WeekendPeakHoursHourlyRate { get; set; }

        // embedded objects
        [MapTo("timing")]
        public GameTiming? Timing { get; set; }

        /// <summary>
        /// Embedded fields that belong to this game.
        /// </summary>
        [MapTo("fields")]
        public IList<Field> Fields { get; }

        //Local
        // this field is to show the information about the game in the Admin app while creating and editing the venue
        [Ignored]
        public string FieldInformation =>
            $"{FieldsCount} {FieldType}s | {PeoplePerGame} Players / {FieldType}";

        private bool isSelected;

        // use to highlight the selected game while booking the field in the venue app
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    RaisePropertyChanged();
                }
            }
        }

        [Ignored]
        public byte[]? FirstImageBytes { get; set; }

        [Ignored]
        public string? FirstImageExtension { get; set; }

        [Ignored]
        public byte[]? SecondImageBytes { get; set; }

        [Ignored]
        public string? SecondImageExtension { get; set; }
    }
}
