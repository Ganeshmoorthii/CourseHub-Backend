using CourseHub.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchService _searchService;
        public SearchController(ILogger<SearchController> logger, ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> SearchUserDetails(Guid userId)
        {
            _logger.LogInformation("SearchCoursesByUserId method called in SearchController.");
            var result = await _searchService.SearchUserDetailsAsync(userId);
            return Ok(result);
        }
    }
}
