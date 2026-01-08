using CourseHub.Application.DTOs.Response;
using CourseHub.Application.Exceptions;
using CourseHub.Application.IServices;
using CourseHub.Infrastructure.IRepository;
using CourseHub.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ILogger<SearchService> _logger;
        private readonly ISearchRepository _searchRepository;
        public SearchService(ILogger<SearchService> logger, ISearchRepository searchRepository)
        {
            _logger = logger;
            _searchRepository = searchRepository;
        }

        public async Task<UserSearchDTO?> SearchUserDetailsAsync(Guid userId)
        {
            var user = await _searchRepository.GetUserWithProfileAndEnrollmentsAsync(userId);
            if (user == null)
                throw new NotFoundException("User", userId);

            var dto = new UserSearchDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Profile = user.Profile == null ? null : new UserProfileInfoDTO
                {
                    FullName = user.Profile.FirstName + " " + user.Profile.LastName,
                    LastName = user.Profile.LastName,
                    Bio = user.Profile.Bio,
                    DateOfBirth = user.Profile.DateOfBirth
                },
                Enrollments = user.Enrollments.Select(e => new EnrollmentInfoDTO
                {
                    Course = new CourseInfoDTO
                    {
                        Id = e.Course.Id,
                        Title = e.Course.Title,
                        Description = e.Course.Description,
                        Price = e.Course.Price,
                        InstructorId = e.Course.InstructorId
                    },
                    EnrolledAt = e.EnrolledAt,
                    Status = e.Status.ToString()
                }).ToList()
            };

            return dto;
        }
    }
}
