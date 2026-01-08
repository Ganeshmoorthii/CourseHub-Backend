using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Infrastructure.Repository
{
    public class SearchRepository : ISearchRepository
    {
        private readonly ILogger<SearchRepository> _logger;
        private readonly CourseHubDbContext _dbContext;
        public SearchRepository(ILogger<SearchRepository> logger, CourseHubDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserWithProfileAndEnrollmentsAsync(Guid userId)
        {
            return await _dbContext.Users
                .Include(u => u.Profile) 
                .Include(u => u.Enrollments)
                    .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> SearchUsersAsync(string? name, string? email, Guid? courseId)
        {
            var query = _dbContext.Users
                .Include(u => u.Profile)
                .Include(u => u.Enrollments)
                    .ThenInclude(e => e.Course)
                .AsQueryable();

            // Filter by name if provided
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(u => u.UserName.Contains(name));

            // Filter by email if provided
            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(u => u.Email.Contains(email));

            // Filter by enrolled course if provided
            if (courseId.HasValue)
                query = query.Where(u => u.Enrollments.Any(e => e.CourseId == courseId.Value));

            return await query.ToListAsync();
        }
    }
}
