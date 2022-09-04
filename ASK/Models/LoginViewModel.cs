using System.ComponentModel.DataAnnotations;

namespace ASK.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя!")]
        [MinLength(4, ErrorMessage = "Слишком короткое имя пользователя!")]
        [MaxLength(30, ErrorMessage = "Слишком длинное имя пользователя!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль!")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не менее 8 символов!")]
        [MaxLength(30, ErrorMessage = "Пароль должен быть не более 30 символов!")]
        public string UserPassword { get; set; }
    }
}
