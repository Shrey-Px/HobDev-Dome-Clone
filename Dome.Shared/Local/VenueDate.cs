namespace Dome.Shared.Local
{
    /// <summary>
    /// represent dates and times when booking a venue in vendor and mobile app
    /// </summary>
    public partial class VenueDate : ObservableObject
    {
        public VenueDate(DateTimeOffset date)
        {
            Date = date;
        }

        public DateTimeOffset Date { get; set; }

        [ObservableProperty]
        bool isSelected;

        [ObservableProperty]
        string timeStatus;
    }
}
