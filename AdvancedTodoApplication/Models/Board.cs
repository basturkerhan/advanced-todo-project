using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvancedTodoApplication.Models
{
    public class Board
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Required(ErrorMessage = "Lutfen pano ismini giriniz")]
        [Display(Name = "Pano İsmi")]
        public string Name { get; set; }
        [Display(Name = "Pano Açıklaması")]
        public string Description { get; set; }
        public List<Category> BoardCategories { get; set; }
        public string OwnerId { get; set; }

        public ICollection<UserBoard> UserBoards { get; set; }

    }
}
