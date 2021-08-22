using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvancedTodoApplication.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Kategori Adı")]
        public string CategoryName { get; set; }

        public List<ToDo> Todos { get; set; }

    }
}
