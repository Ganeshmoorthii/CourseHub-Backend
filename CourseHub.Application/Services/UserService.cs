using AutoMapper;
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.Exceptions;
using CourseHub.Application.IServices;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.IRepository;
using System.Threading.Tasks;

namespace CourseHub.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateUserAsync(CreateUserRequestDTO dto)
        {
            if (dto == null)
                throw new ValidationException("User request cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("User email is required.");

            if (string.IsNullOrWhiteSpace(dto.UserName))
                throw new ValidationException("User name is required.");

            var user = _mapper.Map<User>(dto);
            await _userRepository.AddUserAsync(user);
        }
    }
}
