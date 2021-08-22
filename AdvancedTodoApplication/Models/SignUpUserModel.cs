using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedTodoApplication.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "Lutfen adınızı giriniz")]
        [Display(Name = "Adınız")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lutfen soyadınızı giriniz")]
        [Display(Name = "Soyadınız")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Lutfen email adresinizi giriniz")]
        [Display(Name = "Email adresiniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lutfen şifrenizi giriniz")]
        [Display(Name = "Şifreniz")]
        [Compare("ConfirmPassword", ErrorMessage ="şifreler birbiriyle eşleşmiyor")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Lutfen şifrenizi tekrar giriniz")]
        [Display(Name = "Şifrenizi tekrar giriniz")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
