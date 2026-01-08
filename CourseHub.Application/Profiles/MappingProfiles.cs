using AutoMapper;
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.DTOs.Response;
using CourseHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Course, CreateCourseRequestDTO>().ReverseMap();
            
            CreateMap<Enrollment, CreateEnrollmentRequestDTO>().ReverseMap();
            
            CreateMap<Instructor, CreateInstructorRequestDTO>().ReverseMap();
            
            CreateMap<CreateUserRequestDTO, User>()
            .ForMember(d => d.PasswordHash, o => o.Ignore());

            CreateMap<CreateUserProfileDTO, UserProfile>();

            CreateMap<Course, CourseInfoDTO>();


        }
    }
}
