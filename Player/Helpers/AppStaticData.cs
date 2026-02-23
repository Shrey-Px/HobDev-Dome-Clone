namespace Player.Helpers
{
    public class AppStaticData : IAppStaticData
    {
        public async Task<List<FavouriteGame>> GetGamesForUser()
        {
            return new List<FavouriteGame>
            {
                new FavouriteGame(gameName: "Archery"),
                new FavouriteGame(gameName: "Basketball"),
                new FavouriteGame(gameName: "Baseball"),
                new FavouriteGame(gameName: "Bowling"),
                new FavouriteGame(gameName: "Badminton"),
                new FavouriteGame(gameName: "Cricket"),
                new FavouriteGame(gameName: "Volleyball"),
                new FavouriteGame(gameName: "Pickleball"),
                new FavouriteGame(gameName: "Tennis"),
                new FavouriteGame(gameName: "Golf"),
                new FavouriteGame(gameName: "Hockey"),
                new FavouriteGame(gameName: "Others"),
            };
        }

        public async Task<List<AgeGroup>> GetAgeGroupsForUser()
        {
            return new List<AgeGroup>
            {
                new AgeGroup(ageGroupName: "15-20"),
                new AgeGroup(ageGroupName: "21-30"),
                new AgeGroup(ageGroupName: "31-40"),
                new AgeGroup(ageGroupName: "41-50"),
                new AgeGroup(ageGroupName: "50+"),
            };
        }
    }
}
