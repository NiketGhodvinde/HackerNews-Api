using HackerNewsApi.Interface;
using HackerNewsApi.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace HackerNewsApi.Service
{
    /// <summary>
    /// Service layer for handling story-related business logic.
    /// </summary>
    public class StoryService : IStoryService
    {
        private readonly IStoryRepository _repository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<StoryService> _logger;

        public StoryService(IStoryRepository repository, ICacheService cacheService, ILogger<StoryService> logger)
        {
            _repository = repository;
            _cacheService = cacheService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the total count of new stories.
        /// </summary>
        /// <param name="search">Optional search filter.</param>
        /// <returns>Total story count.</returns>
        public async Task<int> GetTotalStoryCountAsync(string search = "")
        {
            try
            {
                var allStories = await _repository.GetNewStoryIdsAsync();
                return allStories.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving total story count.");
                return 0;
            }
        }

        /// <summary>
        /// Gets a story by ID.
        /// </summary>
        /// <param name="id">Story ID.</param>
        /// <returns>StoryDto if found, null otherwise.</returns>
        public Task<StoryDto?> GetStoryByIdAsync(int id)
        {
            return _repository.GetStoryByIdAsync(id);
        }

        /// <summary>
        /// Gets paginated stories, with optional search filtering.
        /// </summary>
        /// <param name="page">Page number (1-based).</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="search">Optional search keyword.</param>
        /// <returns>Filtered list of stories.</returns>
        public async Task<IEnumerable<StoryDto>> GetStoriesAsync(int page, int pageSize, string search = "")
        {
            try
            {
                var allIds = await _cacheService.GetOrAddNewStoryIdsAsync();

                var pagedIds = allIds.Skip((page - 1) * pageSize).Take(pageSize);
                var stories = await Task.WhenAll(pagedIds.Select(id => _repository.GetStoryByIdAsync(id)));

                var filteredStories = stories
                    .Where(s => s != null && !string.IsNullOrWhiteSpace(s.Title))
                    .Where(s => string.IsNullOrWhiteSpace(search) || s!.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                return filteredStories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving stories.");
                return Enumerable.Empty<StoryDto>();
            }
        }
    }
}
