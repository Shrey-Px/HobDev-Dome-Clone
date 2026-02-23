namespace Dome.Shared.Synced
{
    /// <summary>
    /// Venue Timing represents the opening and closing time of the venue on weekdays and weekends.
    /// </summary>
    public partial class GameTiming : IEmbeddedObject
    {
        private GameTiming() { }

        public GameTiming(
            DateTimeOffset weekdayOpenTime,
            DateTimeOffset weekdayCloseTime,
            DateTimeOffset weekdayPeakHoursStartTime,
            DateTimeOffset weekdayPeakHoursEndTime,
            DateTimeOffset weekendOpenTime,
            DateTimeOffset weekendCloseTime,
            DateTimeOffset weekendPeakHoursStartTime,
            DateTimeOffset weekendPeakHoursEndTime
        )
        {
            WeekdayOpenTime = weekdayOpenTime;
            WeekdayCloseTime = weekdayCloseTime;
            WeekdayPeakHoursStartTime = weekdayPeakHoursStartTime;
            WeekdayPeakHoursEndTime = weekdayPeakHoursEndTime;
            WeekendOpenTime = weekendOpenTime;
            WeekendCloseTime = weekendCloseTime;
            WeekendPeakHoursStartTime = weekendPeakHoursStartTime;
            WeekendPeakHoursEndTime = weekendPeakHoursEndTime;
        }

        [MapTo("weekdayOpenTime")]
        public DateTimeOffset WeekdayOpenTime { get; set; }

        [MapTo("weekdayCloseTime")]
        public DateTimeOffset WeekdayCloseTime { get; set; }

        [MapTo("weekdayPeakHoursStartTime")]
        public DateTimeOffset WeekdayPeakHoursStartTime { get; set; }

        [MapTo("weekdayPeakHoursEndTime")]
        public DateTimeOffset WeekdayPeakHoursEndTime { get; set; }

        [MapTo("weekendOpenTime")]
        public DateTimeOffset WeekendOpenTime { get; set; }

        [MapTo("weekendCloseTime")]
        public DateTimeOffset WeekendCloseTime { get; set; }

        [MapTo("weekendPeakHoursStartTime")]
        public DateTimeOffset WeekendPeakHoursStartTime { get; set; }

        [MapTo("weekendPeakHoursEndTime")]
        public DateTimeOffset WeekendPeakHoursEndTime { get; set; }
    }
}
