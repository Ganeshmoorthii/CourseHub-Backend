using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.DTOs.Response
{
    public class EnrollmentInfoDTO
    {
        public CourseInfoDTO Course { get; set; } = null!;
        public DateTime EnrolledAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
