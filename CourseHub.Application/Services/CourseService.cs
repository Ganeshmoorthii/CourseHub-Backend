using AutoMapper;
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.DTOs.Response;
using CourseHub.Application.Exceptions;
using CourseHub.Application.IServices;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.IRepository;

namespace CourseHub.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(
            ICourseRepository courseRepository,
            IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CourseInfoDTO>> GetCoursesAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
                throw new ValidationException("Page and pageSize must be greater than zero.");

            var courses = await _courseRepository.GetCourses(page, pageSize);
            return _mapper.Map<IEnumerable<CourseInfoDTO>>(courses);
        }


        public async Task CreateCourseAsync(CreateCourseRequestDTO courseRequestDTO)
        {
            if (courseRequestDTO == null)
                throw new ValidationException("Course request cannot be null.");

            if (string.IsNullOrWhiteSpace(courseRequestDTO.Title))
                throw new ValidationException("Course title is required.");

            var exists = await _courseRepository.ExistsByTitleAsync(courseRequestDTO.Title);
            if (exists)
                throw new ConflictException("A course with the same title already exists.");

            var courseEntity = _mapper.Map<Course>(courseRequestDTO);
            await _courseRepository.AddCourseAsync(courseEntity);
        }
    }
}
