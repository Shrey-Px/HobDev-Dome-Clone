namespace Dome.Shared.Synced
{
    public partial class Coach : IRealmObject
    {
        private Coach() { }

        public Coach(
            ObjectId id,
            string coachName,
            string gameCategory,
            string coachDescription,
            Venue venue,
            string competancy,
            string phoneCode,
            string contactNumber,
            string email
        )
        {
            Id = id;
            CoachName = coachName;
            GameCategory = gameCategory;
            CoachDescription = coachDescription;
            Venue = venue;
            Competancy = competancy;
            PhoneCode = phoneCode;
            ContactNumber = contactNumber;
            Email = email;
        }

        [PrimaryKey]
        [MapTo("_id")]
        public ObjectId Id { get; set; }

        [MapTo("coachName")]
        public string CoachName { get; set; }

        [Indexed]
        [MapTo("gameCategory")]
        public string GameCategory { get; set; }

        [MapTo("coachDescription")]
        public string CoachDescription { get; set; }

        [MapTo("venue")]
        public Venue? Venue { get; set; }

        [MapTo("competancy")]
        public string Competancy { get; set; }

        [MapTo("phoneCode")]
        public string PhoneCode { get; set; }

        [MapTo("contactNumber")]
        public string ContactNumber { get; set; }

        [MapTo("email")]
        public string Email { get; set; }
    }
}
