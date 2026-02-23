
namespace Dome.Shared.Synced.Player;

public partial class PlannedBooking : IRealmObject
{
    private PlannedBooking() { }

    public PlannedBooking(
        string gameName,
        VenueUser host,
        string skillLevel,
        string city,
        Venue? selectedVenue,
        int numberOfPlayers,
        string timing,
        DateTimeOffset plannedDate
    )
    {
        GameName = gameName;
        SkillLevel = skillLevel;
        City = city;
        SelectedVenue = selectedVenue;
        NumberOfPlayers = numberOfPlayers;
        Timing = timing;
        PlannedDate = plannedDate;
        Host = host;
    }

    [PrimaryKey]
    [MapTo("_id")]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    [MapTo("gameName")]
    public string GameName { get; set; }

    [MapTo("skillLevel")]
    public string SkillLevel { get; set; }

    [Indexed]
    [MapTo("city")]
    public string City { get; set; }

    [MapTo("selectedVenue")]
    public Venue? SelectedVenue { get; set; }

    /// <summary>
    ///  number of players required for the planned booking including the host
    /// </summary>
    [MapTo("numberOfPlayers")]
    public int NumberOfPlayers { get; set; }

    [MapTo("timing")]
    public string Timing { get; set; }

    [Indexed]
    [MapTo("plannedDate")]
    public DateTimeOffset PlannedDate { get; set; }

    [MapTo("hostMessage")]
    public string? HostMessage { get; set; }

    [MapTo("joinRequests")]
    public IList<JoinRequest> JoinRequests { get; }

    [MapTo("connectedBooking")]
    public Booking? ConnectedBooking { get; set; }

    /// <summary>
    /// conversation id is the id of the chat group created for the planned or hosted booking
    /// </summary>
    [MapTo("conversationId")]
    public string? ConversationId { get; set; }

    // backlinks

    [MapTo("host")]
    public VenueUser? Host { get; set; }

    // local

    [Ignored]
    public string? TimingDisplay =>
        Timing switch
        {
            "Morning" => "6.00 - 12.00",
            "Afternoon" => "12.00 - 16.00",
            "Evening" => "16.00 - 22.00",
            _ => "6.00 - 12.00"
        };

    [Ignored]
    int accepted => JoinRequests.Where(jr => jr.IsApproved).Count();

    [Ignored]
    public int PendingRequests => JoinRequests.Where(jr => !jr.IsApproved).Count();

    [Ignored]
    // 1 is added to the accepted requests to include the host in the team status
    public string? TeamStatus => $"{accepted + 1} / {NumberOfPlayers} Going";

    bool isApplied;

    [Ignored]
    // use to show whether the hosted game is joined by the logged in user.
    public bool IsApplied
    {
        get => isApplied;
        set
        {
            if (value != isApplied)
            {
                isApplied = value;
                RaisePropertyChanged();
            }
        }
    }

    bool isApproved;

    [Ignored]
    public bool IsApproved
    {
        get => isApproved;
        set
        {
            if (value != isApproved)
            {
                isApproved = value;
                RaisePropertyChanged();
            }
        }
    }

    public string? VenueName => SelectedVenue.FullName;
    string date => PlannedDate.ToString("dd MMM yyyy");
    public string? BookingInformation =>
        $"{date} | {Timing} | {NumberOfPlayers.ToString()} players";
}
