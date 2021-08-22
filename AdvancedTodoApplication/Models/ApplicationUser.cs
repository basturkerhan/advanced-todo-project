using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<UserBoard> UserBoards { get; set; }
        public virtual ICollection<UserToDo> UserTodos { get; set; }

    }
}
