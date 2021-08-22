using AdvancedTodoApplication.Models;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Repository
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryById(int id);
        Task<bool> DeleteCategory(int id);
    }
}