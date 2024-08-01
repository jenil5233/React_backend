using LoginRegisterApi.Data;
using LoginRegisterApi.Models;
using LoginRegisterApi_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace LoginRegisterApi_Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserDetail> CreateUser(UserDetail user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<UserDetail>> GetAllUsers()
        {
            return await _dbContext.Users.OrderBy(a => a.Id).ToListAsync();
        }

        public async Task<UserDetail> GetUser(string userName)
        {
            UserDetail user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userName);
            return user;
        }

        public async Task<UserDetail> GetUserById(int id)
        {
            UserDetail user = await _dbContext.Users.Include(a => a.LoginDetail).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<UserDetail> GetUserByEmail(string email)
        {
            UserDetail user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
            return user;
        }

        public async Task<bool> RemoveUser(UserDetail user)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserDetail> UpdateUser(UserDetail user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
