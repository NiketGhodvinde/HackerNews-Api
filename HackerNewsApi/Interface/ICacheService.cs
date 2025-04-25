namespace HackerNewsApi.Interface
{
    public interface ICacheService
    {
        Task<IEnumerable<int>> GetOrAddNewStoryIdsAsync();
    }
}
