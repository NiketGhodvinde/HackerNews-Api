using HackerNewsApi.Model;

namespace HackerNewsApi.Interface
{
    public interface IStoryService
    {
        Task<StoryDto?> GetStoryByIdAsync(int id);
        Task<IEnumerable<StoryDto>> GetStoriesAsync(int page, int pageSize, string search = "");
        Task<int> GetTotalStoryCountAsync(string search = "");
    }
}
