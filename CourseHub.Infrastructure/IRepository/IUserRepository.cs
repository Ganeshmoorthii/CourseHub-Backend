using CourseHub.Application.DTOs.Request;
using CourseHub.Domain.Entities;

public interface IUserRepository
{
    Task AddUserAsync(User newUser);

    Task<bool> ExistsAsync(Guid userId);

    Task<User?> GetUserWithProfileAndEnrollmentsAsync(Guid userId);

    Task<(List<User> Users, int TotalCount)> SearchUsersAsync(
        UserSearchRequestDTO request
    );
}
