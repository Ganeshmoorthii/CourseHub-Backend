using System.ComponentModel.DataAnnotations;

namespace CourseHub.Application.DTOs.Response
{
    public class InstructorInfoDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Bio { get; set; }
    }
}
