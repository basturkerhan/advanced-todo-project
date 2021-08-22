namespace AdvancedTodoApplication.Models
{
    public class UserBoard
    {
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }


        public int BoardId { get; set; }
        public Board Board { get; set; }

    }
}
