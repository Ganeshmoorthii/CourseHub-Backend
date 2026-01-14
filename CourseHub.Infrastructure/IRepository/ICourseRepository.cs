using CourseHub.Domain.DTOs.Request;
using CourseHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Infrastructure.IRepository
{
    public interface ICourseRepository
    {
        Task AddCourseAsync(Course course);
        Task<bool> ExistsAsync(Guid courseId);
        Task<bool> ExistsByTitleAsync(string title);
        Task<IEnumerable<Course>> GetCourses(int page, int pageSize);
        Task<(List<Course> Courses, int TotalCount)> SearchCourseAsync(CourseSearchRequestDTO dto);
    }
}
