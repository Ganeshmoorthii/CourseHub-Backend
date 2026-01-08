using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourseHub.Infrastructure.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ILogger<CourseRepository> _logger;
        private readonly CourseHubDbContext _dbContext;
        public CourseRepository(ILogger<CourseRepository> logger, CourseHubDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task AddCourseAsync(Domain.Entities.Course course)
        {
            _logger.LogInformation("AddCourseAsync method called in CourseRepository.");
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Course added to the database successfully.");
        }
        public async Task<bool> ExistsAsync(Guid courseId)
        {
            _logger.LogInformation("ExistsAsync method called in CourseRepository.");
            var exists = await Task.FromResult(_dbContext.Courses.Any(c => c.Id == courseId));
            _logger.LogInformation($"Course existence check completed: {exists}");
            return exists;
        }

        public async Task<bool> ExistsByTitleAsync(string title)
        {
            _logger.LogInformation("ExistsAsync method called in CourseRepository.");
            var exists = await Task.FromResult(_dbContext.Courses.Any(c => c.Title == title));
            return exists;
        }

        public async Task<IEnumerable<Course>> GetCourses(int page, int pageSize)
        {
            return await _dbContext.Courses
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
