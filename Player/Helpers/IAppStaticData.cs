namespace Player.Helpers
{
    public interface IAppStaticData
    {
        Task<List<FavouriteGame>> GetGamesForUser();

        Task<List<AgeGroup>> GetAgeGroupsForUser();
    }
}
