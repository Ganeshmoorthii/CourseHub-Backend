using CourseHub.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Application.IServices
{
    public interface ISearchService
    {
        public Task<UserSearchDTO?> SearchUserDetailsAsync(Guid userId);
    }
}
