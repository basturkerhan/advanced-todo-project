using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdvancedTodoApplication.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [Display(Name = "İş Kartı Başlığı*")]
        [Required(ErrorMessage = "Lütfen başlık giriniz")]
        public string Title { get; set; }

        [Display(Name = "İş Kartı Açıklaması (opsiyonel)")]
        public string Description { get; set; }

        [Display(Name = "Bu Görev Tamamlandı Mı?")]
        public bool IsChecked { get; set; }
        public string OwnerId { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FinishedAt { get; set; }

        [Display(Name = "Bitirilmesi Gereken Son Tarih (opsiyonel)")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddThh:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Deadline { get; set; }

        public ICollection<UserToDo> ToDoUsers { get; set; }

    }
}
