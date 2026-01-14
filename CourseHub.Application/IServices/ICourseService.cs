
using CourseHub.Application.Contracts;
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.DTOs.Response;
using CourseHub.Domain.DTOs.Request;

namespace CourseHub.Application.IServices
{
    public interface ICourseService
    {
        public Task CreateCourseAsync(CreateCourseRequestDTO courseRequestDTO);
        public Task<IEnumerable<CourseInfoDTO>> GetCoursesAsync(int page, int pageSize);
        public Task<PagedResult<SearchCoursesDTO>> SearchCourseAsync(CourseSearchRequestDTO dto);
    }
}
