using System;

namespace CourseHub.Domain.DTOs.Request
{
    public class CourseSearchRequestDTO
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public string? InstructorName { get; set; }
        public DateTime? EnrolledFrom { get; set; }
        public DateTime? EnrolledTo { get; set; }

        // Default paging
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}