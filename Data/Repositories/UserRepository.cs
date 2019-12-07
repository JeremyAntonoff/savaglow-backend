using System.Linq;
using System.Threading.Tasks;
using Savaglow.Data.Interfaces;
using Savaglow.Models;
using Microsoft.EntityFrameworkCore;

namespace Savaglow.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<User> GetUser(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user;

        }
    }
}