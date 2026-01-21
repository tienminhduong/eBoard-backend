using eBoardAPI.Common;
using eBoardAPI.Context;
using eBoardAPI.Entities;
using eBoardAPI.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace eBoardAPI.Repositories
{
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        public async Task<bool> EmailExistsAsync(string email)
            => await dbContext.Users.AnyAsync(x => x.Email == email);

        public async Task<bool> PhoneExistsAsync(string phone)
            => await dbContext.Users.AnyAsync(x => x.PhoneNumber == phone);

        public async Task<Result> AddAsync(User user)
        {
            try
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return Result.Success();
            }
            catch
            {
                return Result.Failure("Failed to add user.");
            }
        }
    }

}
