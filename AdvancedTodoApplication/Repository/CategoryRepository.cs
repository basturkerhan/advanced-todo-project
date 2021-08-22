using AdvancedTodoApplication.Infrastructure;
using AdvancedTodoApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ToDoContext _context;
        private readonly IToDoRepository _toDoRepository;

        public CategoryRepository(  ToDoContext context,
                                    IToDoRepository toDoRepository  )
        {
            _context        = context;
            _toDoRepository = toDoRepository;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Category.Where(x => x.Id == id)
                .Select(category => new Category()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName,
                    // todos start
                    Todos = category.Todos.Select(todo => new ToDo()
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
                        ToDoUsers   = todo.ToDoUsers.Select(member => new UserToDo()
                        {
                            UserId  = member.UserId,
                            ToDoId  = member.ToDoId,
                            ApplicationUser = member.ApplicationUser
                        }).ToList(),
                        // todo members finish
                    }).ToList(),
                    // todos finish

                }).FirstOrDefaultAsync();
        }


        public async Task<bool> DeleteCategory(int id)
        {
            Category category = await GetCategoryById(id);

            if (category == null)
            {
                return false;
            }

            foreach (ToDo toDo in category.Todos)
            {
                await _toDoRepository.DeleteToDo(toDo.Id);
            }

            Category temp = await _context.Category.FindAsync(id);
            _context.Category.Remove(temp);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
