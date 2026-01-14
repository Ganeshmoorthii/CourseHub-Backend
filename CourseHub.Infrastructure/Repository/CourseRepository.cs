using CourseHub.Domain.DTOs.Request;
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

        public async Task<(List<Course> Courses, int TotalCount)> SearchCourseAsync(CourseSearchRequestDTO dto)
        {
            var query = _dbContext.Courses
                .Include(c => c.Instructor)
                .AsQueryable();
            if(dto.Id.HasValue)
            {
                query = query.Where(c => c.Id == dto.Id.Value);
            }
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                query = query.Where(c => c.Title.Contains(dto.Title));
            }
            if (dto.PriceFrom.HasValue)
            {
                query = query.Where(c =>
                    c.Price >= dto.PriceFrom.Value
                );
            }
            if (dto.PriceTo.HasValue)
            {
                query = query.Where(c => c.Price <= dto.PriceTo.Value);
            }
            if(!string.IsNullOrWhiteSpace(dto.InstructorName))
            {
                query = query.Where(c => c.Instructor.Name.Contains(dto.InstructorName));
            }
            if(dto.EnrolledFrom.HasValue)
            {
                query = query.Where(c =>
                    c.Enrollments.Any(e => e.EnrolledAt >= dto.EnrolledFrom.Value)
                );
            }
            if(dto.EnrolledTo.HasValue)
            {
                query = query.Where(c =>
                    c.Enrollments.Any(e => e.EnrolledAt <= dto.EnrolledTo.Value)
                );
            }
            var totalCount = await query.CountAsync();
            var courses = await query
                .OrderBy(c => c.Title)
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();
            return (courses, totalCount);

        }
    }
}
