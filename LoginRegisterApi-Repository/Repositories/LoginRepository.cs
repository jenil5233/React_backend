using LoginRegisterApi.Data;
using LoginRegisterApi_Entities.Models;
using LoginRegisterApi_Repository.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace LoginRegisterApi_Repository.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _db;

        public LoginRepository(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<LoginDetail> GetDetails(string email, string password)
        {
            var user = await _db.LoginDetails.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
            return user;
        }

        public async Task<LoginDetail> GetUserLoginByEmail(string email)
        {
            var user = await _db.LoginDetails.FirstOrDefaultAsync(a => a.Email == email);
            return user;
        }

        public async Task<LoginDetail> CreateUserLogin(LoginDetail loginDetail)
        {
            await _db.LoginDetails.AddAsync(loginDetail);
            await _db.SaveChangesAsync();
            return loginDetail;
        }

        public async Task<bool> RemoveUserLogin(LoginDetail loginDetail)
        {
            _db.LoginDetails.Remove(loginDetail);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<LoginDetail> UpdateUserLogin(LoginDetail loginDetail)
        {
            _db.LoginDetails.Update(loginDetail);
            await _db.SaveChangesAsync();
            return loginDetail;
        }

    }
}