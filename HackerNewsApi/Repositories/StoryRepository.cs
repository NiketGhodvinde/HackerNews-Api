using HackerNewsApi.Config;
using HackerNewsApi.Interface;
using HackerNewsApi.Model;
using Microsoft.Extensions.Options;

namespace HackerNewsApi.Repositories
{
    /// <summary>
    /// Repository for fetching stories from the Hacker News API.
    /// </summary>
    public class StoryRepository : IStoryRepository
    {
        private readonly HttpClient _httpClient;
        private readonly Settings _settings;
        private readonly ILogger<StoryRepository> _logger;

        public StoryRepository(HttpClient httpClient, IOptions<Settings> options, ILogger<StoryRepository> logger)
        {
            _httpClient = httpClient;
            _settings = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of new story IDs from Hacker News.
        /// </summary>
        /// <returns>List of new story IDs.</returns>
        public async Task<IEnumerable<int>> GetNewStoryIdsAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<int>>("newstories.json");
                return result ?? new List<int>();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogWarning(httpEx, "HTTP error while fetching new story IDs.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching new story IDs.");
            }

            return Enumerable.Empty<int>();
        }

        /// <summary>
        /// Retrieves a story by its ID.
        /// </summary>
        /// <param name="id">The story ID.</param>
        /// <returns>The story DTO or null if not found.</returns>
        public async Task<StoryDto?> GetStoryByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<StoryDto>($"item/{id}.json");
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogWarning(httpEx, $"HTTP error while fetching story with ID {id}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while fetching story with ID {id}.");
            }

            return null;
        }
    }
}
