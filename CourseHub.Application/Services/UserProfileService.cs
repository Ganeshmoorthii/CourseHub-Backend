using AutoMapper;
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.Exceptions;
using CourseHub.Application.IServices;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.IRepository;

namespace CourseHub.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public UserProfileService(
            IUserProfileRepository userProfileRepository,
            IMapper mapper)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task CreateUserProfileAsync(CreateUserProfileDTO dto)
        {
            if (dto == null)
                throw new ValidationException("User profile request cannot be null.");

            var exists = await _userProfileRepository.ExistsByUserIdAsync(dto.UserId);
            if (exists)
                throw new ConflictException("User profile already exists for this user.");

            var userProfile = _mapper.Map<UserProfile>(dto);
            await _userProfileRepository.CreateUserProfile(userProfile);
        }
    }
}
