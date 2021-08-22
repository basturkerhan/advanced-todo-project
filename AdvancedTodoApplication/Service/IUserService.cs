using AdvancedTodoApplication.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Service
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
        Task<ApplicationUser> GetUserById(string id);
    }
}