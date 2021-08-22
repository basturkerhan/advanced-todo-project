using AdvancedTodoApplication.Infrastructure;
using AdvancedTodoApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService( IHttpContextAccessor httpContext, 
                            UserManager<ApplicationUser> userManager )
        {
            _httpContext = httpContext;
            _userManager = userManager;
        }


        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        public async Task<ApplicationUser> GetUserById(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            return user;
        }


        public bool IsAuthenticated()
        {
            return _httpContext.HttpContext.User.Identity.IsAuthenticated;
        }

    }
}
