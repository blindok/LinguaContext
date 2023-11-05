using LinguaContext.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LinguaContext.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, NotNull]
    [MaxLength(35)]
    [DisplayName("Никнейм")]
    public string UserName { get; set; }

    [Required, NotNull]
    [DisplayName("Почта")]
    public string Email { get; set; }

    [Required, NotNull]
    [MaxLength(35)]
    [DisplayName("Имя")]
    public string FirstName { get; set; }

    [MaxLength(35)]
    [DisplayName("Фамилия")]
    public string? LastName { get; set; }

    [Required, NotNull]
    [DisplayName("День Рождения")]
    public DateOnly? BirthDay { get; set; }

    [Required, NotNull]
    public string Role { get; set; } = SD.Role_User;

    [Required]
    public string PasswordHash { get; set; }

    [ValidateNever]
    public PersonalFactors? Factors { get; set; }
}
