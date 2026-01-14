using CourseHub.Application.DTOs.Request;
using CourseHub.Domain.Entities;
using CourseHub.Infrastructure.Data;
using CourseHub.Infrastructure.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

public class UserRepository : IUserRepository
{
    private readonly CourseHubDbContext _dbContext;

    public UserRepository(CourseHubDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddUserAsync(User newUser)
    {
        if(newUser == null)
        {
            throw new ArgumentNullException(nameof(User));
        }
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid userId)
    {
        return await _dbContext.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<User?> GetUserWithProfileAndEnrollmentsAsync(Guid userId)
    {
        return await _dbContext.Users
            .Include(u => u.Profile)
            .Include(u => u.Enrollments)
                .ThenInclude(e => e.Course)
                    .ThenInclude(c => c.Instructor)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<(List<User> Users, int TotalCount)> SearchUsersAsync(UserSearchRequestDTO request)
    {
        var query = _dbContext.Users
            .Include(u => u.Profile)
            .Include(u => u.Enrollments)
                .ThenInclude(e => e.Course)
                    .ThenInclude(c => c.Instructor)
            .AsQueryable();
        if (request.Id.HasValue)
        {
            query = query.Where(i => i.Id == request.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.UserName))
            query = query.Where(u => u.UserName.Contains(request.UserName));

        if (!string.IsNullOrWhiteSpace(request.Email))
            query = query.Where(u => u.Email.Contains(request.Email));

        if (request.DateOfBirth.HasValue)
        {
            var dob = request.DateOfBirth.Value;
            query = query.Where(u => u.Profile != null && u.Profile.DateOfBirth == dob);
        }

        if (!string.IsNullOrWhiteSpace(request.CourseTitle))
            query = query.Where(u =>
                u.Enrollments.Any(e =>
                    e.Course.Title.Contains(request.CourseTitle)));

        if (!string.IsNullOrWhiteSpace(request.InstructorName))
            query = query.Where(u =>
                u.Enrollments.Any(e =>
                    e.Course.Instructor.Name.Contains(request.InstructorName)));

        if (request.PriceFrom.HasValue)
            query = query.Where(u =>
                u.Enrollments.Any(e => e.Course.Price >= request.PriceFrom));

        if (request.PriceTo.HasValue)
            query = query.Where(u =>
                u.Enrollments.Any(e => e.Course.Price <= request.PriceTo));

        if (request.EnrolledFrom.HasValue)
            query = query.Where(u =>
                u.Enrollments.Any(e => e.EnrolledAt >= request.EnrolledFrom));

        if (request.EnrolledTo.HasValue)
            query = query.Where(u =>
                u.Enrollments.Any(e => e.EnrolledAt <= request.EnrolledTo));

        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.UserName)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (users, totalCount);
    }
    //public async Task<(List<User> Users, int TotalCount)> SearchUsersAsync(UserSearchRequestDTO request)
    //{
    //    //var query = _dbContext.Users
    //    //    .Include(u => u.Profile)
    //    //    .Include(u => u.Enrollments)
    //    //        .ThenInclude(e => e.Course)
    //    //            .ThenInclude(c => c.Instructor)
    //    //    .AsQueryable();
    //    var where = new StringBuilder("where i = 1 ");
    //    var parameter = new List<SqlParameter>();
    //    if (request.Id.HasValue)
    //    {
    //        where.Append(" AND  u.Id = @Id ");
    //        parameter.Add(new SqlParameter("@Id", request.Id.Value));
    //        //query = query.Where(i => i.Id == request.Id);
    //    }

    //    if (!string.IsNullOrWhiteSpace(request.UserName))
    //    {
    //        where.Append(" AND u.UserName LIKE @UserName");
    //        parameter.Add(new SqlParameter("@UserName", $"%{request.UserName}%"));
    //        //query = query.Where(u => u.UserName.Contains(request.UserName));
    //    }

    //    if (!string.IsNullOrWhiteSpace(request.Email))
    //    {
    //        where.Append(" AND u.Email LIKE @Email");
    //        parameter.Add(new SqlParameter("@Email", $"%{request.Email}%"));
    //        //query = query.Where(u => u.Email.Contains(request.Email));
    //    }
    //    if (request.DateOfBirth.HasValue)
    //    {
    //        where.Append(" AND u.DateOfBirth LIKE @Dob");
    //        parameter.Add(new SqlParameter("@Dob", request.DateOfBirth.Value));
    //        //var dob = request.DateOfBirth.Value;
    //        //query = query.Where(u => u.Profile != null && u.Profile.DateOfBirth == dob);
    //    }

    //    if (!string.IsNullOrWhiteSpace(request.CourseTitle))
    //    {
    //        where.Append(@"
    //        AND EXISTS (
    //            SELECT 1
    //            FROM Enrollments e
    //            JOIN Courses c ON e.CourseId = c.Id
    //            WHERE e.UserId = u.Id
    //              AND c.Title LIKE @CourseTitle
    //        )");
    //        parameter.Add(new SqlParameter("@CourseTitle", $"%{request.CourseTitle}%"));
    //        //query = query.Where(u =>
    //        //    u.Enrollments.Any(e =>
    //        //        e.Course.Title.Contains(request.CourseTitle)));

    //    }
    //    if (!string.IsNullOrWhiteSpace(request.InstructorName))
    //    {
    //        e.Append(@"
    //        AND EXISTS (
    //            SELECT 1
    //            FROM Enrollments e
    //            JOIN Courses c ON e.CourseId = c.Id
    //            JOIN Instructors i ON c.InstructorId = i.Id
    //            WHERE e.UserId = u.Id
    //              AND i.Name LIKE @InstructorName
    //        )");
    //        parameter.Add(new SqlParameter("@InstructorName", $"%{request.InstructorName}%"));
    //        //query = query.Where(u =>
    //        //    u.Enrollments.Any(e =>
    //        //        e.Course.Instructor.Name.Contains(request.InstructorName)));
    //    }

    //    if (request.PriceFrom.HasValue || request.PriceTo.HasValue)
    //    {
    //        where.Append(@"
    //        AND EXISTS (
    //            SELECT 1
    //            FROM Enrollments e
    //            JOIN Courses c ON e.CourseId = c.Id
    //            WHERE e.UserId = u.Id");

    //        if (request.PriceFrom.HasValue)
    //        {
    //            where.Append(" AND c.Price >= @PriceFrom ");
    //            parameter.Add(new SqlParameter("@PriceFrom", request.PriceFrom.Value));
    //        }

    //        if (request.PriceTo.HasValue)
    //        {
    //            where.Append(" AND c.Price <= @PriceTo ");
    //            parameter.Add(new SqlParameter("@PriceTo", request.PriceTo.Value));
    //        }

    //        where.Append(")");
    //    }

    //    if (request.EnrolledFrom.HasValue || request.EnrolledTo.HasValue)
    //    {
    //        where.Append(@"
    //        AND EXISTS (
    //            SELECT 1
    //            FROM Enrollments e
    //            WHERE e.UserId = u.Id");

    //        if (request.EnrolledFrom.HasValue)
    //        {
    //            where.Append(" AND e.EnrolledAt >= @EnrolledFrom ");
    //            parameter.Add(new SqlParameter("@EnrolledFrom", request.EnrolledFrom.Value));
    //        }

    //        if (request.EnrolledTo.HasValue)
    //        {
    //            where.Append(" AND e.EnrolledAt <= @EnrolledTo ");
    //            parameter.Add(new SqlParameter("@EnrolledTo", request.EnrolledTo.Value));
    //        }

    //        where.Append(")");
    //    }

    //    var countSql = $@"
    //        Select Count(1)
    //        From User u
    //        Left Join UserProfiles p on u.Id = p.UserId;
    //        ";

    //    var totalCount = await _dbContext.Database
    //        .SqlQueryRaw<int>(countSql, parameter.ToArray())
    //        .SingleAsync();

    //    var dataSql = $@"
    //    Select u.*
    //    from Users u
    //    left join UserProfiles p ON u.Id = p.UserId
    //    {where}
    //    order by u.UserName
    //    offset @Skip rows fetch next @Take rows only";

    //    parameter.Add(new SqlParameter("@Skip", (request.Page - 1) * request.PageSize));
    //    parameter.Add(new SqlParameter("@Take", request.PageSize));

    //    var users = await _dbContext.Users
    //        .FromSqlRaw(dataSql, parameter.ToArray())
    //        .AsNoTracking()
    //        .ToListAsync();

    //    return (users, totalCount);
    //}
}
