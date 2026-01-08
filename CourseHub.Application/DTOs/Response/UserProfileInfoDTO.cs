using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.DTOs.Response
{
    public class UserProfileInfoDTO
    {
        //public Guid UserId { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }
        
        [DataType(DataType.Date)]
        public DateOnly? DateOfBirth { get; set; }
    }
}
