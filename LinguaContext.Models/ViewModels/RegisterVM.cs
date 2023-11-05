using System.ComponentModel.DataAnnotations;

namespace LinguaContext.Models.ViewModels;

public class RegisterVM
{
    public User User { get; set; }
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}