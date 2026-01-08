using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.DTOs.Response
{
    public class UserSearchDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserProfileInfoDTO? Profile { get; set; }
        public ICollection<EnrollmentInfoDTO> Enrollments { get; set; } = new List<EnrollmentInfoDTO>();
    }
}
