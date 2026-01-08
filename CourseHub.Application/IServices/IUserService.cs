using CourseHub.Application.Contracts;
using CourseHub.Application.DTOs.Request;
using CourseHub.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.IServices
{
    public interface IUserService
    {
        public Task CreateUserAsync(CreateUserRequestDTO dto);
        Task<PagedResult<UserSearchDTO>> SearchUsersAsync(UserSearchRequestDTO request);
    }
}
