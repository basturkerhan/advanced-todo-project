using AdvancedTodoApplication.Models;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Repository
{
    public interface IToDoRepository
    {
        Task<bool> DeleteToDo(int id);
        Task<bool> DeleteToDo(ToDo item);
        Task<ToDo> GetToDoById(int id);
        Task<bool> IsUserMemberToDo(string userId, int todoId);
        Task<bool> IsUserMemberToDo(int todoId);
        Task<bool> AddUserToToDo(string userId, int todoId);
        Task<bool> RemoveUserFromToDo(string userId, int todoId);
    }
}