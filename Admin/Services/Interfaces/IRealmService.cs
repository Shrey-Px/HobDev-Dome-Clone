namespace Admin.Services.Interfaces
{
    public interface IRealmService
    {
        public FlexibleSyncConfiguration? Config { get; }

        public User? RealmUser { get; }
    }
}
