using System.Threading.Tasks;
using Savaglow.Models;

namespace Savaglow.Data.Interfaces
{
    public interface IUserRepository
    {
         Task<User> GetUser(string userId);
    }
}