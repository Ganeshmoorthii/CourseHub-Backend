using System.ComponentModel.DataAnnotations;

namespace CourseHub.Application.DTOs.Response
{
    public class SearchCoursesDTO
    {
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        // Use decimal to match domain Course.Price
        public decimal? Price { get; set; }

        public InstructorInfoDTO? InstructorInfo { get; set; }
    }
}
