using AutoMapper;
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.Exceptions;
using CourseHub.Application.IServices;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.IRepository;

namespace CourseHub.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IMapper _mapper;

        public InstructorService(
            IInstructorRepository instructorRepository,
            IMapper mapper)
        {
            _instructorRepository = instructorRepository;
            _mapper = mapper;
        }

        public async Task CreateInstructorAsync(CreateInstructorRequestDTO dto)
        {
            if (dto == null)
                throw new ValidationException("Instructor request cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("Instructor name is required.");

            var instructor = _mapper.Map<Instructor>(dto);
            await _instructorRepository.CreateInstructor(instructor);
        }
    }
}
