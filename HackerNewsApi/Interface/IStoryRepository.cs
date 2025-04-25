using HackerNewsApi.Model;

namespace HackerNewsApi.Interface
{
    public interface IStoryRepository
    {
        Task<IEnumerable<int>> GetNewStoryIdsAsync();
        Task<StoryDto?> GetStoryByIdAsync(int id);
    }
}
