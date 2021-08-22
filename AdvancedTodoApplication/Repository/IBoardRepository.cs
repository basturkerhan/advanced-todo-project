using AdvancedTodoApplication.Models;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Repository
{
    public interface IBoardRepository
    {
        Task<bool> IsUserAdminBoard(int boardId);
        Task<Board> GetBoardById(int id);
        Task<bool> IsUserMemberBoard(int boardId);
        Task<bool> IsUserMemberBoard(string userId, int boardId);
        Task<bool> IsCategoryOwner(int categoryId, int boardId);
        Task<bool> AddUserToBoard(string userId, int boardId);
        Task<bool> RemoveUserFromBoard(string userId, int boardId);
        Task<bool> DeleteBoard(int id);
    }
}