using AdvancedTodoApplication.Infrastructure;
using AdvancedTodoApplication.Models;
using AdvancedTodoApplication.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Repository
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ToDoContext _context;
        private readonly IUserService _userService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IToDoRepository _toDoRepository;
        public BoardRepository( ToDoContext context,
                                IUserService userService,
                                ICategoryRepository categoryRepository,
                                IToDoRepository toDoRepository )
        {
            _context = context;
            _userService = userService;
            _categoryRepository = categoryRepository;
            _toDoRepository = toDoRepository;
        }

        public async Task<Board> GetBoardById(int id)
        {

            return await _context.Board.Where(x => x.Id == id)
                 .Select(board => new Board()
                 {
                     Id          = board.Id,
                     Name        = board.Name,
                     Description = board.Description,
                     OwnerId     = board.OwnerId,
                     // categories start
                     BoardCategories  = board.BoardCategories.Select(category => new Category()
                     {
                         Id           = category.Id,
                         CategoryName = category.CategoryName,
                         // todos start
                         Todos        = category.Todos.Select(todo => new ToDo()
                         {
                             Id          = todo.Id,
                             Title       = todo.Title,
                             Description = todo.Description,
                             CreatedAt   = todo.CreatedAt,
                             FinishedAt  = todo.FinishedAt,
                             Deadline    = todo.Deadline,
                             IsChecked   = todo.IsChecked,
                             OwnerId     = todo.OwnerId,
                             // todo members start
                             ToDoUsers = todo.ToDoUsers.Select(member => new UserToDo()
                             {
                                 UserId          = member.UserId,
                                 ToDoId          = member.ToDoId,
                                 ApplicationUser = member.ApplicationUser
                             }).ToList(),
                             // todo members finish
                         }).ToList(),
                         // todos finish
                     }).ToList(),
                     // categories finish

                     //board members start
                     UserBoards = board.UserBoards.Select(uboard => new UserBoard()
                     {
                         UserId          = uboard.UserId,
                         BoardId         = uboard.BoardId,
                         ApplicationUser = uboard.ApplicationUser
                     }).ToList(),

                 }).FirstOrDefaultAsync();

        }


        public async Task<bool> IsUserAdminBoard(int boardId)
        {
            Board board     = await GetBoardById(boardId);
            string userId   = _userService.GetUserId();

            return (string.Compare(board.OwnerId, userId) == 0);
        }


        public async Task<bool> IsUserMemberBoard(int boardId)
        {
            Board board     = await GetBoardById(boardId);
            string userId   = _userService.GetUserId();
            int count       = (from ub in _context.UserBoard where ub.UserId == userId && ub.BoardId == boardId select ub).Count();

            return (count > 0);
        }

        public async Task<bool> IsUserMemberBoard(string userId, int boardId)
        {
            Board board = await GetBoardById(boardId);
            int count   = (from ub in _context.UserBoard where ub.UserId == userId && ub.BoardId == boardId select ub).Count();

            return (count > 0);
        }


        public async Task<bool> IsCategoryOwner(int categoryId, int boardId)
        {
            Board board  = await GetBoardById(boardId);
            bool isOwner = false;

            // panonun tüm kategorilerini gez verilen ID de kategori varsa seç
            foreach (Category boardCategory in board.BoardCategories)
            {
                if (boardCategory.Id == categoryId)
                {
                    isOwner = true;
                    break;
                }
            }

            return isOwner;
        }

        public async Task<bool> AddUserToBoard(string userId, int boardId)
        {
            // kullanıcı zaten o panoda var mı kontrolü
            bool isUserAlreadyMemberBoard = await IsUserMemberBoard(userId, boardId);
            if (isUserAlreadyMemberBoard)
            {
                return false;
            }
            // kontrol bitişi

            ApplicationUser user = await _userService.GetUserById(userId);
            Board board = await GetBoardById(boardId);

            if (board == null || user == null)
            {
                return false;
            }

            UserBoard userBoard = new UserBoard()
            {
                UserId          = user.Id,
                ApplicationUser = user,
                BoardId         = board.Id,
            };

            board.UserBoards.Add(userBoard);
            _context.UserBoard.Add(userBoard);

            _context.Update(board);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> RemoveUserFromBoard(string userId, int boardId)
        {
            ApplicationUser user = await _userService.GetUserById(userId);
            Board board = await GetBoardById(boardId);

            UserBoard removeBoard = null;

            if (board == null || user == null)
            {
                return false;
            }

            foreach (UserBoard userBoard in board.UserBoards)
            {
                if (userBoard.UserId == userId)
                {
                    removeBoard = userBoard;
                }
            }

            if (removeBoard == null)
            {
                return false;
            }

            // userboard tablosundan sil, kullanıcıdan sil, ilgili boarddan sil panodaki görev aldığı kartlardan o kullanıcıyı sil
            foreach(Category category in board.BoardCategories)
            {
                foreach( ToDo categoryTodo in category.Todos)
                {
                    await _toDoRepository.RemoveUserFromToDo(userId, categoryTodo.Id);
                }
            }


            board.UserBoards.Remove(removeBoard);
            _context.UserBoard.Remove(removeBoard);

            _context.Board.Update(board);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteBoard(int id)
        {
            Board board   = await GetBoardById(id);
            string userId = _userService.GetUserId();

            if (board == null)
            {
                return false;
            }

            if (string.Compare(userId, board.OwnerId) != 0)
            {
                return await RemoveUserFromBoard(userId, board.Id);
            }


            foreach (Category category in board.BoardCategories)
            {
                await _categoryRepository.DeleteCategory(category.Id);
            }

            IQueryable<UserBoard> userBoardItems = from b in _context.UserBoard where b.BoardId == id select b;
            List<UserBoard> userBoardList        = await userBoardItems.ToListAsync();
            foreach (UserBoard userBoard in userBoardList)
            {
                _context.UserBoard.Remove(userBoard);
            }

            Board item = await _context.Board.FindAsync(id);
            _context.Board.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
