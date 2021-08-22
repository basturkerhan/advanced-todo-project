namespace AdvancedTodoApplication.Models
{
    public class UserToDo
    {
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public int ToDoId { get; set; }
        public ToDo ToDo { get; set; }
    }
}
