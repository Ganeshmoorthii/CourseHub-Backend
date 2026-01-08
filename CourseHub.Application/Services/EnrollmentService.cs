using CourseHub.Application.DTOs.Request;
using CourseHub.Application.Exceptions;
using CourseHub.Application.IServices;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.IRepository;

namespace CourseHub.Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;

        public EnrollmentService(
            IEnrollmentRepository enrollmentRepository,
            IUserRepository userRepository,
            ICourseRepository courseRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _userRepository = userRepository;
            _courseRepository = courseRepository;
        }

        public async Task CreateEnrollmentAsync(CreateEnrollmentRequestDTO dto)
        {
            if (dto == null)
                throw new ValidationException("Enrollment request cannot be null.");

            var userExists = await _userRepository.ExistsAsync(dto.UserId);
            if (!userExists)
                throw new NotFoundException("User", dto.UserId);

            var courseExists = await _courseRepository.ExistsAsync(dto.CourseId);
            if (!courseExists)
                throw new NotFoundException("Course", dto.CourseId);

            var alreadyEnrolled =
                await _enrollmentRepository.ExistsAsync(dto.UserId, dto.CourseId);

            if (alreadyEnrolled)
                throw new ConflictException("User is already enrolled in this course.");

            var enrollment = new Enrollment
            {
                UserId = dto.UserId,
                CourseId = dto.CourseId,
                EnrolledAt = DateTime.UtcNow,
                Status = "Active"
            };

            await _enrollmentRepository.AddAsync(enrollment);
        }
    }
}
