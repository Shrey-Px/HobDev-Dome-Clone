using System;

namespace Dome.Shared.Synced.Player;

public partial class JoinRequest : IEmbeddedObject
{
    [MapTo("appliedBy")]
    public VenueUser? AppliedBy { get; set; }

    [MapTo("isApproved")]
    public bool IsApproved { get; set; }

    [MapTo("applicantMessage")]
    public string? ApplicantMessage { get; set; }
}
