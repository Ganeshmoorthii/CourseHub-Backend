
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.DTOs.Response;

namespace CourseHub.Application.IServices
{
    public interface ICourseService
    {
        public Task CreateCourseAsync(CreateCourseRequestDTO courseRequestDTO);
        public Task<IEnumerable<CourseInfoDTO>> GetCoursesAsync(int page, int pageSize);
    }
}
