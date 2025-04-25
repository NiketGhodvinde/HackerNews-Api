using HackerNewsApi.Interface;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsApi.Service
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IStoryRepository _repository;

        public CacheService(IMemoryCache cache, IStoryRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }

        public Task<IEnumerable<int>> GetOrAddNewStoryIdsAsync()
        {
            return _cache.GetOrCreateAsync("NewStories", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                return await _repository.GetNewStoryIdsAsync();
            });
        }
    }
}
