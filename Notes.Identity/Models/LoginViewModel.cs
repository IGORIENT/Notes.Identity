using System.ComponentModel.DataAnnotations;

namespace Notes.Identity.Models
{
    public class LoginViewModel
    {
        [Required] //это поле обязательно
        public string? Username { get; set; }
        [Required]
        [DataType(DataType.Password)] // показывай его как скрытый пароль
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
