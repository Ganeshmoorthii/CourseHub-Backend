using System;
using System.ComponentModel.DataAnnotations;

namespace CourseHub.Application.DTOs.Response
{
    public class CourseInfoDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        // Matches domain Course.Price
        public decimal Price { get; set; }

        [Required]
        public Guid InstructorId { get; set; }
    }
}
