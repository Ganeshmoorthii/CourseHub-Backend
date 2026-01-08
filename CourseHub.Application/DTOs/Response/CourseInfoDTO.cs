using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public decimal Price { get; set; }

        [Required]
        public Guid InstructorId { get; set; }
    }
}
