using CourseHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseHub.Infrastructure.IRepository
{
    public interface ISearchRepository
    {
        public Task<User?> GetUserWithProfileAndEnrollmentsAsync(Guid userId);
        public Task<List<User>> SearchUsersAsync(string? name, string? email, Guid? courseId);
    }
}
