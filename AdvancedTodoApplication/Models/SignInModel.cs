using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Models
{
    public class SignInModel
    {
        [Required(ErrorMessage = "Lutfen email adresinizi giriniz")]
        [Display(Name = "Email adresiniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lutfen şifrenizi giriniz")]
        [Display(Name = "Şifreniz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }
}
