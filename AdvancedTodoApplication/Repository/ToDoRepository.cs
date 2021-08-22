using AdvancedTodoApplication.Infrastructure;
using AdvancedTodoApplication.Models;
using AdvancedTodoApplication.Service;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Repository
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoContext _context;
        private readonly IUserService _userService;

        public ToDoRepository(  ToDoContext context,
                                IUserService userService  )
        {
            _context     = context;
            _userService = userService;
        }

        public async Task<ToDo> GetToDoById(int id)
        {
            return await _context.ToDo.Where(x => x.Id == id)
                .Select(todo => new ToDo()
                {
                    Id          = todo.Id,
                    Title       = todo.Title,
                    CreatedAt   = todo.CreatedAt,
                    FinishedAt  = todo.FinishedAt,
                    Deadline    = todo.Deadline,
                    Description = todo.Description,
                    IsChecked   = todo.IsChecked,
                    OwnerId     = todo.OwnerId,
                    // todo members
                    ToDoUsers   = todo.ToDoUsers.Select(member => new UserToDo()
                    {
                        UserId  = member.UserId,
                        ToDoId  = member.ToDoId,
                        ApplicationUser = member.ApplicationUser
                    }).ToList(),
                }).FirstOrDefaultAsync();
        }


        public async Task<bool> IsUserMemberToDo(int todoId)
        {
            string userId = _userService.GetUserId();
            ToDo todo = await GetToDoById(todoId);
            int count = (from ut in _context.UserToDo where ut.UserId == userId && ut.ToDoId == todoId select ut).Count();

            return (count > 0);
        }


        public async Task<bool> IsUserMemberToDo(string userId, int todoId)
        {
            ToDo todo = await GetToDoById(todoId);
            int count = (from ut in _context.UserToDo where ut.UserId == userId && ut.ToDoId == todoId select ut).Count();

            return (count > 0);
        }


        public async Task<bool> DeleteToDo(int id)
        {
            ToDo item = await _context.ToDo.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            // mevcut olduğu durumda kartın silinmesi ve veritabanının kaydedilmesi
            IQueryable<UserToDo> userToDoItems = from b in _context.UserToDo where b.ToDoId == id select b;
            List<UserToDo> userToDoList = await userToDoItems.ToListAsync();
            foreach (UserToDo userToDo in userToDoList)
            {
                _context.UserToDo.Remove(userToDo);
            }

            _context.ToDo.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteToDo(ToDo item)
        {
            IQueryable<UserToDo> userToDoItems = from b in _context.UserToDo where b.ToDoId == item.Id select b;
            List<UserToDo> userToDoList = await userToDoItems.ToListAsync();
            foreach (UserToDo userToDo in userToDoList)
            {
                _context.UserToDo.Remove(userToDo);
            }

            _context.ToDo.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddUserToToDo(string userId, int todoId)
        {
            ApplicationUser user = await _userService.GetUserById(userId);
            ToDo todo = await GetToDoById(todoId);

            if (todo == null || user == null)
            {
                return false;
            }

            UserToDo userToDo = new UserToDo()
            {
                UserId = userId,
                ToDoId = todoId,
                ApplicationUser = user,
            };

            todo.ToDoUsers.Add(userToDo);
            _context.UserToDo.Add(userToDo);
            _context.Update(todo);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> RemoveUserFromToDo(string userId, int todoId)
        {
            ApplicationUser user = await _userService.GetUserById(userId);
            ToDo todo = await GetToDoById(todoId);
            UserToDo removeUserToDo = null;

            if (todo == null || user == null)
            {
                return false;
            }

            bool isUserMemberToDo = await IsUserMemberToDo(userId, todoId);
            if (!isUserMemberToDo)
            {
                return false;
            }

            if( string.Compare(todo.OwnerId, userId) == 0 )
            {
                return false;
            }

            //IQueryable<UserToDo> removeUserToDo = from ut in _context.UserToDo where ut.ToDoId == todoId && ut.UserId == userId select ut;


            //kartın tüm üyelerini gez ve üye ID si verilene eşit olanı seç
            foreach (UserToDo userToDo in todo.ToDoUsers)
            {
                if (userToDo.UserId == userId)
                {
                    removeUserToDo = userToDo;
                }
            }

            if (removeUserToDo == null)
            {
                return false;
            }

            // usertodo tablosundan sil, ilgili kullanıcıdan sil
            todo.ToDoUsers.Remove(removeUserToDo);
            _context.UserToDo.Remove(removeUserToDo);

            _context.ToDo.Update(todo);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
