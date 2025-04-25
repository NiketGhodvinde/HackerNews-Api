using HackerNewsApi.Interface;
using HackerNewsApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsApi.Controllers
{
    /// <summary>
    /// Controller to manage story-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;
        private readonly ILogger<StoriesController> _logger;

        public StoriesController(IStoryService storyService, ILogger<StoriesController> logger)
        {
            _storyService = storyService;
            _logger = logger;
        }

        /// <summary>
        /// Gets the total number of stories. Supports optional search filter.
        /// </summary>
        /// <param name="search">Optional search keyword.</param>
        /// <returns>Total number of stories.</returns>
        [HttpGet("story-count")]
        public async Task<IActionResult> GetStoryCount([FromQuery] string? search)
        {
            try
            {
                var totalCount = await _storyService.GetTotalStoryCountAsync(search);
                return Ok(totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching the total story count.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching the story count.",
                    details = ex.Message
                });
            }
        }

        /// <summary>
        /// Gets a paginated list of stories with optional search filtering.
        /// </summary>
        /// <param name="page">Page number (default is 1).</param>
        /// <param name="pageSize">Number of stories per page (default is 10).</param>
        /// <param name="search">Optional search term.</param>
        /// <returns>Paginated list of stories.</returns>
        [HttpGet("stories")]
        public async Task<IActionResult> GetStories([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = "")
        {
            try
            {
                var stories = await _storyService.GetStoriesAsync(page, pageSize, search);
                return Ok(stories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stories.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "An error occurred while fetching stories.",
                    details = ex.Message
                });
            }
        }
    }


}
