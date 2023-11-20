using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LinguaContext.Models.ViewModels;

public class RegisterVM
{
    [Required]
    public required User User { get; set; }
    [Required]
    [DisplayName("Пароль")]
    public string Password { get; set; } = string.Empty;
    [Required]
    [DisplayName("Повторите Пароль")]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}