using System.ComponentModel.DataAnnotations;

namespace DASHMASTER.WEB.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username Cannot Be Empty!.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password Cannot Be Empty!.")]
        public string Password { get; set; }
    }
}
