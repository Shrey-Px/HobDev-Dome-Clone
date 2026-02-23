namespace Admin.Models.LocalModels
{
    /// <summary>
    /// the local time data is used to populate the time picker Popup for slecting open time and close time of the venue on weekdays and weekends
    /// </summary>
    public class LocalTimeData
    {
        public LocalTimeData(int time)
        {
            Time = time;
        }

        public int Time { get; set; }
    }
}
