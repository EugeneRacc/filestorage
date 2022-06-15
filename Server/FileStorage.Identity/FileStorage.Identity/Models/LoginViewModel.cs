using System.ComponentModel.DataAnnotations;

namespace FileStorage.Identity.Models
{
public class LoginViewModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
}
}